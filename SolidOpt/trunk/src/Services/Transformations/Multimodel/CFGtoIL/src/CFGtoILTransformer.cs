/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using SolidOpt.Services.Transformations.Multimodel;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

using Mono.Cecil;
using Mono.Cecil.Cil;

/// <summary>
/// The namespace contains classes helping the multimodel transformation from control flow grap
/// into intermediate language.
/// </summary>
namespace SolidOpt.Services.Transformations.Multimodel.CFGtoIL
{
  /// <summary>
  /// Control flow graph to CIL transforms a given control flow graph into a method definition.
  /// </summary>
  public class ControlFlowGraphToCil : ITransform<ControlFlowGraph, MethodDefinition>
  {

    #region ITransform implementation
    public MethodDefinition Transform (ControlFlowGraph source)
    {
      String name = source.Method.Name;
      MethodAttributes attrs = source.Method.Attributes;
      TypeReference returnType = source.Method.ReturnType;
      var result = new MethodDefinition(name, attrs, returnType);
      ILProcessor cil = result.Body.GetILProcessor();
      BasicBlock bb = source.Root;
      Instruction instr = null;
      int i = 0;
      while(bb != null) {
        instr = bb.First;
        while (instr != null) {
          cil.Append(instr);
          instr = instr.Next;
        }
        bb = (i < bb.Successors.Count) ? bb.Successors[i++] : null;
      }
      return result;
    }
    #endregion
    
  }
}
