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

	public class CfgNode : IEnumerable<Instruction>, IComparable<CfgNode>
	{
		#region Fields & Properties
		
		public static readonly List<CfgNode> NoSuccessors = new List<CfgNode>(0);
		public static readonly List<CfgNode> NoPredecessor = new List<CfgNode>(0);

		int index;
		Instruction first;		
		public Instruction First {
			get { return first; }
			set { first = value; }
		}

		Instruction last;
		public Instruction Last {
			get { return last; }
			set { last = value; }
		}
		
		List<CfgNode> successors;
		public List<CfgNode> Successors {
			get { return successors;  }
			set { successors = value; }
		}		
		
		List<CfgNode> predecessor = NoPredecessor;		
		public List<CfgNode> Predecessor {
			get { return predecessor;  }
			set { predecessor = value; }
		}

		public int Index {
			get { return index; }
			set { index = value; }
		}
		#endregion
		
		#region Constructors
		
		public CfgNode (Instruction first)
		{
			if (first == null)
				throw new ArgumentNullException ("first");

			this.first = first;
		}		
		
		#endregion
		

		#region IComparable

		public int CompareTo (CfgNode node)
		{
			return first.Offset - node.First.Offset;
		}
		
		#endregion
		
		#region IEnumerable
		
		public IEnumerator<Instruction> GetEnumerator ()
		{
			var instruction = first;
			while (true) {
				yield return instruction;

				if (instruction == last)
					yield break;

				instruction = instruction.Next;
			}
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
		
		#endregion
	}
}
