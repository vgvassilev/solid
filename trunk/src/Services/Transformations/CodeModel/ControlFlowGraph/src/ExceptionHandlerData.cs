/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using Mono.Cecil;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph
{


	public class CatchHandlerData 
	{

		public readonly TypeReference Type;
		public readonly BlockRange Range;

		public CatchHandlerData (TypeReference type, BlockRange range)
		{
			Type = type;
			Range = range;
		}
	}

	public class ExceptionHandlerData : IComparable<ExceptionHandlerData> {

		BlockRange try_range;
		List<CatchHandlerData> catches = new List<CatchHandlerData> ();
		BlockRange finally_range;
		BlockRange fault_range;

		public BlockRange TryRange {
			get { return try_range; }
			set { try_range = value; }
		}

		public List<CatchHandlerData> Catches {
			get { return catches; }
		}

		public BlockRange FinallyRange {
			get { return finally_range; }
			set { finally_range = value; }
		}

		public BlockRange FaultRange {
			get { return fault_range; }
			set { fault_range = value; }
		}

		public ExceptionHandlerData (BlockRange try_range)
		{
			this.try_range = try_range;
		}

		public int CompareTo (ExceptionHandlerData data)
		{
			return try_range.Start.First.Offset - data.try_range.Start.First.Offset;
		}
	}
}
