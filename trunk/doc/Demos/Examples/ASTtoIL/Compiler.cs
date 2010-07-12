/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 14.7.2009 г.
 * Time: 10:09
 * 
 */
using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Cecil.Decompiler.Ast;

using AstMethodDefinitionModel;

namespace ASTtoIL
{
	/// <summary>
	/// Description of Compiler.
	/// </summary>
	public class Compiler : Cecil.Decompiler.Ast.BaseCodeVisitor
	{
		private BlockStatement ast;
		//Mono.Cecil 0.9.3 migration: private CilWorker cil;
		private ILProcessor cil;
		private MethodBody body;
		
		private Dictionary<string, int> labels = new Dictionary<string, int>();
		private List<KeyValuePair<Instruction, string>> fixupLabels = new List<KeyValuePair<Instruction, string>>();
		private List<KeyValuePair<Instruction, int>> fixupBranches = new List<KeyValuePair<Instruction, int>>();
		
		private Instruction branchToFix;
		
		//Mono.Cecil 0.9.3 migration: public Compiler(BlockStatement ast, CilWorker cil)
		public Compiler(BlockStatement ast, ILProcessor cil, MethodBody body)
		{
			this.ast = ast;
			this.cil = cil;
			this.body = body;
			this.branchToFix = cil.Create(OpCodes.Nop);
		}
		
		public void Compile()
		{
			fixupBranches.Clear();
			fixupLabels.Clear();
			labels.Clear();
			
//			List<Instruction> instrList = new List<Instruction>();
//			foreach (Instruction instr in body.Instructions)
//				instrList.Add(instr);

			body.Instructions.Clear();
			
			Visit(ast);
			
			if (body.Instructions[body.Instructions.Count - 1].OpCode != OpCodes.Ret)
				cil.EmitInstruction(OpCodes.Ret);
			
			foreach (KeyValuePair<Instruction, string> pair in fixupLabels) {
				pair.Key.Operand = body.Instructions[labels[pair.Value]];
			}
			
			foreach (KeyValuePair<Instruction, int> pair in fixupBranches) {
				pair.Key.Operand = body.Instructions[pair.Value];
			}
			
//			foreach (Instruction instr in instrList)
//				cil.Append(instr);
		}
		
//		public override void VisitBlockStatement (BlockStatement node)
//		{
//			Visit (node.Statements);
//		}

		public override void VisitReturnStatement (ReturnStatement node)
		{
			Visit (node.Expression);
			cil.EmitInstruction(OpCodes.Ret);
		}

		public override void VisitGotoStatement (GotoStatement node)
		{
			int instructionIndex;
			if (labels.TryGetValue(node.Label, out instructionIndex)) {
				cil.EmitInstruction(OpCodes.Br, body.Instructions[instructionIndex]);
			}
			else {
				fixupLabels.Add(new KeyValuePair<Instruction, string>(cil.EmitInstruction(OpCodes.Br, branchToFix), node.Label));
			}
		}

		public override void VisitLabeledStatement (LabeledStatement node)
		{
			labels.Add(node.Label, body.Instructions.Count);
		}

		public override void VisitIfStatement (IfStatement node)
		{
			Visit (node.Condition);
			

			Instruction conditionalBranch = cil.EmitInstruction(OpCodes.Brfalse, branchToFix);
			
			Visit (node.Then);
			
			if (node.Else != null) {
				Instruction branch = cil.EmitInstruction(OpCodes.Br, branchToFix);
				int index = body.Instructions.Count;
				
				Visit (node.Else);
				//TODO: If else doesn't emit something then this cannot be fixed. It sould be fixed with global fixup
				conditionalBranch.Operand = body.Instructions[index];
				fixupBranches.Add(new KeyValuePair<Instruction, int>(branch, body.Instructions.Count));				
			}
			else {
				fixupBranches.Add(new KeyValuePair<Instruction, int>(conditionalBranch, body.Instructions.Count));				
			}
			 
		}

//		public override void VisitExpressionStatement (ExpressionStatement node)
//		{
//			Visit (node.Expression);
//		}
//
//		public override void VisitThrowStatement (ThrowStatement node)
//		{
//			Visit (node.Expression);
//		}
//
		public override void VisitWhileStatement (WhileStatement node)
		{
			int index = body.Instructions.Count;
			Visit (node.Condition);
			
			Instruction conditionalBranch = cil.EmitInstruction(OpCodes.Brfalse, branchToFix);
			
			Visit (node.Body);
			
			Instruction branch = cil.EmitInstruction(OpCodes.Br, body.Instructions[index]);
			fixupBranches.Add(new KeyValuePair<Instruction, int>(conditionalBranch, body.Instructions.Count));				
		}

		public override void VisitDoWhileStatement (DoWhileStatement node)
		{
			int index = body.Instructions.Count;
			
			Visit (node.Body);
			
			Visit (node.Condition);
			
			cil.EmitInstruction(OpCodes.Brtrue, body.Instructions[index]);
		}

//		public override void VisitBreakStatement (BreakStatement node)
//		{
//			
//		}
//
//		public override void VisitContinueStatement (ContinueStatement node)
//		{
//		}
//
//		public override void VisitForStatement (ForStatement node)
//		{
//			Visit (node.Initializer);
//			Visit (node.Condition);
//			Visit (node.Increment);
//			Visit (node.Body);
//		}
//
//		public override void VisitForEachStatement (ForEachStatement node)
//		{
//			Visit (node.Variable);
//			Visit (node.Expression);
//			Visit (node.Body);
//		}
//
//		public override void VisitConditionCase (ConditionCase node)
//		{
//			Visit (node.Condition);
//		}
//
//		public override void VisitDefaultCase (DefaultCase node)
//		{
//		}
//
//		public override void VisitSwitchStatement (SwitchStatement node)
//		{
//			Visit (node.Expression);
//			Visit (node.Cases);
//		}
//
//		public override void VisitCatchClause (CatchClause node)
//		{
//			Visit (node.Body);
//			Visit (node.Variable);
//		}
//
//		public override void VisitTryStatement (TryStatement node)
//		{
//			Visit (node.Try);
//			Visit (node.CatchClauses);
//			Visit (node.Fault);
//			Visit (node.Finally);
//		}
//
////		public override void VisitBlockExpression (BlockExpression node)
////		{
////			Visit (node.Expressions);
////		}
//
		public override void VisitMethodInvocationExpression (MethodInvocationExpression node)
		{
			Visit (node.Method);
			
			Visit (node.Arguments);
			
			MethodReferenceExpression methRefExp = node.Method as MethodReferenceExpression;
			if (methRefExp != null) {
				if (methRefExp.Method.Resolve().IsVirtual) {
					cil.EmitInstruction(OpCodes.Callvirt, methRefExp.Method);
				}
				else {
					cil.EmitInstruction(OpCodes.Call, methRefExp.Method);
				}
			}
		}

		public override void VisitMethodReferenceExpression (MethodReferenceExpression node)
		{
			Visit (node.Target);
		}

//		public override void VisitDelegateCreationExpression (DelegateCreationExpression node)
//		{
//			Visit (node.Target);
//		}
//
//		public override void VisitDelegateInvocationExpression (DelegateInvocationExpression node)
//		{
//			Visit (node.Target);
//			Visit (node.Arguments);
//		}
//
		public override void VisitLiteralExpression (LiteralExpression node)
		{
			if (node.Value == null) {
				cil.EmitInstruction(OpCodes.Ldnull);
			}
			else {
				switch (node.Value.GetType().FullName) {
					case "System.Int64" : cil.EmitInstruction(OpCodes.Ldc_I8, (Int64)node.Value); break;
					case "System.Int32" : cil.EmitInstruction(OpCodes.Ldc_I4, (Int32)node.Value); break;
					case "System.Int16" : cil.EmitInstruction(OpCodes.Ldc_I4, (Int16)node.Value); break;
					
					case "System.Single" : cil.EmitInstruction(OpCodes.Ldc_R4, (Single)node.Value); break;
					case "System.Double" : cil.EmitInstruction(OpCodes.Ldc_R8, (Double)node.Value); break;
					
					case "System.String" : cil.EmitInstruction(OpCodes.Ldstr, (String)node.Value); break;
					
					case "System.Boolean" : 
						if ((Boolean)node.Value) {
							cil.EmitInstruction(OpCodes.Ldc_I4_1);
						}
						else {
							cil.EmitInstruction(OpCodes.Ldc_I4_0);
						}
					break;
					
				}
			}
		}

		public override void VisitUnaryExpression (UnaryExpression node)
		{
			Visit (node.Operand);
			switch(node.Operator) {
				case UnaryOperator.BitwiseNot : 
					cil.EmitInstruction(OpCodes.Not);
					break;	
				
				case UnaryOperator.LogicalNot : 
					cil.EmitInstruction(OpCodes.Ldc_I4_0);
					cil.EmitInstruction(OpCodes.Ceq);
					break;
					
				case UnaryOperator.Negate : 
					cil.EmitInstruction(OpCodes.Neg);
					break;
					
				case UnaryOperator.PostDecrement : 
					//TODO: Expression--;
					cil.EmitInstruction(OpCodes.Ldc_I4_1);
					cil.EmitInstruction(OpCodes.Sub);
					break;
					
				case UnaryOperator.PreDecrement : 
					//TODO: --Expression;
					cil.EmitInstruction(OpCodes.Ldc_I4_1);
					cil.EmitInstruction(OpCodes.Sub);
					cil.EmitInstruction(OpCodes.Dup);
					break;
					
				case UnaryOperator.PostIncrement : 
					//TODO: Expression++;
					cil.EmitInstruction(OpCodes.Ldc_I4_1);
					cil.EmitInstruction(OpCodes.Add);
					break;
					
				case UnaryOperator.PreIncrement : 
					//TODO: ++Expression;
					cil.EmitInstruction(OpCodes.Ldc_I4_1);
					cil.EmitInstruction(OpCodes.Add);
					cil.EmitInstruction(OpCodes.Dup);
					break;
			}
		}

		public override void VisitBinaryExpression (BinaryExpression node)
		{
			Visit (node.Left);
			
			Instruction posBranch;
			Instruction negBranch;
			
			switch(node.Operator) {
				case BinaryOperator.Add : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Add);
					break;	
				
				case BinaryOperator.BitwiseAnd : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.And);
					break;
					
				case BinaryOperator.BitwiseOr : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Or);
					break;
					
				case BinaryOperator.BitwiseXor : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Xor);
					break;
					
				case BinaryOperator.Divide : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Div);
					break;
					
				case BinaryOperator.GreaterThan : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Cgt);
					break;
					
				case BinaryOperator.GreaterThanOrEqual : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Clt);
					cil.EmitInstruction(OpCodes.Ldc_I4_0);
					cil.EmitInstruction(OpCodes.Ceq);
					break;
				
				case BinaryOperator.LeftShift : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Shl);
					break;	
				
				case BinaryOperator.LessThan : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Clt);
					break;
					
				case BinaryOperator.LessThanOrEqual : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Cgt);
					cil.EmitInstruction(OpCodes.Ldc_I4_0);
					cil.EmitInstruction(OpCodes.Ceq);
					break;
				
				case BinaryOperator.LogicalAnd : 
					negBranch = cil.EmitInstruction(OpCodes.Brfalse, branchToFix);
						
					Visit (node.Right);
					posBranch = cil.EmitInstruction(OpCodes.Br, branchToFix);
					
					negBranch.Operand = cil.EmitInstruction(OpCodes.Ldc_I4_0);
					
					fixupBranches.Add(new KeyValuePair<Instruction, int>
					           					(posBranch, body.Instructions.Count));
					
					break;
				
				case BinaryOperator.LogicalOr : 
					posBranch = cil.EmitInstruction(OpCodes.Brtrue, branchToFix);
						
					Visit (node.Right);
					negBranch = cil.EmitInstruction(OpCodes.Br, branchToFix);
					
					posBranch.Operand = cil.EmitInstruction(OpCodes.Ldc_I4_1);
					
					fixupBranches.Add(new KeyValuePair<Instruction, int>
					           					(negBranch, body.Instructions.Count));
					
					break;
					
				case BinaryOperator.Modulo : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Rem);
					break;
				
				case BinaryOperator.Multiply : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Mul);
					break;
					
				case BinaryOperator.RightShift : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Shr);
					break;
					
				case BinaryOperator.Subtract : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Sub);
					break;
				
				case BinaryOperator.ValueEquality : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Ceq);
					break;
				
				case BinaryOperator.ValueInequality : 
					Visit (node.Right);
					cil.EmitInstruction(OpCodes.Ceq);
					cil.EmitInstruction(OpCodes.Ldc_I4_0);
					cil.EmitInstruction(OpCodes.Ceq);
					break;
			}
		}

		public override void VisitAssignExpression (AssignExpression node)
		{
			//Visit (node.Target); 
			
			switch (node.Target.CodeNodeType) {
				case CodeNodeType.VariableReferenceExpression :
					Visit (node.Expression);
				
					cil.EmitInstruction(OpCodes.Stloc, ((VariableReferenceExpression) node.Target).Variable.Index);
					
					break;
				
				case CodeNodeType.ArgumentReferenceExpression :
					Visit (node.Expression);
				
					cil.EmitInstruction(OpCodes.Starg, ((ArgumentReferenceExpression) node.Target).Parameter.Index+1);
					
					break;
				
				case CodeNodeType.FieldReferenceExpression :
					Visit (node.Expression);
				
					FieldReferenceExpression fldRefExp = (FieldReferenceExpression) node.Target;
					//TODO: fldRefExp.Target!
					FieldDefinition fld = fldRefExp.Field.Resolve();
					if ((fld.Attributes & FieldAttributes.Static) != 0) {
						cil.EmitInstruction(OpCodes.Stsfld, fld);
					}
					else {
						cil.EmitInstruction(OpCodes.Stfld, fld);
					}
					
					break;
			}
		}

		public override void VisitArgumentReferenceExpression (ArgumentReferenceExpression node)
		{
			cil.EmitInstruction(OpCodes.Ldarg, node.Parameter.Index+1);
		}

		public override void VisitVariableReferenceExpression (VariableReferenceExpression node)
		{
			cil.EmitInstruction(OpCodes.Ldloc, node.Variable.Index);
		}

//		public override void VisitVariableDeclarationExpression (VariableDeclarationExpression node)
//		{
//		}
//
//		public override void VisitThisReferenceExpression (ThisReferenceExpression node)
//		{
//		}
//
//		public override void VisitBaseReferenceExpression (BaseReferenceExpression node)
//		{
//		}
//
		public override void VisitFieldReferenceExpression (FieldReferenceExpression node)
		{
			//Visit (node.Target);
			//TODO: node.Target!
			FieldReference fldRef = (FieldReference) node.Field;
			FieldDefinition fld = fldRef.Resolve();
			if ((fld.Attributes & FieldAttributes.Static) != 0) {
				cil.EmitInstruction(OpCodes.Ldsfld, fld);
			}
			else {
				cil.EmitInstruction(OpCodes.Ldfld, fld);
			}
		}

//		public override void VisitCastExpression (CastExpression node)
//		{
//			Visit (node.Expression);
//		}
//
//		public override void VisitSafeCastExpression (SafeCastExpression node)
//		{
//			Visit (node.Expression);
//		}
//
//		public override void VisitCanCastExpression (CanCastExpression node)
//		{
//			Visit (node.Expression);
//		}
//
//		public override void VisitTypeOfExpression (TypeOfExpression node)
//		{
//		}
//
		public override void VisitConditionExpression (ConditionExpression node)
		{
			Visit (node.Condition);
			
			Instruction conditionalBranch = cil.EmitInstruction(OpCodes.Brfalse, branchToFix);
			
			Visit (node.Then);
			
			if (node.Else != null) {
				Instruction branch = cil.EmitInstruction(OpCodes.Br, branchToFix);
				int index = body.Instructions.Count;
				
				Visit (node.Else);
				//TODO: If else doesn't emit something then this cannot be fixed. It sould be fixed with global fixup
				conditionalBranch.Operand = body.Instructions[index];
				fixupBranches.Add(new KeyValuePair<Instruction, int>(branch, body.Instructions.Count));				
			}
			else {
				fixupBranches.Add(new KeyValuePair<Instruction, int>(conditionalBranch, body.Instructions.Count));				
			}
		}
//
//		public override void VisitNullCoalesceExpression (NullCoalesceExpression node)
//		{
//			Visit (node.Condition);
//			Visit (node.Expression);
//		}
//
//		public override void VisitAddressDereferenceExpression (AddressDereferenceExpression node)
//		{
//			Visit (node.Expression);
//		}
//
//		public override void VisitAddressReferenceExpression (AddressReferenceExpression node)
//		{
//			Visit (node.Expression);
//		}
//
//		public override void VisitAddressOfExpression (AddressOfExpression node)
//		{
//			Visit (node.Expression);
//		}
//
//		public override void VisitArrayCreationExpression (ArrayCreationExpression node)
//		{
//			Visit (node.Dimensions);
//			Visit (node.Initializer);
//		}
//
//		public override void VisitArrayIndexerExpression (ArrayIndexerExpression node)
//		{
//			Visit (node.Target);
//			Visit (node.Indices);
//		}
//
//		public override void VisitObjectCreationExpression (ObjectCreationExpression node)
//		{
//			Visit (node.Arguments);
//			Visit (node.Initializer);
//		}
//
//		public override void VisitPropertyReferenceExpression (PropertyReferenceExpression node)
//		{
//			Visit (node.Target);
//		}
//
//		public override void VisitTypeReferenceExpression (TypeReferenceExpression node)
//		{
//		}
	}

	//Mono.Cecil 0.9.3 migration
	public static class ILProcessorExtenstions 
	{
		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode)
		{
			Instruction result = cil.Create (opcode);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, TypeReference type)
		{
			Instruction result = cil.Create (opcode, type);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, MethodReference method)
		{
			Instruction result = cil.Create (opcode, method);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, CallSite site)
		{
			Instruction result = cil.Create (opcode, site);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, FieldReference field)
		{
			Instruction result = cil.Create (opcode, field);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, string value)
		{
			Instruction result = cil.Create (opcode, value);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, byte value)
		{
			Instruction result = cil.Create (opcode, value);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, sbyte value)
		{
			Instruction result = cil.Create (opcode, value);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, int value)
		{
			Instruction result = cil.Create (opcode, value);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, long value)
		{
			Instruction result = cil.Create (opcode, value);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, float value)
		{
			Instruction result = cil.Create (opcode, value);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, double value)
		{
			Instruction result = cil.Create (opcode, value);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, Instruction target)
		{
			Instruction result = cil.Create (opcode, target);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, Instruction [] targets)
		{
			Instruction result = cil.Create (opcode, targets);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, VariableDefinition variable)
		{
			Instruction result = cil.Create (opcode, variable);
			cil.Append(result);
			return result;
		}

		public static Instruction EmitInstruction(this ILProcessor cil, OpCode opcode, ParameterDefinition parameter)
		{
			Instruction result = cil.Create (opcode, parameter);
			cil.Append(result);
			return result;
		}

	}
}
