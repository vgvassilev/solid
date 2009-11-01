/*
 *
 * User: Vassil Vassilev
 * Date: 18.9.2009 г.
 * Time: 12:17
 * 
 */
using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Cecil.Decompiler.Ast;

using AstMethodDefinitionModel;
using ILtoAST;

using SolidOpt.Services.Transformations;

namespace ConstantFolding
{
	/// <summary>
	/// Description of ConstantFoldingTransformer.
	/// </summary>
	public class ConstantFoldingTransformer : BaseCodeTransformer, ITransform<AstMethodDefinition>
	{
		public ConstantFoldingTransformer()
		{
		}
		
		public AstMethodDefinition Transform(AstMethodDefinition source)
		{
			source.Block = (BlockStatement) Visit(source.Block);
			return source;
		}
		
		public override ICodeNode VisitBinaryExpression(BinaryExpression node)
		{
			base.VisitBinaryExpression(node);
			
			var left = node.Left as LiteralExpression;
			var right = node.Right as LiteralExpression;
			CastExpression leftCast = null;
			CastExpression rightCast = null;
			if (left == null && node.Left is CastExpression) {
				leftCast = node.Left as CastExpression;
				left = leftCast.Expression as LiteralExpression;
			}
			if (right == null && node.Right is CastExpression) {
				rightCast = node.Right as CastExpression;
				right = rightCast.Expression as LiteralExpression;
			}
			
			if (left != null && right != null) {
				if (leftCast != null) {
					left.Value = Cast(left.Value, leftCast.TargetType);
				}
				if (rightCast != null) {
					right.Value = Cast(right.Value, rightCast.TargetType);
				}
				switch (node.Operator) {
					#region Arithmetic
					case BinaryOperator.Add :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value + (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value + (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value + (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value + (UInt64)right.Value);
						if (left.Value is Single) return new LiteralExpression ((Single)left.Value + (Single)right.Value);
						if (left.Value is Double) return new LiteralExpression ((Double)left.Value + (Double)right.Value);
						if (left.Value is String) return new LiteralExpression ((String)left.Value + (String)right.Value);
						throw new NotImplementedException();
					case BinaryOperator.Subtract :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value - (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value - (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value - (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value - (UInt64)right.Value);
						if (left.Value is Single) return new LiteralExpression ((Single)left.Value - (Single)right.Value);
						if (left.Value is Double) return new LiteralExpression ((Double)left.Value - (Double)right.Value);
						throw new NotImplementedException();
					case BinaryOperator.Divide :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value / (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value / (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value / (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value / (UInt64)right.Value);
						if (left.Value is Single) return new LiteralExpression ((Single)left.Value / (Single)right.Value);
						if (left.Value is Double) return new LiteralExpression ((Double)left.Value / (Double)right.Value);
						throw new NotImplementedException();
					case BinaryOperator.Modulo :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value % (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value % (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value % (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value % (UInt64)right.Value);
						throw new NotImplementedException();
					case BinaryOperator.Multiply :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value * (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value * (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value * (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value * (UInt64)right.Value);
						if (left.Value is Single) return new LiteralExpression ((Single)left.Value * (Single)right.Value);
						if (left.Value is Double) return new LiteralExpression ((Double)left.Value * (Double)right.Value);
						throw new NotImplementedException();
					
					#endregion
					
					#region Bitwise
					
					case BinaryOperator.BitwiseAnd :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value & (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value & (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value & (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value & (UInt64)right.Value);
						throw new NotImplementedException();
					case BinaryOperator.BitwiseOr :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value | (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value | (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value | (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value | (UInt64)right.Value);
						throw new NotImplementedException();
					case BinaryOperator.BitwiseXor :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value ^ (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value ^ (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value ^ (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value ^ (UInt64)right.Value);
						throw new NotImplementedException();
					case BinaryOperator.LeftShift :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value << (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value << (Int32)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value << (Int32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value << (Int32)right.Value);
						throw new NotImplementedException();
					case BinaryOperator.RightShift :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value >> (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value >> (Int32)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value >> (Int32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value >> (Int32)right.Value);
						throw new NotImplementedException();
					
					
					#endregion
					
					#region Comparations
					
					case BinaryOperator.GreaterThan :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value > (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value > (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value > (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value > (UInt64)right.Value);
						if (left.Value is Single) return new LiteralExpression ((Single)left.Value > (Single)right.Value);
						if (left.Value is Double) return new LiteralExpression ((Double)left.Value > (Double)right.Value);
						return node;
					case BinaryOperator.GreaterThanOrEqual :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value >= (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value >= (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value >= (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value >= (UInt64)right.Value);
						if (left.Value is Single) return new LiteralExpression ((Single)left.Value >= (Single)right.Value);
						if (left.Value is Double) return new LiteralExpression ((Double)left.Value >= (Double)right.Value);
						return node;
					case BinaryOperator.LessThan :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value < (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value < (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value < (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value < (UInt64)right.Value);
						if (left.Value is Single) return new LiteralExpression ((Single)left.Value < (Single)right.Value);
						if (left.Value is Double) return new LiteralExpression ((Double)left.Value < (Double)right.Value);
						return node;
					case BinaryOperator.LessThanOrEqual :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value <= (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value <= (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value <= (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value <= (UInt64)right.Value);
						if (left.Value is Single) return new LiteralExpression ((Single)left.Value <= (Single)right.Value);
						if (left.Value is Double) return new LiteralExpression ((Double)left.Value <= (Double)right.Value);
						return node;
					case BinaryOperator.ValueEquality :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value == (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value == (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value == (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value == (UInt64)right.Value);
						if (left.Value is Single) return new LiteralExpression ((Single)left.Value == (Single)right.Value);
						if (left.Value is Double) return new LiteralExpression ((Double)left.Value == (Double)right.Value);
						return node;
					case BinaryOperator.ValueInequality :
						if (left.Value is Int32) return new LiteralExpression ((Int32)left.Value != (Int32)right.Value);
						if (left.Value is Int64) return new LiteralExpression ((Int64)left.Value != (Int64)right.Value);
						if (left.Value is UInt32) return new LiteralExpression ((UInt32)left.Value != (UInt32)right.Value);
						if (left.Value is UInt64) return new LiteralExpression ((UInt64)left.Value != (UInt64)right.Value);
						if (left.Value is Single) return new LiteralExpression ((Single)left.Value != (Single)right.Value);
						if (left.Value is Double) return new LiteralExpression ((Double)left.Value != (Double)right.Value);
						return node;
					
					#endregion
					
					#region Logical
					
					case BinaryOperator.LogicalAnd :
						if (left.Value is Boolean) return new LiteralExpression ((Boolean)left.Value && (Boolean)right.Value);
						throw new NotImplementedException();
					case BinaryOperator.LogicalOr :
						if (left.Value is Boolean) return new LiteralExpression ((Boolean)left.Value || (Boolean)right.Value);
						throw new NotImplementedException();
					
					#endregion
				}
			}
			
			return node;
			
		}
		
		public override ICodeNode VisitUnaryExpression(UnaryExpression node)
		{
			base.VisitUnaryExpression(node);
			
			var operand = node.Operand as LiteralExpression;
			CastExpression operandCast = null;
			if (operand == null && node.Operand is CastExpression) {
				operandCast = node.Operand as CastExpression;
				operand = operandCast.Expression as LiteralExpression;
			}
			
			if (operand != null) {
				if (operandCast != null) {
					operand.Value = Cast(operand.Value, operandCast.TargetType);
				}
				switch (node.Operator) {
					#region Arithmetic
					case UnaryOperator.Negate :
						if (operand.Value is Int32) return new LiteralExpression (-(Int32)operand.Value);
						if (operand.Value is Int64) return new LiteralExpression (-(Int64)operand.Value);
						if (operand.Value is UInt32) return new LiteralExpression (-(UInt32)operand.Value);
//						if (operand.Value is UInt64) return new LiteralExpression (-(UInt64)operand.Value);
						if (operand.Value is Single) return new LiteralExpression (-(Single)operand.Value);
						if (operand.Value is Double) return new LiteralExpression (-(Double)operand.Value);
						throw new NotImplementedException();
						
//					case UnaryOperator.PostDecrement :
//					case UnaryOperator.PostIncrement :
//					case UnaryOperator.PreDecrement :
//					case UnaryOperator.PreIncrement :
//					case UnaryOperator.PreIncrement :
						
					#endregion
					
					#region Bitwise
					case UnaryOperator.BitwiseNot :
						if (operand.Value is Int32) return new LiteralExpression (~(Int32)operand.Value);
						if (operand.Value is Int64) return new LiteralExpression (~(Int64)operand.Value);
						if (operand.Value is UInt32) return new LiteralExpression (~(UInt32)operand.Value);
						if (operand.Value is UInt64) return new LiteralExpression (~(UInt64)operand.Value);
						throw new NotImplementedException();
					#endregion
					
					#region Logical
					case UnaryOperator.LogicalNot :
						if (operand.Value is Boolean) return new LiteralExpression (!(Boolean)operand.Value);
						throw new NotImplementedException();
					#endregion
				}
			}
			
			return node;
			
		}
		
		private object Cast(object value, TypeReference targetType)
		{
			switch (targetType.Name) {
				case "SByte" :
					return (SByte) value;
				case "Int16" :
					return (Int16) value;
				case "Int32" :
					return (Int32) value;
				case "Int64" :
					return (Int64) value;
				case "Byte" :
					return (Byte) value;
				case "UInt16" :
					return (UInt16) value;
				case "UInt32" :
					return (UInt32) value;
				case "UInt64" :
					return (UInt64) value;
				case "Single" :
					return (Single) value;
				case "Double" :
					return (Double) value;
			}
			return value;
		}
	}
}
