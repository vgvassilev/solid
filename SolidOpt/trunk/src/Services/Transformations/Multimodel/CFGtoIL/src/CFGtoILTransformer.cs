/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;

using SolidOpt.Services.Transformations.Multimodel;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

using Mono.Cecil;
using Mono.Cecil.Cil;

/// <summary>
/// The namespace contains classes helping the multimodel transformation from control flow graph
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
      List<BasicBlock> notVisited = new List<BasicBlock>();
      List<BasicBlock> visited = new List<BasicBlock>();
      notVisited.Add(bb);
      while(bb != null) {
        instr = bb.First;
        while (instr != bb.Last.Next) {
          cil.Append(instr);
          instr = instr.Next;
        }
        notVisited.Remove(bb);
        visited.Add(bb);
        bb.Successors.Sort(new BBComparer());
        notVisited.AddRange(bb.Successors);
        GetExclusiveSet(ref notVisited, visited);
        bb = (notVisited.Count > 0) ? notVisited[0] : null;
      }
      return result;
    }

    #endregion

    private void GetExclusiveSet(ref List<BasicBlock> excludeFrom, List<BasicBlock> excludes) {
      foreach (BasicBlock bb in excludes)
        excludeFrom.Remove(bb);
    }
  }
  
  internal sealed class BBComparer : IComparer<BasicBlock> {
    public int Compare(BasicBlock x, BasicBlock y) {
      return x.First.Offset.CompareTo(y.First.Offset);
    }
  }           
}
