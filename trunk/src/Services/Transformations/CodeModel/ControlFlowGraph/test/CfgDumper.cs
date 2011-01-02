/*
 * $Id:
 * Author: Vassil Vassilev (vasil.georgiev.vasilev@cern.ch)
 */

using System;
using System.Text;
using Mono.Cecil.Cil;

using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph.Test
{
	/// <summary>
	/// The test/cases folder contains multiple il files that contain text representation
	/// of a control flow graph. The purpose ot these files is to verify that the both 
	/// transformations from IL to CFG and from CFG to IL are correct. 
	/// </summary>	
	public static class CfgDumper
	{
		/// <summary>
		/// The extension method, which is intended to dump the control flow graph in the 
		/// same way as it is in the test/cases folder.
		/// </summary>
		/// <param name="cfg">
		/// A <see cref="ControlFlowGraph"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public static string Dump(this ControlFlowGraph cfg) {
			StringBuilder sb = new StringBuilder();
			foreach (CfgNode node in cfg.Graph) {
								
				sb.AppendLine(String.Format("block {0}:", cfg.Graph.IndexOf(node)));
				sb.AppendLine("\tbody:");
				foreach (Instruction instruction in node) {
					sb.AppendLine("\t\t");
					sb.AppendLine(instruction.ToString());
					sb.AppendLine();
				}
				
				if (node.Successors != null && node.Successors.Count > 0) {
					sb.AppendLine("\tsuccessors:");
					foreach (CfgNode successor in node.Successors) {
						sb.AppendLine(String.Format("\t\tblock {0}", cfg.Graph.IndexOf(successor)));
					}
				}
			}
			
			return sb.ToString();
		}
	}
}
