/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using SolidOpt.Services.Transformations.Multimodel;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoCFG
{
	/// <summary>
	/// Description of CilToControlFlowGraph.
	/// </summary>
	public class CilToControlFlowGraph : DecompilationStep, IDecompile<MethodDefinition, ControlFlowGraph>
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
			return builder.Create();
		}
		
		
		public ControlFlowGraph Decompile(MethodDefinition source)
		{
			return Process(source.Body);
		}
	}
}
