/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph
{


	public class InstructionData {

		public readonly int StackBefore;
		public readonly int StackAfter;

		public InstructionData (int before, int after)
		{
			this.StackBefore = before;
			this.StackAfter = after;
		}
	}
}
