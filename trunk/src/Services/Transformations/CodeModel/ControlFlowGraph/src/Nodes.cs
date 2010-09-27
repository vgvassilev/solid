/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
 
using System;

using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph
{
	/// <summary>
	/// Description of Nodes.
	/// </summary>
	public class Nodes : CfgNode
	{
		
		#region Constructors
		
		public Nodes(Instruction first) : base(first)
		{
		}
		
		#endregion
		
	}
}
