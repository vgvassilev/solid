/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;

using SolidOpt.Services.Transformations.Optimizations;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

namespace SolidOpt.Services.Transformations.Optimizations.CFG.DeadCodeElimination
{
	public class DeadCodeEliminationTransformer : IOptimize<ControlFlowGraph>
	{
		
		public DeadCodeEliminationTransformer()
		{
		}
		
		#region IOptimize<ControlFlowGraph> implementation
		public ControlFlowGraph Optimize(ControlFlowGraph source)
		{
			// Unreachable block is a block without predecessors, which is not the root block
			foreach(BasicBlock block in source.RawBlocks) {
				if (block != source.Root)
					if (block.Predecessors.Count == 0)
						source.RawBlocks.Remove(block);
			}

			return source;
		}
		#endregion
		
	}
}

