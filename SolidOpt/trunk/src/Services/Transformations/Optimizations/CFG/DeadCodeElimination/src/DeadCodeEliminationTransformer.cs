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
			DeadCodeElimination(source.Graph);
			return source;
		}
		#endregion
		
		public void DeadCodeElimination(List<CfgNode> graph)
		{
			// Mark
			BitArray marks = new BitArray(graph.Count);
			List<CfgNode> active = new List<CfgNode>();
			active.Add(graph[0]);
			while (active.Count != 0) {
				CfgNode current = active[0];
				int i = graph.IndexOf(current);
				if (!marks[i]) {
					marks[i] = true;
					if (current is Nodes) {
						DeadCodeElimination((current as Nodes).SubNodes);
					}
					foreach (CfgNode n in current.Successors) {
						int i1 = graph.IndexOf(n);
						if (!marks[i1]) active.Add(n);
					}
				}
				active.RemoveAt(0);
			}
			
			// Remove unmarked
			for (int i = graph.Count-1; i >= 0; i--) {
				if (!marks[i]) {
					graph.RemoveAt(i);
				}
			}
		}
	}
}

