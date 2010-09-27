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
		public readonly Nodes Range;

		public CatchHandlerData (TypeReference type, Nodes range)
		{
			Type = type;
			Range = range;
		}
	}

	public class ExceptionHandlerData : IComparable<ExceptionHandlerData> {

		Nodes try_range;
		List<CatchHandlerData> catches = new List<CatchHandlerData> ();
		Nodes finally_range;
		Nodes fault_range;

		public Nodes TryRange {
			get { return try_range; }
			set { try_range = value; }
		}

		public List<CatchHandlerData> Catches {
			get { return catches; }
		}

		public Nodes FinallyRange {
			get { return finally_range; }
			set { finally_range = value; }
		}

		public Nodes FaultRange {
			get { return fault_range; }
			set { fault_range = value; }
		}

		public ExceptionHandlerData (Nodes try_range)
		{
			this.try_range = try_range;
		}

		public int CompareTo (ExceptionHandlerData data)
		{
			throw new NotImplementedException();
//			return try_range.Start.First.Offset - data.try_range.Start.First.Offset;
		}
	}
}
