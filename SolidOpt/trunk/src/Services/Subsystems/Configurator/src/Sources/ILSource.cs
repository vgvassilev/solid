/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

using SolidOpt.Services.Subsystems.HetAccess;

using System.Diagnostics;
using System.Reflection;

//    Ldnull,
//    Ldc_I4_M1,
//    Ldc_I4_0,
//    Ldc_I4_1,
//    Ldc_I4_2,
//    Ldc_I4_3,
//    Ldc_I4_4,
//    Ldc_I4_5,
//    Ldc_I4_6,
//    Ldc_I4_7,
//    Ldc_I4_8,
//    Ldc_I4_S,
//    Ldc_I4,
//    Ldc_I8,
//    Ldc_R4,
//    Ldc_R8,
//*    Ldobj,
//    Ldstr,
//*    Newobj,
//    Stfld,
//    Stsfld,
//*    Stobj,
//*    Newarr,
//?    Ldtoken,


namespace SolidOpt.Services.Subsystems.Configurator.Sources
{
  /// <summary>
  /// Generates Configuration Intermediate Representation (Dictionary) from assembly.
  /// </summary>
  public class ILSource<TParamName> : IConfigSource<TParamName>
  {
    public ILSource()
    {
      Trace.Listeners.Add(new ConsoleTraceListener());
    }
    
    public bool CanParse(Uri resUri, Stream resStream)
    {
      return (resUri.IsFile && Path.GetExtension(resUri.LocalPath).ToLower() == ".dll");
    }
    
    public Dictionary<TParamName, object> LoadConfiguration(Stream resStream)
    {
      //Mono.Cecil 0.9.3 migration: AssemblyDefinition assembly = AssemblyFactory.GetAssembly(resStream);
      AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(resStream);
      //Mono.Cecil 0.9.3 migration: assembly.MainModule.Accept(new StructureVisitor<TParamName>());
      Dictionary<TParamName, object> dict = new Dictionary<TParamName, object>();
      foreach(ModuleDefinition module in assembly.Modules) {
        foreach(TypeDefinition type in module.Types) {
          if (type.Name != "<Module>") {
            if (type.Name != "Global") {
              Dictionary<TParamName, object> subDict = new Dictionary<TParamName, object>();
              VisitConfigTypes(type, subDict);
              dict[(TParamName)Convert.ChangeType(type.Name, typeof(TParamName))] = subDict;
            } else {
              VisitConfigTypes(type, dict);
            }
          }
        }
      }
      
      return dict;
    }
    
    //FIXME: Fix the VisitConfigTypes because it is no more visitor...
    internal void VisitConfigTypes(TypeDefinition type, Dictionary<TParamName, object> dict)
    {
      foreach (FieldDefinition field in type.Fields) {
        dict[(TParamName)Convert.ChangeType(field.Name, typeof(TParamName))] = GetFieldValue(type.Methods, field);
      }
      
      foreach (TypeDefinition nestedType in type.NestedTypes) {
        Dictionary<TParamName, object> subDict = new Dictionary<TParamName, object>();
        VisitConfigTypes(nestedType, subDict);
        dict[(TParamName)Convert.ChangeType(nestedType.Name, typeof(TParamName))] = subDict;
      }
    }
    
    internal object GetFieldValue(Collection<MethodDefinition> collection, FieldDefinition field)
    {
      foreach (MethodDefinition cctor in collection) {
        if (cctor.IsConstructor && cctor.IsStatic && cctor.IsHideBySig) {
          foreach (Instruction instr in cctor.Body.Instructions) {
            if (instr.Next.Operand != null && 
                (instr.Next.OpCode == OpCodes.Stsfld &&
                (instr.Next.Operand as FieldDefinition).Name == field.Name)) {
              switch (instr.OpCode.Code) {
                case Code.Ldstr : 
                  return instr.Operand;
                case Code.Ldc_I4_M1 :
                  return -1;
                case Code.Ldc_I4_0 :
                  return 0;
                case Code.Ldc_I4_1 :
                  return 1;
                case Code.Ldc_I4_2 :
                  return 2;
                case Code.Ldc_I4_3 :
                  return 3;
                case Code.Ldc_I4_4 :
                  return 4;
                case Code.Ldc_I4_5 :
                  return 5;
                case Code.Ldc_I4_6 :
                  return 6;
                case Code.Ldc_I4_7 :
                  return 7;
                case Code.Ldc_I4_8 :
                  return 8;
                  
                case Code.Ldc_I4_S :
                case Code.Ldc_I4 :
                case Code.Ldc_I8 :
                  return instr.Operand;
                case Code.Ldnull :
                  return null;
                case Code.Ldc_R4 :
                case Code.Ldc_R8 :
                  return instr.Operand;
                  
              }
            }
          }
        }
      }
      return null;
    }
  }
  
  //Mono.Cecil 0.9.3 migration: 
  /*
  #region Visitors
  internal class StructureVisitor<TParamName>: BaseStructureVisitor
  {
    public override void VisitModuleDefinition(ModuleDefinition module)
    {        
      module.Accept(new ReflectionVisitor<TParamName>());
    }
  }

  internal class ReflectionVisitor<TParamName>: BaseReflectionVisitor
  {
    internal bool isFirstLevel = true;
    
    public override void VisitTypeDefinitionCollection(Collection<TypeDefinition> types)
    {
      base.VisitCollection(types);
    }
    
    public override void VisitTypeDefinition(TypeDefinition type)
    {
      if (!type.IsNestedPublic) {
        if (isFirstLevel && type.Name != "<Module>" && type.Name != "Global") {
          isFirstLevel = false;
          Dictionary<TParamName, object> subDict = new Dictionary<TParamName, object>();
          VisitConfigTypes(type, subDict);
          ILSource<TParamName>.dict[(TParamName)Convert.ChangeType(type.Name, typeof(TParamName))] = subDict;
        }
        else
          VisitConfigTypes(type, ILSource<TParamName>.dict);
      }
    }
    
    internal void VisitConfigTypes(TypeDefinition type, Dictionary<TParamName, object> dict)
    {
      foreach (FieldDefinition field in type.Fields) {
        dict[(TParamName)Convert.ChangeType(
          field.Name, typeof(TParamName))] = GetFieldValue(type.Methods, field);
      }
      
      foreach (TypeDefinition t in type.NestedTypes) {
        Dictionary<TParamName, object> subDict = new Dictionary<TParamName, object>();
        VisitConfigTypes(t, subDict);
        dict[(TParamName)Convert.ChangeType(t.Name, typeof(TParamName))] = subDict;
      }
    }
    
    //Mono 0.9 migration
    internal object GetFieldValue(Collection<MethodReference> collection, FieldDefinition field)
    {
      foreach (MethodDefinition cctor in collection) {
        if (cctor.IsConstructor && cctor.IsStatic && cctor.IsSpecialName && cctor.IsHideBySig) {
          foreach (Instruction instr in cctor.Body.Instructions) {
            if (instr.Next.Operand != null && 
                (instr.Next.OpCode == OpCodes.Stsfld &&
                (instr.Next.Operand as FieldDefinition).Name == field.Name)) {
              switch (instr.OpCode.Code) {
                case Code.Ldstr : 
                  return instr.Operand;
                case Code.Ldc_I4_M1 :
                  return -1;
                case Code.Ldc_I4_0 :
                  return 0;
                case Code.Ldc_I4_1 :
                  return 1;
                case Code.Ldc_I4_2 :
                  return 2;
                case Code.Ldc_I4_3 :
                  return 3;
                case Code.Ldc_I4_4 :
                  return 4;
                case Code.Ldc_I4_5 :
                  return 5;
                case Code.Ldc_I4_6 :
                  return 6;
                case Code.Ldc_I4_7 :
                  return 7;
                case Code.Ldc_I4_8 :
                  return 8;
                  
                case Code.Ldc_I4_S :
                case Code.Ldc_I4 :
                case Code.Ldc_I8 :
                  return instr.Operand;
                case Code.Ldnull :
                  return null;
                case Code.Ldc_R4 :
                case Code.Ldc_R8 :
                  return instr.Operand;
                  
              }
            }
          }
        }
      }
      return null;
    }
  }
  #endregion
  */
}