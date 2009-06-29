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
			Instruction instruction;
			
			for (int i = 0; i < source.Body.Instructions.Count; i++) {
				instruction = source.Body.Instructions[i] as Instruction;
				if (instruction.OpCode == OpCodes.Nop) {
					source.Body.CilWorker.Remove(instruction);
				}
			}
						
			return source;
		}
	}
}
