/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph
{
	public class ControlFlowGraph {

		MethodBody body;
		Nodes nodes;
//		Dictionary<int, InstructionData> data;
//		List<ExceptionHandlerData> exception_data;
//		HashSet<int> exception_objects_offsets;

		public MethodBody MethodBody {
			get { return body; }
		}

		public Nodes Nodes {
			get { return nodes; }
		}

		public ControlFlowGraph(
			MethodBody body,
			Nodes nodes)
//			Dictionary<int, InstructionData> instructionData,
//			List<ExceptionHandlerData> exception_data,
//			HashSet<int> exception_objects_offsets
		{
			this.body = body;
			this.nodes = nodes;
//			this.data = instructionData;
//			this.exception_data = exception_data;
//			this.exception_objects_offsets = exception_objects_offsets;
		}

//		public InstructionData GetData(Instruction instruction)
//		{
//			return data[instruction.Offset];
//		}

//		public ExceptionHandlerData[] GetExceptionData()
//		{
//			return exception_data.ToArray();
//		}

//		public bool HasExceptionObject(int offset)
//		{
//			if (exception_objects_offsets == null)
//				return false;
//
//			return exception_objects_offsets.Contains(offset);
//		}	
		
		public override string ToString()
		{
			StringWriter writer = new StringWriter ();
			FormatControlFlowGraph (writer);
			return writer.ToString ();
		}

		public void FormatControlFlowGraph(TextWriter writer)
		{
			foreach (Node node in Nodes.GetNodesEnumerator()) {
				writer.WriteLine ("block {0}:", node.Index);
				writer.WriteLine ("\tbody:");
				foreach (Instruction instruction in node) {
					writer.Write ("\t\t");
//					var data = GetData (instruction);
//					writer.Write ("[{0}:{1}] ", data.StackBefore, data.StackAfter);
					writer.Write(instruction);
					writer.WriteLine ();
				}
				List<CfgNode> successors = node.Successors;
				if (successors.Count > 0) {
					writer.WriteLine ("\tsuccessors:");
					foreach (CfgNode successor in successors) {
						writer.WriteLine ("\t\tblock {0}", successor.Index);
					}
				}
			}
		}
		
	}
}
