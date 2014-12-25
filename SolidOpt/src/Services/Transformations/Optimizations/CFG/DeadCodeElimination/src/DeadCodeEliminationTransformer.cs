/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;

using Mono.Cecil.Cil;

using SolidOpt.Services.Transformations.Optimizations;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

namespace SolidOpt.Services.Transformations.Optimizations.CFG.DeadCodeElimination
{
  public class DeadCodeEliminationTransformer : IOptimize<ControlFlowGraph<Instruction>>
  {
    
    public DeadCodeEliminationTransformer()
    {
    }

    public ControlFlowGraph<Instruction> Transform(ControlFlowGraph<Instruction> source)
    {
      return Optimize(source);
    }

    #region IOptimize<ControlFlowGraph> implementation
    public ControlFlowGraph<Instruction> Optimize(ControlFlowGraph<Instruction> source)
    {
      // Unreachable block is a block without predecessors, which is not the root block
      foreach(BasicBlock<Instruction> block in source.RawBlocks) {
        if (block != source.Root)
          if (block.Predecessors.Count == 0)
            source.RawBlocks.Remove(block);
      }

      return source;
    }
    #endregion
    
  }
}

