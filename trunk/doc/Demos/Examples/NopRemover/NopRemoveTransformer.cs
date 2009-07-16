/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 24.6.2009 г.
 * Time: 17:23
 * 
 */
using System;
using System.Collections.Generic;

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
//			HashSet<Instruction> removed = new HashSet<Instruction>();
//			CilWorker cil = source.Body.CilWorker;
////			for (int i = source.Body.Instructions.Count-1; i >= 0; i--) {
////				Instruction instruction = source.Body.Instructions[i];
////				if (instruction.OpCode == OpCodes.Nop) {
////					removed.Add(instruction);
////					cil.Remove(instruction);
////				}
////			}
//			Instruction instruction1 = source.Body.Instructions[source.Body.Instructions.Count-1];
//			while (instruction1 != null) {
//				if (instruction1.OpCode == OpCodes.Nop) {
//					removed.Add(instruction1);
//					cil.Remove(instruction1);
//				}
//				instruction1 = instruction1.Previous;
//			}
//			foreach (Instruction instruction in source.Body.Instructions) {
//				while (removed.Contains(instruction.Operand as Instruction))
//					instruction.Operand = (instruction.Operand as Instruction).Next;
//			}
//			
			
			CilWorker cil = source.Body.CilWorker;
			
			foreach (Instruction instruction in source.Body.Instructions) {
				if (instruction.OpCode.FlowControl == FlowControl.Branch || 
	    			instruction.OpCode.FlowControl == FlowControl.Cond_Branch) {
					
					while (((Instruction)instruction.Operand).OpCode == OpCodes.Nop) {
						if (((Instruction)instruction.Operand).Next == null)
							break;
						instruction.Operand = ((Instruction)instruction.Operand).Next;
					}
				}
			}
			
			//Instruction instruction1 = source.Body.Instructions[source.Body.Instructions.Count-1];
			Instruction instruction1 = source.Body.Instructions[1];
			while (instruction1 != null) {
				if (instruction1.OpCode == OpCodes.Nop) {
//					int i = source.Body.Instructions.IndexOf(instruction1);
//					if (instruction1.Next != null) {
//						instruction1.Next.Previous = instruction1.Previous;
//					}
//					if (instruction1.Previous != null) {
//						instruction1.Previous.Next = instruction1.Next;
//					}
//					source.Body.Instructions.RemoveAt(i);
					cil.Remove(instruction1);
				}
				instruction1 = instruction1.Previous;
			}
			
			source.Body.Optimize();
			
			return source;
		}
	}
}
