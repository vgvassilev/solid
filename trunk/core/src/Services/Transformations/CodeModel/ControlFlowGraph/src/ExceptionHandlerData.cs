// 
//  ExceptionHandlerData.cs
//  
//  Author:
//       vvassilev <vasil.georgiev.vasilev@cern.ch>
//  
//  Copyright (c) 2010 vvassilev
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
