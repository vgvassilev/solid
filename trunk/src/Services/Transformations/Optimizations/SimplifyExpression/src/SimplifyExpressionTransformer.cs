/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Cecil.Decompiler.Ast;

using SolidOpt.Services.Transformations.Multimodel.AstMethodDefinitionModel;

using SolidOpt.Services.Transformations.Optimizations;


namespace SolidOpt.Services.Transformations.Optimizations.SimplifyExpression
{
	/// <summary>
	/// Description of SimplifyExpressionTransformer.
	/// </summary>
	public class SimplifyExpressionTransformer : BaseCodeTransformer, IOptimize<AstMethodDefinition>
	{
		#region Fields
		private Dictionary<Expression, int> expressionNumbers = new Dictionary<Expression, int>();
		private int expressionCnt = 0;
		#endregion
		
		#region Constructors
		
		public SimplifyExpressionTransformer()
		{
		}
		
		#endregion
		
		public AstMethodDefinition Optimize(AstMethodDefinition source)
		{
			expressionNumbers.Clear();
			expressionCnt = 0;
			
//			expressions.Clear();
//			blocks.Clear();
//			localVarSubstitution.Clear();
//			this.source = source;
			source.Block = (BlockStatement) Visit(source.Block);
//			source = fixer.FixUp(source, localVarSubstitution);
			
			Console.WriteLine("~~~");
			foreach (var d in expressionNumbers) {
				Console.Write(d.Value.ToString());
				Console.Write(", ");
				Console.Write(d.Key.ToString());
				Console.Write(", ");
				ConsoleColor savecolor = Console.ForegroundColor;
				WriteExpression(d.Key);
				Console.ForegroundColor = savecolor;
				Console.WriteLine();
			}
			
			return source;
		}

		private void WriteExpression(Cecil.Decompiler.Ast.Expression expr)
		{
			Cecil.Decompiler.Languages.ILanguage csharpLang = Cecil.Decompiler.Languages.CSharp.GetLanguage(
				Cecil.Decompiler.Languages.CSharpVersion.V1);
			
			var writer = csharpLang.GetWriter (new Cecil.Decompiler.Languages.ColoredConsoleFormatter());

			writer.Write(expr);
		}
		
		public override ICodeNode VisitBinaryExpression(BinaryExpression node)
		{
			ICodeNode result = base.VisitBinaryExpression(node);
			
			// a -> 1
			// b -> 2
			// a+b -> 3
			// b+a -> 3
			// a-b -> 4
			// b-a -> 5
			
			if (!expressionNumbers.ContainsKey(node)) {
				int value = -1;
				int valueLeft = expressionNumbers[node.Left];
				int valueRight = expressionNumbers[node.Right];
				
				switch (node.Operator) {
					case BinaryOperator.Add:
						foreach (var keypair in expressionNumbers) {
							if (keypair.Key.CodeNodeType == CodeNodeType.BinaryExpression) {
								BinaryExpression expr = (BinaryExpression)keypair.Key;
								if (expr.Operator == BinaryOperator.Add) {
									int valueLeft1 = expressionNumbers[expr.Left];
									int valueRight1 = expressionNumbers[expr.Right];
									if (valueLeft == valueLeft1 && valueRight == valueRight1 ||
									    valueLeft == valueRight1 && valueRight == valueLeft1) {
										value = keypair.Value;
										break;
									}
								}
							}
						}
						break;
					case BinaryOperator.Multiply:
						foreach (var node1 in expressionNumbers) {
							if (node1.Key.CodeNodeType == CodeNodeType.BinaryExpression) {
								BinaryExpression expr = (BinaryExpression)node1.Key;
								if (expr.Operator == BinaryOperator.Multiply) {
									int valueLeft1 = expressionNumbers[expr.Left];
									int valueRight1 = expressionNumbers[expr.Right];
									if (valueLeft == valueLeft1 && valueRight == valueRight1 ||
									    valueLeft == valueRight1 && valueRight == valueLeft1) {
										value = node1.Value;
										break;
									}
								}
							}
						}
						break;
				}
				
				if (value == -1) {
					value = expressionCnt++;
				}
				
				expressionNumbers.Add(node, value);
			}
			
			return result;
		}
		
		public override ICodeNode VisitUnaryExpression(UnaryExpression node)
		{
			ICodeNode result = base.VisitUnaryExpression(node);
			
			int value;
			if (expressionNumbers.TryGetValue(node, out value)) {
				//
			}
			else {
				expressionNumbers.Add(node, expressionCnt++);
			}
			
			return result;
		}
		
		public override ICodeNode VisitAssignExpression(AssignExpression node)
		{
			ICodeNode result = base.VisitAssignExpression(node);
			
			int value;
			if (expressionNumbers.TryGetValue(node, out value)) {
				//
			}
			else {
				expressionNumbers.Add(node, expressionCnt++);
			}
			
			return result;
		}
		
		public override ICodeNode VisitCastExpression(CastExpression node)
		{
			ICodeNode result = base.VisitCastExpression(node);
			//
			return result;
		}
		
		public override ICodeNode VisitConditionExpression(ConditionExpression node)
		{
			ICodeNode result = base.VisitConditionExpression(node);
			//
			return result;
		}
		
		public override ICodeNode VisitLiteralExpression(LiteralExpression node)
		{
			ICodeNode result = base.VisitLiteralExpression(node);
			
			// 1 -> 1
			// 2 -> 2
			// 5 -> 3
			// 2 -> 2
			// "abc" -> 4
			
			if (!expressionNumbers.ContainsKey(node)) {
				int value = -1;
				foreach (var keypair in expressionNumbers) {
					if (keypair.Key.CodeNodeType == CodeNodeType.LiteralExpression) {
						LiteralExpression expr = (LiteralExpression)keypair.Key;
						if (expr.Value.Equals(node.Value)) {
							value = keypair.Value;
							break;
						}
					}
				}

				if (value == -1) {
					value = expressionCnt++;
				}
				
				expressionNumbers.Add(node, value);
			}
			
			return result;
		}
		
		public override ICodeNode VisitVariableReferenceExpression(VariableReferenceExpression node)
		{
			ICodeNode result = base.VisitVariableReferenceExpression(node);
			
			// a -> 1
			// b -> 2
			// a -> 1
			// c -> 3
			
			if (!expressionNumbers.ContainsKey(node)) {
				int value = -1;
				foreach (var keypair in expressionNumbers) {
					if (keypair.Key.CodeNodeType == CodeNodeType.VariableReferenceExpression) {
						VariableReferenceExpression expr = (VariableReferenceExpression)keypair.Key;
						if (expr.Variable == node.Variable) {
							value = keypair.Value;
							break;
						}
					}
				}

				if (value == -1) {
					value = expressionCnt++;
				}
				
				expressionNumbers.Add(node, value);
			}
			
			return result;
		}
		
		public override ICodeNode VisitArgumentReferenceExpression(ArgumentReferenceExpression node)
		{
			ICodeNode result = base.VisitArgumentReferenceExpression(node);
			
			// a -> 1
			// b -> 2
			// a -> 1
			// c -> 3
			
			if (!expressionNumbers.ContainsKey(node)) {
				int value = -1;
				foreach (var keypair in expressionNumbers) {
					if (keypair.Key.CodeNodeType == CodeNodeType.ArgumentReferenceExpression) {
						ArgumentReferenceExpression expr = (ArgumentReferenceExpression)keypair.Key;
						if (expr.Parameter == node.Parameter) {
							value = keypair.Value;
							break;
						}
					}
				}

				if (value == -1) {
					value = expressionCnt++;
				}
				
				expressionNumbers.Add(node, value);
			}
			
			return result;
		}
		
		public override ICodeNode VisitFieldReferenceExpression(FieldReferenceExpression node)
		{
			ICodeNode result = base.VisitFieldReferenceExpression(node);
			
			// x.a -> 1
			// x.b -> 2
			// x.a -> 1
			// x.c -> 3
			// y.a -> 4
			
			if (!expressionNumbers.ContainsKey(node)) {
				int value = -1;
				if (node.Target == null) {
					foreach (var keypair in expressionNumbers) {
						if (keypair.Key.CodeNodeType == CodeNodeType.FieldReferenceExpression) {
							FieldReferenceExpression expr = (FieldReferenceExpression)keypair.Key;
							if (expr.Field == node.Field && expr.Target == null) {
								value = keypair.Value;
								break;
							}
						}
					}
				}
				else {
					int valueTarget = expressionNumbers[node.Target];
					foreach (var keypair in expressionNumbers) {
						if (keypair.Key.CodeNodeType == CodeNodeType.FieldReferenceExpression) {
							FieldReferenceExpression expr = (FieldReferenceExpression)keypair.Key;
							if (expr.Field == node.Field) {
								int valueTarget1 = expressionNumbers[expr.Target];
								if (valueTarget == valueTarget1) {
									value = keypair.Value;
									break;
								}
							}
						}
					}
				}

				if (value == -1) {
					value = expressionCnt++;
				}
				
				expressionNumbers.Add(node, value);
			}
			
			return result;
		}
		
		public override ICodeNode VisitObjectCreationExpression(ObjectCreationExpression node)
		{
			ICodeNode result = base.VisitObjectCreationExpression(node);
			
			int value;
			if (expressionNumbers.TryGetValue(node, out value)) {
				//
			}
			else {
				expressionNumbers.Add(node, expressionCnt++);
			}
			
			return result;
		}
		
		public override ICodeNode VisitMethodInvocationExpression(MethodInvocationExpression node)
		{
			ICodeNode result = base.VisitMethodInvocationExpression(node);
			
			int value;
			if (expressionNumbers.TryGetValue(node, out value)) {
				//
			}
			else {
				expressionNumbers.Add(node, expressionCnt++);
			}
			
			return result;
		}
		
		public override ICodeNode VisitMethodReferenceExpression(MethodReferenceExpression node)
		{
			ICodeNode result = base.VisitMethodReferenceExpression(node);
			
			int value;
			if (expressionNumbers.TryGetValue(node, out value)) {
				//
			}
			else {
				expressionNumbers.Add(node, expressionCnt++);
			}
			
			return result;
		}
		
	}
	
//	public class ExpressionEqualityComparer: EqualityComparer<Expression>
//	{
//		public ExpressionEqualityComparer()
//		{	
//		}
//		
//		public override bool Equals(Expression x, Expression y)
//		{
//			return true;
//		}
//		
//		public override int GetHashCode(Expression obj)
//		{
//			BinaryExpression binaryExpression = obj as BinaryExpression;
//			if (binaryExpression != null) return binaryExpression.;
//			
//			return base.GetHashCode(obj);
//		}
//		
//	}
}
