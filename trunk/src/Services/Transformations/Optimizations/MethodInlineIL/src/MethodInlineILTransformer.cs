using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

using SolidOpt.Services.Transformations.Optimizations;

namespace SolidOpt.Services.Transformations.Optimizations.MethodInlineIL
{
	/// <summary>
	/// Description of MethodInlineILTransformer.
	/// </summary>
	public class MethodInlineILTransformer : IOptimize<MethodDefinition>
	{
		public MethodInlineILTransformer()
		{
		}
		
		public MethodDefinition Optimize(MethodDefinition source)
		{
			ILProcessor cil = source.Body.GetILProcessor();
			
			Instruction instruction = source.Body.Instructions[0];
			while (instruction != null) {
				if (instruction.OpCode.FlowControl == FlowControl.Call) {
					MethodDefinition inlineMethod = ((MethodReference)instruction.Operand).Resolve();
					if (IsInlineable(inlineMethod)) {
						Console.WriteLine(source.FullName + " call inline " + inlineMethod.FullName);
						
						// Clone inlinee method
						//TODO: crate clone method
						inlineMethod.Body = null;
						
						//TODO: Think about inlineMethod.Body.SimplifyMacros();
						
						// Create temporary local variables for arguments
						List<VariableDefinition> tempVarArgList = new List<VariableDefinition>();
						foreach (ParameterDefinition param in inlineMethod.Parameters) {
							VariableDefinition v = new VariableDefinition(param.ParameterType);
							tempVarArgList.Add(v);
							source.Body.Variables.Add(v);
						}
						// Store arguments to temporary local variables
						for (int i = tempVarArgList.Count-1; i >= 0; i--) {
							cil.InsertBefore(instruction, Instruction.Create(OpCodes.Stloc, tempVarArgList[i]));
						}
						
						// Copy inlinee local variables to method locals
						List<VariableDefinition> tempVarLocalList = new List<VariableDefinition>();
						foreach (VariableDefinition vv in inlineMethod.Body.Variables) {
							VariableDefinition v = new VariableDefinition(vv.VariableType);
							tempVarLocalList.Add(v);
							source.Body.Variables.Add(v);
						}
						
						// Clone body and fixup instructions
						foreach (Instruction instruction1 in inlineMethod.Body.Instructions) {
							if (instruction1.OpCode == OpCodes.Ldarg || instruction1.OpCode == OpCodes.Ldarg_S) {
								instruction1.OpCode = OpCodes.Ldloc;
								instruction1.Operand = tempVarArgList[((ParameterDefinition)instruction1.Operand).Index];
							} else if (instruction1.OpCode == OpCodes.Ldarg_0) {
								instruction1.OpCode = OpCodes.Ldloc;
								instruction1.Operand = tempVarArgList[0];
							} else if (instruction1.OpCode == OpCodes.Ldarg_1) {
								instruction1.OpCode = OpCodes.Ldloc;
								instruction1.Operand = tempVarArgList[1];
							} else if (instruction1.OpCode == OpCodes.Ldarg_2) {
								instruction1.OpCode = OpCodes.Ldloc;
								instruction1.Operand = tempVarArgList[2];
							} else if (instruction1.OpCode == OpCodes.Ldarg_3) {
								instruction1.OpCode = OpCodes.Ldloc;
								instruction1.Operand = tempVarArgList[3];
							} else if (instruction1.OpCode == OpCodes.Ldarga || instruction1.OpCode == OpCodes.Ldarga_S) {
								instruction1.OpCode = OpCodes.Ldloca;
								instruction1.Operand = tempVarArgList[((ParameterDefinition)instruction1.Operand).Index];
							} else
							
							if (instruction1.OpCode == OpCodes.Starg || instruction1.OpCode == OpCodes.Starg_S) {
								instruction1.OpCode = OpCodes.Stloc;
								instruction1.Operand = tempVarArgList[((ParameterDefinition)instruction1.Operand).Index];
							} else
							
							if (instruction1.OpCode == OpCodes.Ldloc || instruction1.OpCode == OpCodes.Ldloc_S) {
								instruction1.OpCode = OpCodes.Ldloc;
								instruction1.Operand = tempVarLocalList[((VariableDefinition)instruction1.Operand).Index];
							} else if (instruction1.OpCode == OpCodes.Ldloc_0) {
								instruction1.OpCode = OpCodes.Ldloc;
								instruction1.Operand = tempVarLocalList[0];
							} else if (instruction1.OpCode == OpCodes.Ldloc_1) {
								instruction1.OpCode = OpCodes.Ldloc;
								instruction1.Operand = tempVarLocalList[1];
							} else if (instruction1.OpCode == OpCodes.Ldloc_2) {
								instruction1.OpCode = OpCodes.Ldloc;
								instruction1.Operand = tempVarLocalList[2];
							} else if (instruction1.OpCode == OpCodes.Ldloc_3) {
								instruction1.OpCode = OpCodes.Ldloc;
								instruction1.Operand = tempVarLocalList[3];
							} else if (instruction1.OpCode == OpCodes.Ldloca || instruction1.OpCode == OpCodes.Ldloca_S) {
								instruction1.OpCode = OpCodes.Ldloca;
								instruction1.Operand = tempVarLocalList[((VariableDefinition)instruction1.Operand).Index];
							} else
							
							if (instruction1.OpCode == OpCodes.Stloc || instruction1.OpCode == OpCodes.Stloc_S) {
								instruction1.OpCode = OpCodes.Stloc;
								instruction1.Operand = tempVarLocalList[((VariableDefinition)instruction1.Operand).Index];
							} else if (instruction1.OpCode == OpCodes.Stloc_0) {
								instruction1.OpCode = OpCodes.Stloc;
								instruction1.Operand = tempVarLocalList[0];
							} else if (instruction1.OpCode == OpCodes.Stloc_1) {
								instruction1.OpCode = OpCodes.Stloc;
								instruction1.Operand = tempVarLocalList[1];
							} else if (instruction1.OpCode == OpCodes.Stloc_2) {
								instruction1.OpCode = OpCodes.Stloc;
								instruction1.Operand = tempVarLocalList[2];
							} else if (instruction1.OpCode == OpCodes.Stloc_3) {
								instruction1.OpCode = OpCodes.Stloc;
								instruction1.Operand = tempVarLocalList[3];
							} else
							
							if (instruction1.OpCode == OpCodes.Ret) {
								if (instruction1.Next != null) {
									instruction1.OpCode = OpCodes.Br;
									instruction1.Operand = instruction;
								} else {
									continue;
								}
							}
							
							cil.InsertBefore(instruction, instruction1);
						}
						
						// Clone exception handler
						foreach (ExceptionHandler handler in inlineMethod.Body.ExceptionHandlers) {
							source.Body.ExceptionHandlers.Add(handler);
						}
						
						instruction.OpCode = OpCodes.Nop;
						instruction.Operand = null;
						
						inlineMethod.Body = null;
					}
				}

				instruction = instruction.Next;
			}
			
			source.Body.OptimizeMacros();
			
			return source;
		}
		
		/// <summary>
		/// Проверява дали методът съдържа подходящ атрибут, посочващ дали може да бъде inline-ван
		/// </summary>
		/// <param name="method">Методът, който е кандидат за inline</param>
		/// <returns></returns>
		private bool IsInlineable(MethodReference method)
		{
			foreach (CustomAttribute ca in method.Resolve().CustomAttributes) {
				if (ca.Constructor.DeclaringType.FullName == (typeof(InlineableAttribute).FullName)) {
					return true;
				}
			}
			return false;
		}
	}	
	
	/// <summary>
	/// Атрибут, използван за обозначаване на това, че методът може да бъде inline-нат.
	/// TODO: Класът трябва да бъде преместен в специална отделна библиотека за атрибути
	/// </summary>
	public class InlineableAttribute : Attribute
	{
		
	}
}
