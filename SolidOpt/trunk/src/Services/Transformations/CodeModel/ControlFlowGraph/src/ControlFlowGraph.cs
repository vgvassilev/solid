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

		private List<BasicBlock> rawBlocks = null;
		public List<BasicBlock> RawBlocks {
			get { return this.rawBlocks; }
		}		
		
		#endregion

		public ControlFlowGraph(BasicBlock root, List<BasicBlock> rawBlocks)
		{
			this.root = root;
			this.rawBlocks = rawBlocks;
		}

	}
}
