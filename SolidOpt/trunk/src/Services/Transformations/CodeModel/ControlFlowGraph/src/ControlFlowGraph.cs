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
	/// <summary>
	/// Implementation of a Control Flow Graph structure. The CFG represents the
	/// the method body as a graph, which consist of nodes. Every node of the CFG
	/// contains linear block of CIL instructions. A node can have successors and
	/// predecessors, which represent the explicit change (branches) of the 
	/// control flow.
	/// </summary>
	public class ControlFlowGraph {
		
		#region Fields & Properties
		
		MethodBody body;
		public MethodBody MethodBody {
			get { return body; }
		}

		List<CfgNode> graph;
		public List<CfgNode> Graph {
			get { return graph; }
		}		
		
		#endregion

		public ControlFlowGraph(MethodBody body, List<CfgNode> graph)
		{
			this.body = body;
			this.graph = graph;
		}
	
		public override string ToString()
		{
			StringWriter writer = new StringWriter ();
			FormatControlFlowGraph (writer);
			return writer.ToString ();
		}

		public void FormatControlFlowGraph(TextWriter writer)
		{
			foreach (CfgNode node in Graph) {
								
				writer.WriteLine ("block {0}:", Graph.IndexOf(node));
				writer.WriteLine ("\tbody:");
				foreach (Instruction instruction in node) {
					writer.Write ("\t\t");
//					var data = GetData (instruction);
//					writer.Write ("[{0}:{1}] ", data.StackBefore, data.StackAfter);
					writer.Write(instruction);
					writer.WriteLine ();
				}
				
				
				if (node.Successors != null && node.Successors.Count > 0) {
					writer.WriteLine ("\tsuccessors:");
					foreach (CfgNode successor in node.Successors) {
						writer.WriteLine ("\t\tblock {0}", Graph.IndexOf(successor));
					}
				}
			}
		}
		
		public void FormatControlFlowGraphNode(TextWriter writer, CfgNode node, string indent)
		{
			
			indent += "\t";
			
			writer.WriteLine (indent + "node: {0} ", Graph.IndexOf(node));
			writer.WriteLine (indent + "body:");
			
			indent += "\t";
			Instruction instruction = (node == null) ? null : node.First;
			while (instruction != null && instruction.Next != null) {
				writer.Write(indent);
				writer.Write(instruction);
				writer.WriteLine();
				instruction = instruction.Next;
			}
			
			if (node != null && node.Successors != null && node.Successors.Count > 0) {
				writer.WriteLine (indent + "\tsuccessors:");
				foreach (CfgNode successor in node.Successors) {
					FormatControlFlowGraphNode(writer, successor, indent);
				}
			}
			
		}
	}
}
