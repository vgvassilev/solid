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
	/// Замества извикване на метод с директното му разписване. За коректното заместване е необходимо
	/// методът който ще се разписва да бъде маркиран с атрибута Inlineable и да няма странични ефекти 
	/// върху системата. Т.е. да променя единствено съдържанието единствено в себе си. Например
	/// <code>
	/// [Inlineable]
	/// int Inlinee (a, b)
	/// {
	/// 	if (a &lt;= b)
	/// 		return a * b;
	/// 	else
	///		 	return a + b;
	/// }
	/// 
	/// void Inliner ()
	/// {
	/// 	...
	/// 	int sum = Inlinee(5, 8);
	/// }
	/// </code>
	/// След трансформацията Inliner придобива вида:
	/// <code>
	/// void Inliner ()
	/// {
	/// 	...
	/// 	int result;
	/// 	if (5 &lt;= 8)
	/// 		result = 5 * 8;
	/// 	else
	///		 	result = 5 + 8;
	/// 	int sum = result;
	/// }	
	/// </code>
	/// </summary>
	public class InlineTransformer : BaseCodeTransformer, ITransform<AstMethodDefinition>
	{
		#region Fields
		/// <summary>
		/// Поле, в което ще се съхраняват блокове, които съдържат извиквания на метод,
		/// които ще бъде inline-ван.
		/// </summary>
		private List<BlockStatement> blocks = new List<BlockStatement>();
		
		/// <summary>
		/// Поле, в което ще се съхраняват изрази, които съдържат извиквания на метод,
		/// които ще бъде inline-ван.
		/// </summary>
		private List<ExpressionStatement> expressions = new List<ExpressionStatement>();
		
		/// <summary>
		/// Поле, в което ще се съхранява текущият блок, съдържащ извикването на метод.
		/// </summary>
		private BlockStatement currentBlock;
		
		/// <summary>
		/// Поле, в което ще се съхранява текущия израз, съдържащ извикването на метод.
		/// </summary>
		private ExpressionStatement currentExpression;
		
		/// <summary>
		/// Структура, в която се съхраняват новите локални променливи, които ще заменят старите.
		/// Целта е да не съвпадне локална променлива с тази от inline-вания метод.
		/// </summary>
		private Dictionary<VariableDefinition, VariableDefinition> localVarSubstitution =
			new Dictionary<VariableDefinition, VariableDefinition>();
		
		/// <summary>
		/// Структура, в която се съхраняват новите локални променливи, които ще заменят параметрите.
		/// Целта е да се променят всички референции към параметър с локални променливи.
		/// </summary>
		private Dictionary<ParameterDefinition, VariableDefinition> paramVarSubstitution =
			new Dictionary<ParameterDefinition, VariableDefinition>();
		
		private AstFinalFixer fixer = new AstFinalFixer();
		#endregion
		
		#region Constructors
		
		public InlineTransformer()
		{
		}
		
		#endregion
		
		public AstMethodDefinition Transform(AstMethodDefinition source)
		{
//			if (source.Method.Name == "OutParamAssign")
//				return source;
			
			expressions.Clear();
			blocks.Clear();
			localVarSubstitution.Clear();
			
			source.Block = (BlockStatement) Visit(source.Block);
			FixUpMethodCalls(source);
			source = fixer.FixUp(source, localVarSubstitution);
			
			return source;
		}
		
		#region Model Transformers
		
		/// <summary>
		/// Записва текущия блок, който се обхожда.
		/// </summary>
		public override ICodeNode VisitBlockStatement(BlockStatement node)
		{
			currentBlock = node;
			return base.VisitBlockStatement(node);
		}
		
		/// <summary>
		/// Записва текущия израз, който се обхожда.
		/// </summary>
		public override ICodeNode VisitExpressionStatement(ExpressionStatement node)
		{
			currentExpression = node;
			return base.VisitExpressionStatement(node);
		}
		
		/// <summary>
		/// Ако извикването на метод е подходящо за inline-ване се записват текущия блок и израз.
		/// На втори пас се замества с реалния код на целевия метод.
		/// </summary>
		public override ICodeNode VisitMethodInvocationExpression(MethodInvocationExpression node)
		{
			MethodReferenceExpression methodRef = (MethodReferenceExpression) node.Method;
			if (IsInlineable(methodRef.Method)) {
				blocks.Add(currentBlock);
				expressions.Add(currentExpression);
			}
			return base.VisitMethodInvocationExpression(node);
		}
		
		#endregion
		
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
		
		/// <summary>
		/// Премахва извикването на метода и добавя едно по едно изреченията му. 
		/// Добавя и локалните променливи на inline-вания метод, като прилага необходимите субституции.
		/// </summary>
		private void FixUpMethodCalls(AstMethodDefinition source)
		{
			ILtoASTTransformer il2astTransformer = new ILtoASTTransformer();
			AstMethodDefinition ast;
			int expressionIndex;
			
			MethodInvocationExpression mInvoke;
			MethodReferenceExpression mRef;
			MethodDefinition mDef;
			
			AstPreInsertFixer preFixer = new AstPreInsertFixer();
			
			int vLastIndex = source.Method.Body.Variables.Count;
			VariableDefinition newVariable;
			for (int i = 0; i < blocks.Count; i++) {
				mInvoke = expressions[i].Expression as MethodInvocationExpression;
				mRef = mInvoke.Method as MethodReferenceExpression;
				mDef = mRef.Method.Resolve();
				
//				ast = preFixer.FixUp(il2astTransformer.Transform(mDef), paramVarSubstitution);
				
				expressionIndex = blocks[i].Statements.IndexOf(expressions[i]);
				
				//При рекурсивно извикване на inline-вания метод в себе си се получава -1!!!
				if (expressionIndex >= 0) {
					
					blocks[i].Statements.RemoveAt(expressionIndex);
					
					Expression arg;
					ParameterDefinition paramDef;
					for (int current = mInvoke.Arguments.Count - 1; current >= 0 ; current--) {
						paramDef = mRef.Method.Parameters[current];
						
						arg = mInvoke.Arguments[current];
						newVariable = new VariableDefinition(paramDef.ParameterType);
						newVariable.Method = source.Method;
						newVariable.Index = vLastIndex++;
						newVariable.Name = newVariable.ToString();
						paramVarSubstitution[paramDef] = newVariable;
						
						source.Method.Body.Variables.Add(newVariable);
							
						blocks[i].Statements.Insert(expressionIndex, new ExpressionStatement(
								new AssignExpression(new VariableDeclarationExpression(newVariable), arg)));
						expressionIndex++;
					}
					
				}
				else {
					break;
				}
				
				ast = preFixer.FixUp(il2astTransformer.Transform(mDef), paramVarSubstitution);
				
				for (int j = ast.Block.Statements.Count - 1; j >= 0; j-- ) {
					blocks[i].Statements.Insert(expressionIndex, ast.Block.Statements[j]);
				}
				
				if (mDef.Body.Variables.Count != 0 && (!localVarSubstitution.ContainsKey(mDef.Body.Variables[0]))) {
					
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
		
		
		/// <summary>
		/// Извършва предварителни трансформации върху inline-вания метод, 
		/// за да го подготви за вграждане в извикващия метод.
		/// </summary>
		internal class AstPreInsertFixer : BaseCodeTransformer 
		{
			#region Fields
			
			private Dictionary<ParameterDefinition, VariableDefinition> paramVarSubstitution;
			private AstMethodDefinition source;
			private LabeledStatement exitLabel;
			private static int exitNumber = 0;
			
			#endregion
			
			#region Constructors
			
			public AstPreInsertFixer ()
			{
				
			}
			
			#endregion
			
			
			/// <summary>
			/// Добавя уникални етикети, за да може да се осъществи от return към goto в 
			/// края на inline-вания метод.
			/// </summary>
			public AstMethodDefinition FixUp(AstMethodDefinition source, Dictionary<ParameterDefinition, VariableDefinition> paramVarSubstitution)
			{
				this.source = source;
				this.paramVarSubstitution = paramVarSubstitution;
				
				exitNumber++;
				this.exitLabel = this.source.Block.Statements[this.source.Block.Statements.Count-1] as LabeledStatement;
				
				if (exitLabel == null) {
					this.exitLabel = new LabeledStatement("@_exit" + exitNumber);
					this.source.Block = (BlockStatement) Visit(this.source.Block);	
					this.source.Block.Statements.Add(exitLabel);
				}
				else {
					this.source.Block = (BlockStatement) Visit(this.source.Block);	
				}
				
				return this.source;
			}
			
			#region Model Transformers
			
			/// <summary>
			/// Заменя срещанията на return с goto в края на метода.
			/// </summary>
			public override ICodeNode VisitReturnStatement(ReturnStatement node)
			{
				return new GotoStatement(exitLabel.Label);
			}
			
			/// <summary>
			/// Прави преходите да бъдат към новите, уникални етикиети.
			/// </summary>
			public override ICodeNode VisitGotoStatement(GotoStatement node)
			{
				node.Label = "@_" + source.Method.Name + "@" + exitNumber + node.Label;
				return base.VisitGotoStatement(node);
			}
			
			/// <summary>
			/// Прави уникални етикетите на другите goto констрикции, защото може да има съвпадение
			/// между тези на метода и тези на inline-вания метод.
			/// </summary>
			public override ICodeNode VisitLabeledStatement(LabeledStatement node)
			{
				node.Label = "@_" + source.Method.Name + "@" + exitNumber + node.Label;
				return base.VisitLabeledStatement(node);
			}
			
			public override ICodeNode VisitArgumentReferenceExpression(ArgumentReferenceExpression node)
			{
				VariableDefinition varDef;
				if (paramVarSubstitution.TryGetValue(node.Parameter.Resolve(), out varDef)) {
					return new VariableReferenceExpression(varDef);
				}
				return base.VisitArgumentReferenceExpression(node);
			}
			
			
			#endregion
		}
		
		/// <summary>
		/// Извършва трансформации в края на процеса по inline-ване на методи
		/// </summary>
		internal class AstFinalFixer : BaseCodeTransformer
		{
			#region Fields
			
			private AstMethodDefinition source;
			private Dictionary<VariableDefinition, VariableDefinition> localVarSubstitution;
			private HashSet<VariableDefinition> isVariableDefined = new HashSet<VariableDefinition>();
			private AssignExpression lastAssignment;
			
			#endregion
			
			#region Constructors
			
			public AstFinalFixer()
			{
			}
			
			#endregion
			
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
			
			#region Model Transformations
			
			/// <summary>
			/// 
			/// </summary>
			public override ICodeNode VisitExpressionStatement(ExpressionStatement node)
			{
				var result = base.VisitExpressionStatement(node);
				if (node.Expression == null) {
					return null;
				}
				return result;
			}
			
			/// <summary>
			/// Съхранява последното присвояване
			/// </summary>
			public override ICodeNode VisitAssignExpression(AssignExpression node)
			{
				lastAssignment = node;
				return base.VisitAssignExpression(node);
			}
			
			/// <summary>
			/// При срещане на референция към променлива се прилага споменатата субституция.
			/// </summary>
			public override ICodeNode VisitVariableReferenceExpression(VariableReferenceExpression node)
			{
				VariableDefinition varDef;
				if (localVarSubstitution.TryGetValue(node.Variable.Resolve(), out varDef)) {
					node.Variable = varDef;
				}
				return base.VisitVariableReferenceExpression(node);
			}
			
			/// <summary>
			/// Прави субституция ако променливата е вече дефинирана и заменя декларацията на променлива
			/// с референция.
			/// </summary>
			public override ICodeNode VisitVariableDeclarationExpression(VariableDeclarationExpression node)
			{
				VariableDefinition varDef;
				if (localVarSubstitution.TryGetValue(node.Variable, out varDef)) {
					if (!isVariableDefined.Add(node.Variable)) {
						var varRef = lastAssignment.Target as VariableDeclarationExpression;
						if (varRef != null) {
							if (varRef.Variable == varDef) {
								throw new NotImplementedException();
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
			
			#endregion
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
