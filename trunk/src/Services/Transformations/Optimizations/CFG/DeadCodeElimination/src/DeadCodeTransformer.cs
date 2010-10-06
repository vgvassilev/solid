/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using SolidOpt.Services.Transformations.Optimizations;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

namespace SolidOpt.Services.Transformations.Optimizations.CFG.DeadCodeElimination
{
	public class DeadCodeTransformer : IOptimize<ControlFlowGraph>
	{
		
		public DeadCodeTransformer ()
		{
		}
		
		#region IOptimize<ControlFlowGraph> implementation
		public ControlFlowGraph Optimize (ControlFlowGraph source)
		{
			throw new System.NotImplementedException();
		}
		
		#endregion
		
	}
}

