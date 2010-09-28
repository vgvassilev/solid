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

		public MethodBody MethodBody {
			get { return body; }
		}

		public Nodes Nodes {
			get { return nodes; }
		}

		public ControlFlowGraph(MethodBody body, Nodes nodes)
		{
			this.body = body;
			this.nodes = nodes;
		}
	
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
