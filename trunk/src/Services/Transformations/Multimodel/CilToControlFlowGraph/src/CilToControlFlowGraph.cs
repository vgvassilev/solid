/*
 *
 * User: Vassil Vassilev
 * Date: 18.11.2009 г.
 * Time: 22:18
 * 
 */
using System;
using System.Collections.Generic;

using SolidOpt.Services.Transformations.Multimodel;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.Multimodel.CilToControlFlowGraph
{
	/// <summary>
	/// Description of CilToControlFlowGraph.
	/// </summary>
	public class CilToControlFlowGraph : DecompilationStep
	{		
		#region Constructors
		
		public CilToControlFlowGraph ()
		{
		}
		
		#endregion
		
		public override object Process (object codeModel)
		{
			return Process (codeModel as MethodBody);
		}
		

		public ControlFlowGraph Process (MethodBody source)
		{
			if (source == null)
				throw new ArgumentNullException ("method");
			if (!source.Method.HasBody)
				throw new ArgumentException ();

			var builder = new ControlFlowGraphBuilder (source.Method);
			return builder.CreateGraph ();
		}
		
	}
}
