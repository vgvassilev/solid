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

	public class CfgNode: IEnumerable<Instruction>, IComparable<CfgNode>
	{
		#region Fields & Properties
		
		public static readonly List<CfgNode> Empty = new List<CfgNode>(0);

		int index;
		public int Index {
			get { return index; }
			set { index = value; }
		}
		
		List<CfgNode> successors;
		public List<CfgNode> Successors {
			get { return successors;  }
			set { successors = value; }
		}		
		
		List<CfgNode> predecessor;
		public List<CfgNode> Predecessor {
			get { return predecessor;  }
			set { predecessor = value; }
		}

		Instruction first;
		public virtual Instruction First {
			get { return first; }
			set { first = value; }
		}

		Instruction last;
		public virtual Instruction Last {
			get { return last; }
			set { last = value; }
		}

		#endregion
		
		#region Constructors
		
		public CfgNode()
		{
		}
		
		public CfgNode(List<CfgNode> successors, List<CfgNode> predecessor)
		{
			this.successors = successors;
			this.predecessor = predecessor;
		}

		public CfgNode(Instruction first)
		{
			if (first == null)
				throw new ArgumentNullException ("first");

			this.first = first;
		}
		
		public CfgNode(Instruction first, Instruction last)
		{
			if (first == null || last == null)
				throw new ArgumentNullException ("first");

			this.first = first;
			this.last = last;
		}

		#endregion
				
		public bool Contains(Instruction instruction)
		{
			return (instruction.Offset >= First.Offset && instruction.Offset <= Last.Offset);
			
		}
		
		#region IComparable

		public int CompareTo(CfgNode node)
		{
			return first.Offset - node.First.Offset;
		}
		
		#endregion

		#region IEnumerable
		
		public virtual IEnumerator<Instruction> GetEnumerator()
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
		
		public virtual IEnumerable<CfgNode> GetNodesEnumerator()
		{
			yield return this;
		}

		#endregion		
	}
}
