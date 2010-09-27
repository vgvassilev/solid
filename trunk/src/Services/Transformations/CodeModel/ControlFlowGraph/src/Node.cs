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
	public class Node : CfgNode, IEnumerable<Instruction>, IComparable<Node>
	{
		#region Constructors
		
		public Node(Instruction first)
		{
			if (first == null)
				throw new ArgumentNullException ("first");

			this.first = first;
		}		
		
		public Node(Instruction first, Instruction last)
		{
			if (first == null || last == null)
				throw new ArgumentNullException ("first");

			this.first = first;
			this.last = last;
		}				
//		public Node(Instruction first) : base (first){}
//		public Node(Instruction first, Instruction last) : base (first, last){}
//		
		#endregion
				
		#region Fields & Properties
		
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
		
		#endregion
		
		#region IComparable

		public int CompareTo (Node node)
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
