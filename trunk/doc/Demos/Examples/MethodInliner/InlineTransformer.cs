/*
 * 
 *
 * User: Vassil Vassilev
 * Date: 04.8.2009 г.
 * Time: 11:06
 * 
 */
using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Cecil.Decompiler.Ast;

using AstMethodDefinitionModel;
using ILtoAST;

using SolidOpt.Optimizer.Transformers;

namespace MethodInliner
{
	/// <summary>
	/// Description of InlineTransformer.
	/// </summary>
	public class InlineTransformer : BaseCodeTransformer, ITransform<AstMethodDefinition>
	{
		private List<BlockStatement> blocks = new List<BlockStatement>();
		private List<ExpressionStatement> expressions = new List<ExpressionStatement>();
		private BlockStatement currentBlock;
		private ExpressionStatement currentExpression;
		private Dictionary<VariableDefinition, VariableDefinition> localVarSubstitution = 
			new Dictionary<VariableDefinition, VariableDefinition>();
		private AstFixer fixer = new AstFixer();
		
		public InlineTransformer()
		{
		}
		
		public override ICodeNode VisitBlockStatement(BlockStatement node)
		{
			currentBlock = node;
			return base.VisitBlockStatement(node);
		}
		
		public override ICodeNode VisitExpressionStatement(ExpressionStatement node)
		{
			currentExpression = node;
			return base.VisitExpressionStatement(node);
		}
		
		public override ICodeNode VisitMethodInvocationExpression(MethodInvocationExpression node)
		{
			MethodReferenceExpression methodRef = (MethodReferenceExpression) node.Method;
			if (IsInlineable(methodRef.Method)) {
				blocks.Add(currentBlock);
				expressions.Add(currentExpression);
			}
			return base.VisitMethodInvocationExpression(node);
		}
		
		private bool IsInlineable(MethodReference method)
		{
			foreach (CustomAttribute ca in method.Resolve().CustomAttributes) {
				if (ca.Constructor.DeclaringType.FullName == (typeof(InlineAttribute).FullName)) {
					return true;
				}
			}
			return false;
		}
		
		public AstMethodDefinition Transform(AstMethodDefinition source)
		{
			if (source.Method.Name == "OutParamAssign")
				return source;
			
			expressions.Clear();
			blocks.Clear();
			localVarSubstitution.Clear();
			
			source.Block = (BlockStatement) Visit(source.Block);
			FixUpMethodCalls(source);
			source = fixer.FixUp(source, localVarSubstitution);
			
			return source;
		}
		
		private void FixUpMethodCalls(AstMethodDefinition source)
		{
			ILtoASTTransformer il2astTransformer = new ILtoASTTransformer();
			AstMethodDefinition ast;
			int expressionIndex;
			MethodInvocationExpression mInvoke;
			MethodReferenceExpression mRef;
			MethodDefinition mDef;
				
			for (int i = 0; i < blocks.Count; i++) {
				mInvoke = expressions[i].Expression as MethodInvocationExpression;
				mRef = mInvoke.Method as MethodReferenceExpression;
				mDef = mRef.Method.Resolve();
				
				ast = il2astTransformer.Transform(mDef);
				expressionIndex = blocks[i].Statements.IndexOf(expressions[i]);
				
				blocks[i].Statements.RemoveAt(expressionIndex);
				for (int j = ast.Block.Statements.Count - 1; j >= 0; j-- ) {
					blocks[i].Statements.Insert(expressionIndex, ast.Block.Statements[j]);
				}
				
				if (mDef.Body.Variables.Count != 0 && (!localVarSubstitution.ContainsKey(mDef.Body.Variables[0]))) {
					int vLastIndex = source.Method.Body.Variables.Count;
					VariableDefinition newVariable;
					foreach (VariableDefinition variable in mDef.Body.Variables) {
						newVariable = new VariableDefinition(variable.VariableType);
						newVariable.Method = source.Method;
						newVariable.Index = vLastIndex++;
						newVariable.Name = newVariable.ToString();
						localVarSubstitution[variable] = newVariable;
						
						source.Method.Body.Variables.Add(variable);
					}
				}
					
			}
		
		}
		
		internal class AstFixer : BaseCodeTransformer
		{
			private AstMethodDefinition source;
			private Dictionary<VariableDefinition, VariableDefinition> localVarSubstitution;
			private HashSet<VariableDefinition> isVariableDefined = new HashSet<VariableDefinition>();
			private AssignExpression lastAssignment;
			
			public AstFixer()
			{
			}
			
			public override ICodeNode VisitExpressionStatement(ExpressionStatement node)
			{
				var result = base.VisitExpressionStatement(node);
				if (node.Expression == null) {
					return null;
				}
				return result;
			}
			
			
			public AstMethodDefinition FixUp(AstMethodDefinition source, Dictionary<VariableDefinition, VariableDefinition> localVarSubstitution)
			{
				isVariableDefined.Clear();
				
				this.source = source;
				this.localVarSubstitution = localVarSubstitution;
				if (localVarSubstitution.Count > 0) {
					this.source.Block = (BlockStatement) Visit(this.source.Block);
				}
				return this.source;
			}
			
			public override ICodeNode VisitAssignExpression(AssignExpression node)
			{
				lastAssignment = node;
				return base.VisitAssignExpression(node);
			}
			
			
			public override ICodeNode VisitVariableReferenceExpression(VariableReferenceExpression node)
			{
				VariableDefinition varDef;
				if (localVarSubstitution.TryGetValue(node.Variable.Resolve(), out varDef)) {
					node.Variable = varDef;
				}
				return base.VisitVariableReferenceExpression(node);
			}
			
			public override ICodeNode VisitVariableDeclarationExpression(VariableDeclarationExpression node)
			{
				VariableDefinition varDef;
				if (localVarSubstitution.TryGetValue(node.Variable, out varDef)) {
					if (!isVariableDefined.Add(node.Variable)) {
						var varRef = lastAssignment.Target as VariableDeclarationExpression;
						if (varRef != null) {
							if (varRef.Variable == varDef) {
								throw NotImplementedException();
							}
							else {
								return new VariableReferenceExpression(varDef);
							}
						}
						else {
							return null;
						}
					}
					else {
						node.Variable = varDef;
					}
				}
				return base.VisitVariableDeclarationExpression(node);
			}
			
			
		}
		
		
	}
	public class InlineAttribute : Attribute
	{
		
	}
}
