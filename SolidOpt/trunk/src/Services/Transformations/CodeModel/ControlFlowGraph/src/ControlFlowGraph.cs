/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph
{
  /// <summary>
  /// Implementation of a Control Flow Graph structure. The CFG represents the
  /// the method body as a graph, which consist of nodes. Every node of the CFG
  /// contains linear block of CIL instructions. A node can have successors and
  /// predecessors, which represent the explicit change (branches) of the 
  /// control flow.
  /// </summary>
  public class ControlFlowGraph {
    
    #region Fields & Properties
    
    private BasicBlock root;
    public BasicBlock Root {
      get { return root; }
    }

    private MethodDefinition method;
    public MethodDefinition Method {
        get { return method; }
    }

    private List<BasicBlock> rawBlocks = null;
    public List<BasicBlock> RawBlocks {
      get { return this.rawBlocks; }
    }    
    
    #endregion

    public ControlFlowGraph(MethodDefinition method, BasicBlock root, List<BasicBlock> rawBlocks)
    {
      this.method = method;
      this.root = root;
      this.rawBlocks = rawBlocks;
    }

    public override string ToString ()
    {
      StringBuilder sb = new StringBuilder();
      
      foreach (BasicBlock block in RawBlocks) {
       sb.AppendLine(String.Format("block {0}:", block.Name));
       sb.AppendLine(String.Format("  kind: {0}", block.Kind.ToString().ToLower()));
       sb.AppendLine("  body:");
       foreach (Instruction instruction in block)
         sb.AppendLine(String.Format("    {0}", instruction.ToString()));
      
       if (block.Successors != null && block.Successors.Count > 0) {
         sb.AppendLine("  successors:");
         foreach (BasicBlock succ in block.Successors) {
           sb.AppendLine(String.Format("    block {0}", succ.Name));
         }
       }
      
       if (block.Predecessors != null && block.Predecessors.Count > 0) {
         sb.AppendLine("  predecessors:");
         foreach (BasicBlock pred in block.Predecessors) {
           sb.AppendLine(String.Format("    block {0}", pred.Name));
         }
       }
      }
      
      return sb.ToString();

    }
  }
}
