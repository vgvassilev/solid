/*
 *
 * User: Vassil Vassilev
 * Date: 18.11.2009 г.
 * Time: 22:18
 * 
 */
using System;

using SolidOpt.Services.Transformations.Multimodel;

using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph
{
	/// <summary>
	/// Description of CilToControlFlowGraph.
	/// </summary>
	public class CilToControlFlowGraph : IDecompile<MethodBody, InstructionBlock []>
	{
		
		#region Fields & Properties
		
		
		#endregion
		
		
		public CilToControlFlowGraph()
		{
		}
		
		public InstructionBlock[] Decompile(MethodBody source)
		{
			throw new NotImplementedException();
		}
	}
}
