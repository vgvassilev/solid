/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 24.6.2009 г.
 * Time: 17:23
 * 
 */
using System;

using Mono.Cecil;
using Mono.Cecil.Cil;

using SolidOpt.Optimizer.Transformers;

namespace NopRemover
{
	/// <summary>
	/// Description of NopRemoveTransformer.
	/// </summary>
	public class NopRemoveTransformer : ITransform<MethodDefinition>
	{
		public NopRemoveTransformer()
		{
		}
		
		public MethodDefinition Transform(MethodDefinition source)
		{
			
			CilWorker cil = source.Body.CilWorker;
			foreach (Instruction instr in source.Body.Instructions) {
				if (instr.OpCode == OpCodes.Nop)
					cil.Remove(instr);
			}
			return source;
		}
	}
}
