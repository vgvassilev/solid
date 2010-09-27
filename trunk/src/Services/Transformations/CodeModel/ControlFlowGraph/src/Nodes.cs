/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
 
using System;

using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph
{
	/// <summary>
	/// Description of Nodes.
	/// </summary>
	public class Nodes : CfgNode, IEnumerable<Instruction>
	{
		
		#region Fields & Properties
		
		List<CfgNode> subNodes = new List<CfgNode>();
		public List<CfgNode> SubNodes {
			get { return subNodes; }
			set { subNodes = value; }
		}
		
		#endregion
		
		#region Constructors
		
		public Nodes()
		{
		}
		
		public Nodes(List<CfgNode> subNodes)
		{
			this.subNodes = subNodes;
		}
		
		#endregion

		#region IEnumerable
		
		public override IEnumerator<Instruction> GetEnumerator()
		{
			var instruction = First;
			while (true) {
				yield return instruction;

				if (instruction == Last)
					yield break;

				instruction = instruction.Next;
			}
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
		
		public override IEnumerable<CfgNode> GetNodesEnumerator()
		{
			foreach (CfgNode node in SubNodes) yield return node;
		}

		#endregion		
	}
}
