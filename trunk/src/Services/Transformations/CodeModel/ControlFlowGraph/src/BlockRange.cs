/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph
{

	public class BlockRange 
	{

		public readonly InstructionBlock Start;
		public readonly InstructionBlock End;

		public BlockRange (InstructionBlock start, InstructionBlock end)
		{
			Start = start;
			End = end;
		}
	}
}
