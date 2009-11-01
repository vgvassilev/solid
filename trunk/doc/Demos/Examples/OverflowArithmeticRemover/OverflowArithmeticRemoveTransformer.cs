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

using SolidOpt.Services.Transformations;

namespace OverflowArithmeticRemover
{
	/// <summary>
	/// Description of OverflowArithmeticRemoveTransformer.
	/// </summary>
	public class OverflowArithmeticRemoveTransformer : ITransform<MethodDefinition>
	{
		public OverflowArithmeticRemoveTransformer()
		{
		}
		
		public MethodDefinition Transform(MethodDefinition source)
		{
			source.Body.Simplify();
			foreach (Instruction instruction in source.Body.Instructions) {
				if (instruction.OpCode == OpCodes.Add_Ovf) {
					instruction.OpCode = OpCodes.Add;
					continue;
				}
				if (instruction.OpCode == OpCodes.Sub_Ovf) {
					instruction.OpCode = OpCodes.Sub;
					continue;
				}
				if (instruction.OpCode == OpCodes.Mul_Ovf) {
					instruction.OpCode = OpCodes.Mul;
					continue;
				}
				if (instruction.OpCode == OpCodes.Conv_Ovf_I) {
					instruction.OpCode = OpCodes.Conv_I;
					continue;
				}
				if (instruction.OpCode == OpCodes.Conv_Ovf_I1) {
					instruction.OpCode = OpCodes.Conv_I1;
					continue;
				}
				if (instruction.OpCode == OpCodes.Conv_Ovf_I2) {
					instruction.OpCode = OpCodes.Conv_I2;
					continue;
				}
				if (instruction.OpCode == OpCodes.Conv_Ovf_I4) {
					instruction.OpCode = OpCodes.Conv_I4;
					continue;
				}
				if (instruction.OpCode == OpCodes.Conv_Ovf_I8) {
					instruction.OpCode = OpCodes.Conv_I8;
					continue;
				}
				if (instruction.OpCode == OpCodes.Conv_Ovf_U) {
					instruction.OpCode = OpCodes.Conv_U;
					continue;
				}
				if (instruction.OpCode == OpCodes.Conv_Ovf_U1) {
					instruction.OpCode = OpCodes.Conv_U1;
					continue;
				}
				if (instruction.OpCode == OpCodes.Conv_Ovf_U2) {
					instruction.OpCode = OpCodes.Conv_U2;
					continue;
				}
				if (instruction.OpCode == OpCodes.Conv_Ovf_U4) {
					instruction.OpCode = OpCodes.Conv_U4;
					continue;
				}
				if (instruction.OpCode == OpCodes.Conv_Ovf_U8) {
					instruction.OpCode = OpCodes.Conv_U8;
					continue;
				}
/*
//					case OpCodes.Add_Ovf_Un:
//						instruction.OpCode = OpCodes.Add_;
//						break;
					case OpCodes.Conv_Ovf_I:
						instruction.OpCode = OpCodes.Conv_I;
						break;
//					case OpCodes.Conv_Ovf_I_Un:
//						instruction.OpCode = OpCodes.Conv_I_Un;
//						break;
					case OpCodes.Conv_Ovf_I1:
						instruction.OpCode = OpCodes.Conv_I1;
						break;
//					case OpCodes.Conv_Ovf_I1_Un:
//						instruction.OpCode = OpCodes.Conv_I1_Un;
//						break;
					case OpCodes.Conv_Ovf_I2:
						instruction.OpCode = OpCodes.Conv_I2;
						break;
//					case OpCodes.Conv_Ovf_I2_Un:
//						instruction.OpCode = OpCodes.Conv_I2_Un;
//						break;
					case OpCodes.Conv_Ovf_I4:
						instruction.OpCode = OpCodes.Conv_I4;
						break;
//					case OpCodes.Conv_Ovf_I4_Un:
//						instruction.OpCode = OpCodes.Conv_I4_Un;
//						break;
					case OpCodes.Conv_Ovf_I8:
						instruction.OpCode = OpCodes.Conv_I8;
						break;
//					case OpCodes.Conv_Ovf_I8_Un:
//						instruction.OpCode = OpCodes.Conv_I8_Un;
//						break;
					case OpCodes.Conv_Ovf_U:
						instruction.OpCode = OpCodes.Conv_U;
						break;
//					case OpCodes.Conv_Ovf_U_Un:
//						instruction.OpCode = OpCodes.Conv_U_Un;
//						break;
					case OpCodes.Conv_Ovf_U1:
						instruction.OpCode = OpCodes.Conv_U1;
						break;
//					case OpCodes.Conv_Ovf_U1_Un:
//						instruction.OpCode = OpCodes.Conv_U1_Un;
//						break;
					case OpCodes.Conv_Ovf_U2:
						instruction.OpCode = OpCodes.Conv_U2;
						break;
//					case OpCodes.Conv_Ovf_U2_Un:
//						instruction.OpCode = OpCodes.Conv_U2_Un;
//						break;
					case OpCodes.Conv_Ovf_U4:
						instruction.OpCode = OpCodes.Conv_U4;
						break;
//					case OpCodes.Conv_Ovf_U4_Un:
//						instruction.OpCode = OpCodes.Conv_U4_Un;
//						break;
					case OpCodes.Conv_Ovf_U8:
						instruction.OpCode = OpCodes.Conv_U8;
						break;
//					case OpCodes.Conv_Ovf_U8_Un:
//						instruction.OpCode = OpCodes.Conv_U8_Un;
//						break;
					case OpCodes.Mul_Ovf:
						instruction.OpCode = OpCodes.Mul;
						break;
//					case OpCodes.Mul_Ovf_Un:
//						instruction.OpCode = OpCodes.Mul_Un;
//						break;
					case OpCodes.Sub_Ovf:
						instruction.OpCode = OpCodes.Sub;
						break;
//					case OpCodes.Sub_Ovf_Un:
//						instruction.OpCode = OpCodes.Sub_Un;
//						break;
				}
			*/
			}
			source.Body.Optimize();
			
			return source;
		}
	}
}
