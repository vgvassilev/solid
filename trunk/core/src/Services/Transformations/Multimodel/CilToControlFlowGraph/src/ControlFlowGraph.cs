// 
//  ControlFlowGraph.cs
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
using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph
{
	public class ControlFlowGraph {

		MethodBody body;
		InstructionBlock [] blocks;
		Dictionary<int, InstructionData> data;
		List<ExceptionHandlerData> exception_data;
		HashSet<int> exception_objects_offsets;

		public MethodBody MethodBody {
			get { return body; }
		}

		public InstructionBlock [] Blocks {
			get { return blocks; }
		}

		public ControlFlowGraph (
			MethodBody body,
			InstructionBlock [] blocks,
			Dictionary<int, InstructionData> instructionData,
			List<ExceptionHandlerData> exception_data,
			HashSet<int> exception_objects_offsets)
		{
			this.body = body;
			this.blocks = blocks;
			this.data = instructionData;
			this.exception_data = exception_data;
			this.exception_objects_offsets = exception_objects_offsets;
		}

		public InstructionData GetData (Instruction instruction)
		{
			return data [instruction.Offset];
		}

		public ExceptionHandlerData [] GetExceptionData ()
		{
			return exception_data.ToArray ();
		}

		public bool HasExceptionObject (int offset)
		{
			if (exception_objects_offsets == null)
				return false;

			return exception_objects_offsets.Contains (offset);
		}

		public static ControlFlowGraph Create (MethodDefinition method)
		{
			if (method == null)
				throw new ArgumentNullException ("method");
			if (!method.HasBody)
				throw new ArgumentException ();

			var builder = new ControlFlowGraphBuilder (method);
			return builder.CreateGraph ();
		}
	}
}
