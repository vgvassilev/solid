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

namespace SolidOpt.Services.Transformations.Optimizations.IL.NopRemover
{
  /// <summary>
  /// Simple method, which removes the nop instructions. 
  /// </summary>
  public class NopRemoveTransformer : IOptimize<MethodDefinition>
  {
    public NopRemoveTransformer()
    {
    }
    
    public MethodDefinition Transform(MethodDefinition source)
    {
      return Optimize(source);
    }

    public MethodDefinition Optimize(MethodDefinition source)
    {
      ILProcessor cil = source.Body.GetILProcessor();
      
      //FIXME: Fix branch targets
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
      
      // FIXME: Fix exception handlers
      foreach (ExceptionHandler handler in source.Body.ExceptionHandlers) {
        while (handler.TryStart != null && handler.TryStart.OpCode == OpCodes.Nop)
          handler.TryStart = handler.TryStart.Next;
        
        while (handler.FilterStart != null && handler.FilterStart.OpCode == OpCodes.Nop) 
          handler.FilterStart = handler.FilterStart.Next;
        
        while (handler.HandlerStart != null && handler.HandlerStart.OpCode == OpCodes.Nop) 
          handler.HandlerStart = handler.HandlerStart.Next;
        
        while (handler.TryEnd != null && handler.TryEnd.OpCode == OpCodes.Nop)
          handler.TryEnd = handler.TryEnd.Previous;
        
        while (handler.FilterEnd != null && handler.FilterEnd.OpCode == OpCodes.Nop) 
          handler.FilterEnd = handler.FilterEnd.Previous;
        
        while (handler.HandlerEnd != null && handler.HandlerEnd.OpCode == OpCodes.Nop)
          handler.HandlerEnd = handler.HandlerEnd.Previous;
      }
      
      // Remove Nop instructions
      Instruction instruction1 = source.Body.Instructions[source.Body.Instructions.Count-1];
      while (instruction1 != null) {
        if (instruction1.OpCode == OpCodes.Nop) {
          cil.Remove(instruction1);
        }
        instruction1 = instruction1.Previous;
      }
      
      source.Body.OptimizeMacros();
      
      return source;
    }
  }  
}
