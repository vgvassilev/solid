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
		private CilWorker cil;
		private MethodBody body;
		
		private Dictionary<string, int> labels = new Dictionary<string, int>();
		private List<KeyValuePair<Instruction, string>> fixupLabels = new List<KeyValuePair<Instruction, string>>();
		private List<KeyValuePair<Instruction, int>> fixupBranches = new List<KeyValuePair<Instruction, int>>();
		
		private Instruction branchToFix;
		
		public Compiler(BlockStatement ast, CilWorker cil)
		{
			this.ast = ast;
			this.cil = cil;
			this.body = cil.GetBody();
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
				cil.Emit(OpCodes.Ret);
			
			foreach (KeyValuePair<Instruction, string> pair in fixupLabels) {
				pair.Key.Operand = labels[pair.Value];
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
			cil.Emit(OpCodes.Ret);
		}

		public override void VisitGotoStatement (GotoStatement node)
		{
			int instructionIndex;
			if (labels.TryGetValue(node.Label, out instructionIndex)) {
				cil.Emit(OpCodes.Br, body.Instructions[instructionIndex]);
			}
			else {
				fixupLabels.Add(new KeyValuePair<Instruction, string>(cil.Emit(OpCodes.Br, branchToFix), node.Label));
			}
		}

		public override void VisitLabeledStatement (LabeledStatement node)
		{
			labels.Add(node.Label, body.Instructions.Count);
		}

		public override void VisitIfStatement (IfStatement node)
		{
			Visit (node.Condition);
			

			Instruction conditionalBranch = cil.Emit(OpCodes.Brfalse, branchToFix);
			
			Visit (node.Then);
			
			if (node.Else != null) {
				Instruction branch = cil.Emit(OpCodes.Br, branchToFix);
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

////		public override void VisitExpressionStatement (ExpressionStatement node)
////		{
////			Visit (node.Expression);
////		}
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
			
			Instruction conditionalBranch = cil.Emit(OpCodes.Brfalse, branchToFix);
			
			Visit (node.Body);
			
			Instruction branch = cil.Emit(OpCodes.Br, body.Instructions[index]);
			fixupBranches.Add(new KeyValuePair<Instruction, int>(conditionalBranch, body.Instructions.Count));				
		}

		public override void VisitDoWhileStatement (DoWhileStatement node)
		{
			int index = body.Instructions.Count;
			
			Visit (node.Body);
			
			Visit (node.Condition);
			
			cil.Emit(OpCodes.Brtrue, body.Instructions[index]);
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
					cil.Emit(OpCodes.Callvirt, methRefExp.Method);
				}
				else {
					cil.Emit(OpCodes.Call, methRefExp.Method);
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
				cil.Emit(OpCodes.Ldnull);
			}
			else {
				switch (node.Value.GetType().FullName) {
					case "System.Int64" : cil.Emit(OpCodes.Ldc_I8, (Int64)node.Value); break;
					case "System.Int32" : cil.Emit(OpCodes.Ldc_I4, (Int32)node.Value); break;
					case "System.Int16" : cil.Emit(OpCodes.Ldc_I4, (Int16)node.Value); break;
					
					case "System.Single" : cil.Emit(OpCodes.Ldc_R4, (Single)node.Value); break;
					case "System.Double" : cil.Emit(OpCodes.Ldc_R8, (Double)node.Value); break;
					
					case "System.String" : cil.Emit(OpCodes.Ldstr, (String)node.Value); break;
					
					case "System.Boolean" : 
						if ((Boolean)node.Value) {
							cil.Emit(OpCodes.Ldc_I4_1);
						}
						else {
							cil.Emit(OpCodes.Ldc_I4_0);
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
					cil.Emit(OpCodes.Not);
					break;	
				
				case UnaryOperator.LogicalNot : 
					cil.Emit(OpCodes.Ldc_I4_0);
					cil.Emit(OpCodes.Ceq);
					break;
					
				case UnaryOperator.Negate : 
					cil.Emit(OpCodes.Neg);
					break;
					
				case UnaryOperator.PostDecrement : 
					//TODO: Expression--;
					break;
					
				case UnaryOperator.PreDecrement : 
					//TODO: --Expression;
					break;
					
				case UnaryOperator.PostIncrement : 
					//TODO: Expression++;
					break;
					
				case UnaryOperator.PreIncrement : 
					//TODO: ++Expression;
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
					cil.Emit(OpCodes.Add);
					break;	
				
				case BinaryOperator.BitwiseAnd : 
					Visit (node.Right);
					cil.Emit(OpCodes.And);
					break;
					
				case BinaryOperator.BitwiseOr : 
					Visit (node.Right);
					cil.Emit(OpCodes.Or);
					break;
					
				case BinaryOperator.BitwiseXor : 
					Visit (node.Right);
					cil.Emit(OpCodes.Xor);
					break;
					
				case BinaryOperator.Divide : 
					Visit (node.Right);
					cil.Emit(OpCodes.Div);
					break;
					
				case BinaryOperator.GreaterThan : 
					Visit (node.Right);
					cil.Emit(OpCodes.Cgt);
					break;
					
				case BinaryOperator.GreaterThanOrEqual : 
					Visit (node.Right);
					cil.Emit(OpCodes.Clt);
					cil.Emit(OpCodes.Ldc_I4_0);
					cil.Emit(OpCodes.Ceq);
					break;
				
				case BinaryOperator.LeftShift : 
					Visit (node.Right);
					cil.Emit(OpCodes.Shl);
					break;	
				
				case BinaryOperator.LessThan : 
					Visit (node.Right);
					cil.Emit(OpCodes.Clt);
					break;
					
				case BinaryOperator.LessThanOrEqual : 
					Visit (node.Right);
					cil.Emit(OpCodes.Cgt);
					cil.Emit(OpCodes.Ldc_I4_0);
					cil.Emit(OpCodes.Ceq);
					break;
				
				case BinaryOperator.LogicalAnd : 
					negBranch = cil.Emit(OpCodes.Brfalse, branchToFix);
						
					Visit (node.Right);
					posBranch = cil.Emit(OpCodes.Br, branchToFix);
					
					negBranch.Operand = cil.Emit(OpCodes.Ldc_I4_0);
					
					fixupBranches.Add(new KeyValuePair<Instruction, int>
					           					(posBranch, body.Instructions.Count));
					
					break;
				
				case BinaryOperator.LogicalOr : 
					posBranch = cil.Emit(OpCodes.Brtrue, branchToFix);
						
					Visit (node.Right);
					negBranch = cil.Emit(OpCodes.Br, branchToFix);
					
					posBranch.Operand = cil.Emit(OpCodes.Ldc_I4_1);
					
					fixupBranches.Add(new KeyValuePair<Instruction, int>
					           					(negBranch, body.Instructions.Count));
					
					break;
					
				case BinaryOperator.Modulo : 
					Visit (node.Right);
					cil.Emit(OpCodes.Rem);
					break;
				
				case BinaryOperator.Multiply : 
					Visit (node.Right);
					cil.Emit(OpCodes.Mul);
					break;
					
				case BinaryOperator.RightShift : 
					Visit (node.Right);
					cil.Emit(OpCodes.Shr);
					break;
					
				case BinaryOperator.Subtract : 
					Visit (node.Right);
					cil.Emit(OpCodes.Sub);
					break;
				
				case BinaryOperator.ValueEquality : 
					Visit (node.Right);
					cil.Emit(OpCodes.Ceq);
					break;
				
				case BinaryOperator.ValueInequality : 
					Visit (node.Right);
					cil.Emit(OpCodes.Ceq);
					cil.Emit(OpCodes.Ldc_I4_0);
					cil.Emit(OpCodes.Ceq);
					break;
			}
		}

		public override void VisitAssignExpression (AssignExpression node)
		{
			//Visit (node.Target); 
			
			switch (node.Target.CodeNodeType) {
				case CodeNodeType.VariableReferenceExpression :
					Visit (node.Expression);
				
					cil.Emit(OpCodes.Stloc, ((VariableReferenceExpression) node.Target).Variable.Index);
					
					break;
				
				case CodeNodeType.ArgumentReferenceExpression :
					Visit (node.Expression);
				
					cil.Emit(OpCodes.Starg, ((ArgumentReferenceExpression) node.Target).Parameter.Sequence);
					
					break;
				
				case CodeNodeType.FieldReferenceExpression :
					Visit (node.Expression);
				
					FieldReferenceExpression fldRefExp = (FieldReferenceExpression) node.Target;
					//TODO: fldRefExp.Target!
					FieldDefinition fld = fldRefExp.Field.Resolve();
					if ((fld.Attributes & FieldAttributes.Static) != 0) {
						cil.Emit(OpCodes.Stsfld, fld);
					}
					else {
						cil.Emit(OpCodes.Stfld, fld);
					}
					
					break;
			}
		}

		public override void VisitArgumentReferenceExpression (ArgumentReferenceExpression node)
		{
			cil.Emit(OpCodes.Ldarg, node.Parameter.Sequence);
		}

		public override void VisitVariableReferenceExpression (VariableReferenceExpression node)
		{
			cil.Emit(OpCodes.Ldloc, node.Variable.Index);
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
				cil.Emit(OpCodes.Ldsfld, fld);
			}
			else {
				cil.Emit(OpCodes.Ldfld, fld);
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
			
			Instruction conditionalBranch = cil.Emit(OpCodes.Brfalse, branchToFix);
			
			Visit (node.Then);
			
			if (node.Else != null) {
				Instruction branch = cil.Emit(OpCodes.Br, branchToFix);
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
}
