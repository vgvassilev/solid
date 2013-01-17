/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

using SolidOpt.Services.Transformations.Optimizations;

namespace SolidOpt.Services.Transformations.Optimizations.IL.MethodInline
{
  /// <summary>
  /// Replaces method invocation with the invoked method itself. For correct replacement we need the method,
  /// which is going to be inlined to be maked with the attribute "Inlineable". The method should be pure, which 
  /// means that it shouldn't contain any side-effects.
  /// The action takes place at the level of IL. CALL is replaced by the inlinee method body. Before inlinee insert
  /// instruction that store actual parameters in a temporary local variables. The inlinee method body is copied
  /// and changed the instructions for reading and writing from/to the new local variables. Copy inlinee method
  /// instructions makes the relevant substitutions. RET instructions are replaced by unconditional branch at the
  /// end of the insertion of the method body. Inline make also copy of exception handlers.
  ///
  /// Inlining method example:
  /// <code>
  /// [Inlineable]
  /// int Inlinee (a, b)
  /// {
  ///   if (a &lt;= b)
  ///     return a * b;
  ///   else
  ///     return a + b;
  /// }
  /// 
  /// void Inliner ()
  /// {
  ///   ...
  ///   int sum = Inlinee(5, 8);
  /// }
  /// </code>
  /// After the transformation Inliner becomes:
  /// <code>
  /// void Inliner ()
  /// {
  ///   ...
  ///   int temp_result;
  ///  int temp_param_1 = 5;
  ///  int temp_param_2= 8;
  ///   if (temp_param_1 &lt;= temp_param_2)
  ///     result = temp_param_1 * temp_param_2;
  ///   else
  ///     result = temp_param_1 + temp_param_2;
  ///   int sum = result;
  /// }  
  /// </code>
  /// </summary>
  public class InlineTransformer : IOptimize<MethodDefinition>
  {
    public InlineTransformer()
    {
    }

    public MethodDefinition Transform(MethodDefinition source)
    {
      return Optimize(source);
    }
    
    public MethodDefinition Optimize(MethodDefinition source)
    {
      ILProcessor cil = source.Body.GetILProcessor();
      
      Instruction instruction = source.Body.Instructions[0];
      while (instruction != null) {
        if (instruction.OpCode.FlowControl == FlowControl.Call) {
          MethodDefinition inlineMethod = ((MethodReference)instruction.Operand).Resolve();
          if (IsInlineable(inlineMethod)) {
            Console.WriteLine(source.FullName + " call inline " + inlineMethod.FullName);
            
            // Clone inlinee method
            //TODO: crate copy of method
            inlineMethod.Body = null;
            
            //TODO: Think about inlineMethod.Body.SimplifyMacros();
            
            // Create temporary local variables for arguments
            List<VariableDefinition> tempVarArgList = new List<VariableDefinition>();
            foreach (ParameterDefinition param in inlineMethod.Parameters) {
              VariableDefinition v = new VariableDefinition(param.ParameterType);
              tempVarArgList.Add(v);
              source.Body.Variables.Add(v);
            }
            // Store arguments to temporary local variables
            for (int i = tempVarArgList.Count-1; i >= 0; i--) {
              cil.InsertBefore(instruction, Instruction.Create(OpCodes.Stloc, tempVarArgList[i]));
            }
            
            // Copy inlinee local variables to method locals
            List<VariableDefinition> tempVarLocalList = new List<VariableDefinition>();
            foreach (VariableDefinition vv in inlineMethod.Body.Variables) {
              VariableDefinition v = new VariableDefinition(vv.VariableType);
              tempVarLocalList.Add(v);
              source.Body.Variables.Add(v);
            }
            
            // Clone body and fixup instructions
            foreach (Instruction instruction1 in inlineMethod.Body.Instructions) {
              if (instruction1.OpCode == OpCodes.Ldarg || instruction1.OpCode == OpCodes.Ldarg_S) {
                instruction1.OpCode = OpCodes.Ldloc;
                instruction1.Operand = tempVarArgList[((ParameterDefinition)instruction1.Operand).Index];
              } else if (instruction1.OpCode == OpCodes.Ldarg_0) {
                instruction1.OpCode = OpCodes.Ldloc;
                instruction1.Operand = tempVarArgList[0];
              } else if (instruction1.OpCode == OpCodes.Ldarg_1) {
                instruction1.OpCode = OpCodes.Ldloc;
                instruction1.Operand = tempVarArgList[1];
              } else if (instruction1.OpCode == OpCodes.Ldarg_2) {
                instruction1.OpCode = OpCodes.Ldloc;
                instruction1.Operand = tempVarArgList[2];
              } else if (instruction1.OpCode == OpCodes.Ldarg_3) {
                instruction1.OpCode = OpCodes.Ldloc;
                instruction1.Operand = tempVarArgList[3];
              } else if (instruction1.OpCode == OpCodes.Ldarga || instruction1.OpCode == OpCodes.Ldarga_S) {
                instruction1.OpCode = OpCodes.Ldloca;
                instruction1.Operand = tempVarArgList[((ParameterDefinition)instruction1.Operand).Index];
              } else
              
              if (instruction1.OpCode == OpCodes.Starg || instruction1.OpCode == OpCodes.Starg_S) {
                instruction1.OpCode = OpCodes.Stloc;
                instruction1.Operand = tempVarArgList[((ParameterDefinition)instruction1.Operand).Index];
              } else
              
              if (instruction1.OpCode == OpCodes.Ldloc || instruction1.OpCode == OpCodes.Ldloc_S) {
                instruction1.OpCode = OpCodes.Ldloc;
                instruction1.Operand = tempVarLocalList[((VariableDefinition)instruction1.Operand).Index];
              } else if (instruction1.OpCode == OpCodes.Ldloc_0) {
                instruction1.OpCode = OpCodes.Ldloc;
                instruction1.Operand = tempVarLocalList[0];
              } else if (instruction1.OpCode == OpCodes.Ldloc_1) {
                instruction1.OpCode = OpCodes.Ldloc;
                instruction1.Operand = tempVarLocalList[1];
              } else if (instruction1.OpCode == OpCodes.Ldloc_2) {
                instruction1.OpCode = OpCodes.Ldloc;
                instruction1.Operand = tempVarLocalList[2];
              } else if (instruction1.OpCode == OpCodes.Ldloc_3) {
                instruction1.OpCode = OpCodes.Ldloc;
                instruction1.Operand = tempVarLocalList[3];
              } else if (instruction1.OpCode == OpCodes.Ldloca || instruction1.OpCode == OpCodes.Ldloca_S) {
                instruction1.OpCode = OpCodes.Ldloca;
                instruction1.Operand = tempVarLocalList[((VariableDefinition)instruction1.Operand).Index];
              } else
              
              if (instruction1.OpCode == OpCodes.Stloc || instruction1.OpCode == OpCodes.Stloc_S) {
                instruction1.OpCode = OpCodes.Stloc;
                instruction1.Operand = tempVarLocalList[((VariableDefinition)instruction1.Operand).Index];
              } else if (instruction1.OpCode == OpCodes.Stloc_0) {
                instruction1.OpCode = OpCodes.Stloc;
                instruction1.Operand = tempVarLocalList[0];
              } else if (instruction1.OpCode == OpCodes.Stloc_1) {
                instruction1.OpCode = OpCodes.Stloc;
                instruction1.Operand = tempVarLocalList[1];
              } else if (instruction1.OpCode == OpCodes.Stloc_2) {
                instruction1.OpCode = OpCodes.Stloc;
                instruction1.Operand = tempVarLocalList[2];
              } else if (instruction1.OpCode == OpCodes.Stloc_3) {
                instruction1.OpCode = OpCodes.Stloc;
                instruction1.Operand = tempVarLocalList[3];
              } else
              
              if (instruction1.OpCode == OpCodes.Ret) {
                if (instruction1.Next != null) {
                  instruction1.OpCode = OpCodes.Br;
                  instruction1.Operand = instruction;
                } else {
                  continue;
                }
              }
              
              cil.InsertBefore(instruction, instruction1);
            }
            
            // Clone exception handler
            foreach (ExceptionHandler handler in inlineMethod.Body.ExceptionHandlers) {
              source.Body.ExceptionHandlers.Add(handler);
            }
            
            instruction.OpCode = OpCodes.Nop;
            instruction.Operand = null;
            
            inlineMethod.Body = null;
          }
        }

        instruction = instruction.Next;
      }
      
      source.Body.OptimizeMacros();
      
      return source;
    }
    
    /// <summary>
    /// Checks if the method is appropriate for inlining. 
    /// </summary>
    /// <param name="method">
    /// A <see cref="MethodReference"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    private bool IsInlineable(MethodReference method)
    {
      foreach (CustomAttribute ca in method.Resolve().CustomAttributes) {
        if (ca.Constructor.DeclaringType.FullName == (typeof(InlineableAttribute).FullName)) {
          return true;
        }
      }
      return false;
    }
  }  
  

}
