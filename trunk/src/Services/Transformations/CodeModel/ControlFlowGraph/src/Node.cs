/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
 
using System;
using System.Collections;
using System.Collections.Generic;

using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph
{
	/// <summary>
	/// Description of Node.
	/// </summary>
	public class Node : CfgNode
	{
		#region Fields & Properties
				
		#endregion
		
		#region Constructors
		
		public Node(): base () {}
		public Node(List<CfgNode> successors, List<CfgNode> predecessor): base (successors, predecessor) {}
		public Node(Instruction first) : base (first) {}
		public Node(Instruction first, Instruction last) : base (first, last) {}
		
		#endregion
	}
}
