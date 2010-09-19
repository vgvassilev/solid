/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

using SolidOpt.Services.Transformations.Optimizations;

namespace SolidOpt.Services.Transformations.Optimizations.NopRemover
{
	/// <summary>
	/// Description of NopRemoveTransformer.
	/// </summary>
	public class NopRemoveTransformer : IOptimize<MethodDefinition>
	{
		public NopRemoveTransformer()
		{
		}
		
		public MethodDefinition Optimize(MethodDefinition source)
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
			
			ILProcessor cil = source.Body.GetILProcessor();
			
			// Fix branch targets
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
			
			// Fix exception handlers
			foreach (ExceptionHandler handler in source.Body.ExceptionHandlers) {
				while (handler.TryStart.OpCode == OpCodes.Nop) handler.TryStart = handler.TryStart.Next;
				while (handler.FilterStart.OpCode == OpCodes.Nop) handler.FilterStart = handler.FilterStart.Next;
				while (handler.HandlerStart.OpCode == OpCodes.Nop) handler.HandlerStart = handler.HandlerStart.Next;
				while (handler.TryEnd.OpCode == OpCodes.Nop) handler.TryEnd = handler.TryEnd.Previous;
				while (handler.FilterEnd.OpCode == OpCodes.Nop) handler.FilterEnd = handler.FilterEnd.Previous;
				while (handler.HandlerEnd.OpCode == OpCodes.Nop) handler.HandlerEnd = handler.HandlerEnd.Previous;
			}
			
			// Remove Nop instructions
			Instruction instruction1 = source.Body.Instructions[source.Body.Instructions.Count-1];
//			Instruction instruction1 = source.Body.Instructions[1];
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
			
			source.Body.OptimizeMacros();
			
			return source;
		}
	}	
}
