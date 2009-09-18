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
		private Dictionary<ParameterDefinition, Expression> paramVarSubstitution =
			new Dictionary<ParameterDefinition, Expression>();
		
		private Expression thisSubstitution;
		
		private AstFinalFixer fixer = new AstFinalFixer();
		
		private static VariableReference returnVariable;
		public static VariableReference ReturnVariable {
			get { return returnVariable; }
			set { returnVariable = value; }
		}
		private static ParameterReference returnParameter;
		public static ParameterReference ReturnParameter {
			get { return returnParameter; }
			set { returnParameter = value; }
		}
		
		private AstMethodDefinition source;
		
		private List<SideEffectInfo> sideEffects = new List<SideEffectInfo>();
		private SideEffectInfo currentSideEffect = new SideEffectInfo();
		
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
			this.source = source;
			source.Block = (BlockStatement) Visit(source.Block);
			source = fixer.FixUp(source, localVarSubstitution);
			
			return source;
		}
		
		#region Model Transformers
		
		/// <summary>
		/// Записва текущия блок, който се обхожда.
		/// </summary>
//		public override ICodeNode VisitBlockStatement(BlockStatement node)
//		{
//			currentBlock = node;
//			return base.VisitBlockStatement(node);
//		}
		
		/// <summary>
		/// Записва текущия израз, който се обхожда.
		/// </summary>
//		public override ICodeNode VisitExpressionStatement(ExpressionStatement node)
//		{
//			currentExpression = node;
//			return base.VisitExpressionStatement(node);
//		}
		
		
		
//		public override ICodeNode VisitAssignExpression(AssignExpression node)
//		{
//			CodeNodeCollection<Expression> collection = new CodeNodeCollection<Expression>();
//			collection.Add(node.Expression);
//			collection = (CodeNodeCollection<Expression>) Visit (collection);
//			
//			if (collection.Count > 0 && collection[0].Equals(node.Expression)) {
//				return node;
//			}
//			
//			node.Expression = collection[collection.Count - 1];
//			collection[collection.Count - 1] = node;
//			return collection;
//		}
		
		public override ICodeNode VisitExpressionStatement(ExpressionStatement node)
		{
			sideEffects.Clear();
			currentSideEffect = new SideEffectInfo();
			
			ICodeNode result = (ICodeNode) Visit (node.Expression);
			CodeNodeCollection<Expression> original = result as CodeNodeCollection<Expression>;
			
			if (original != null) {
				var collection = new CodeNodeCollection<Statement>();
				
				for (int i = 0; i < original.Count; i++) {
					collection.Add(new ExpressionStatement(original[i]));
				}
				
				return (CodeNodeCollection<Statement>) Visit(collection);
			}
			else {
				node.Expression = (Expression) result;
				
				var assignExp = result as AssignExpression;
				if (assignExp != null) {
					var mInvoke = assignExp.Expression as MethodInvocationExpression;
					if (mInvoke != null && IsSimpleInlineCase(mInvoke)) {
						return InlineExpansion(mInvoke, assignExp.Target, source);
					}
					else {
						return node;
					}
				}
				else {
					SideEffectInfo row;
					MethodInvocationExpression mInvoke;
					MethodReferenceExpression mRef;
					var expansion = new CodeNodeCollection<Statement>();
					
					for (int i = 0; i < sideEffects.Count; i++) {
						row = sideEffects[i];
						VariableDefinition @var;
						
						for (int j = 0; j < row.SideEffectsInNode.Count; j++) {
							mInvoke = row.SideEffectsInNode[j];
							mRef = mInvoke.Method as MethodReferenceExpression;
							if (mRef.Method.ReturnType.ReturnType.Name != "Void") {
								expansion.Add(new ExpressionStatement(new AssignExpression(
									new VariableReferenceExpression(row.SideEffectsInNodeVar[j]), mInvoke)));
							}
							else {
								expansion.Add(new ExpressionStatement(mInvoke));
							}
						}
						mRef = row.mInvokeNode.Method as MethodReferenceExpression;
						for (int j = 0; j < row.mInvokeNode.Arguments.Count; j++) {
							ParameterDefinition paramDef = mRef.Method.Parameters[j];
							
							//enable constant folding
							Expression arg = row.mInvokeNode.Arguments[j];
							if (!(arg is ArgumentReferenceExpression || arg is VariableReferenceExpression
				    				|| arg is LiteralExpression)) {
								@var = RegisterVariable(paramDef.ParameterType, source.Method);
							
								expansion.Add(new ExpressionStatement(new AssignExpression(
									new VariableReferenceExpression(@var), arg)));
								row.mInvokeNode.Arguments[j] = new VariableReferenceExpression(@var);
							}
//							@var = RegisterVariable(paramDef.ParameterType, source.Method);
							
//							expansion.Add(new ExpressionStatement(new AssignExpression(
//								new VariableReferenceExpression(@var), row.mInvokeNode.Arguments[j])));
//							row.mInvokeNode.Arguments[j] = new VariableReferenceExpression(@var);
						}
						
						//TODO: Трябва да се оптимизира в случая когато има странични ефекти, а няма инлайн
						//или има странични ефекти останали след последния инлайн.
						for (int j = 0; j < currentSideEffect.SideEffectsInNode.Count; j++) {
							mInvoke = currentSideEffect.SideEffectsInNode[j];
							mRef = mInvoke.Method as MethodReferenceExpression;
							expansion.Add(new ExpressionStatement(new AssignExpression(
								new VariableReferenceExpression(currentSideEffect.SideEffectsInNodeVar[j]), mInvoke)));
						}
						//endtodo.
						
						mRef = row.mInvokeNode.Method as MethodReferenceExpression;
						
						if (mRef.Method.ReturnType.ReturnType.Name != "Void") {
							expansion.Add(new ExpressionStatement(new AssignExpression(
									new VariableReferenceExpression(row.mInvokeNodeVar), row.mInvokeNode)));
						}
						else {
							expansion.Add(new ExpressionStatement(row.mInvokeNode));
						}
					}
					
					expansion.Add(node);
					
					if (expansion.Count > 1) {
						return (CodeNodeCollection<Statement>) Visit(expansion);
					}
					else {
						return node;
					}
				}
			}
		}
		
		public override ICodeNode VisitAssignExpression(AssignExpression node)
		{
			ICodeNode result;
			if (node.Expression is MethodInvocationExpression) {
				var oldExpression = node.Expression;
				
				result = (ICodeNode) base.VisitAssignExpression(node);
				
				AssignExpression assign = (AssignExpression) result;
				var varRef = assign.Expression as VariableReferenceExpression;
				if (varRef != null) {
					assign.Expression = oldExpression;
					return assign;
				} 
//				else {
//					var varDecl = assign.Expression as VariableDeclarationExpression;
//					if (varDecl != null) {
//						assign.Expression = oldExpression;
//						return assign;
//					} 
//				}
			}
			else {
				result = (ICodeNode) base.VisitAssignExpression(node);
			}
			return result;
		}
		
		public override ICodeNode VisitBinaryExpression(BinaryExpression node)
		{
			ICodeNode currentLeft = (ICodeNode) Visit(node.Left);
			ICodeNode currentRight = (ICodeNode) Visit(node.Right);
			
			var argExpand = new CodeNodeCollection<Expression>();
			
			var argAsCollection = currentLeft as CodeNodeCollection<Expression>;
			if (argAsCollection != null) {
				for (int j = 0; j < argAsCollection.Count - 1; j++) {
					argExpand.Add(argAsCollection[j]);
				}
				node.Left = argAsCollection[argAsCollection.Count - 1];
			}
			else {
				node.Left = (Expression) currentLeft;
			}
			
			argAsCollection = currentRight as CodeNodeCollection<Expression>;
			if (argAsCollection != null) {
				for (int j = 0; j < argAsCollection.Count - 1; j++) {
					argExpand.Add(argAsCollection[j]);
				}
				node.Right = argAsCollection[argAsCollection.Count - 1];
			}
			else {
				node.Right = (Expression) currentRight;
			}
			
			if (argExpand.Count > 0) {
				argExpand.Add(node);
				return argExpand;
			}
			else {
				return node;
			}
		}
		
		public override ICodeNode VisitUnaryExpression(UnaryExpression node)
		{
			ICodeNode currentOperand = (ICodeNode) Visit(node.Operand);
			
			var argAsCollection = currentOperand as CodeNodeCollection<Expression>;
			if (argAsCollection != null) {
				node.Operand = argAsCollection[argAsCollection.Count - 1];
				argAsCollection[argAsCollection.Count - 1] = node;
				return argAsCollection;
			}
			else {
				node.Operand = (Expression) currentOperand;
				return node;
			}
		}
		
		
		/// <summary>
		/// Ако извикването на метод е подходящо за inline-ване се записват текущия блок и израз.
		/// На втори пас се замества с реалния код на целевия метод.
		/// </summary>
		public override ICodeNode VisitMethodInvocationExpression(MethodInvocationExpression node)
		{
			MethodReferenceExpression methodRef = (MethodReferenceExpression) node.Method;
				
			if (IsInlineable(methodRef.Method)) {
				currentSideEffect.mInvokeNode = node;
				VariableReferenceExpression varRefExp = null;
				if (methodRef.Method.ReturnType.ReturnType.FullName != "System.Void") {
					currentSideEffect.mInvokeNodeVar = RegisterVariable(methodRef.Method.ReturnType.ReturnType, source.Method);
					varRefExp = new VariableReferenceExpression(currentSideEffect.mInvokeNodeVar);
				}
				sideEffects.Add(currentSideEffect);
				currentSideEffect = new SideEffectInfo();
				return varRefExp;
				
			}
//			else if (HasSideEffects(methodRef.Method)) {
//				currentSideEffect.SideEffectsInNode.Add(node);
//				VariableDefinition @var = RegisterVariable(methodRef.Method.ReturnType.ReturnType, source.Method);
//				currentSideEffect.SideEffectsInNodeVar.Add(@var);
//				return new VariableReferenceExpression(@var);
//				
//			}
			
			//node.Method = (Expression) Visit(node.Method);
			
			var argExpand = new CodeNodeCollection<Expression>();
			
			for (int i = 0; i < node.Arguments.Count; i++) {
				ICodeNode currentArgument = (ICodeNode) Visit(node.Arguments[i]); 
				var argAsCollection = currentArgument as CodeNodeCollection<Expression>;
				
				if (argAsCollection != null) {
					for (int j = 0; j < argAsCollection.Count - 1; j++) {
						argExpand.Add(argAsCollection[j]);
					}
					node.Arguments[i] = argAsCollection[argAsCollection.Count - 1];
				}
			}
			if (argExpand.Count > 0) {
				argExpand.Add(node);
				return argExpand;
			}
			else {
				return node;
			}
		}
		
		
		public override ICodeNode VisitIfStatement(IfStatement node)
		{
			return base.VisitIfStatement(node);
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
		
		private bool HasSideEffects(MethodReference method)
		{
			foreach (CustomAttribute ca in method.Resolve().CustomAttributes) {
				if (ca.Constructor.DeclaringType.FullName == (typeof(SideEffectsAttribute).FullName)) {
					bool a = (bool) ca.ConstructorParameters[0];
					return a;
				}
			}
			return true;
		}
		
		private bool IsSimpleInlineCase(MethodInvocationExpression mInvoke)
		{
			foreach (Expression arg in mInvoke.Arguments) {
				if (!(arg is ArgumentReferenceExpression || arg is VariableReferenceExpression
				    || arg is LiteralExpression)) {
					return false;
				}
			}
			
			return true;
		}
		
		private CodeNodeCollection<Statement> InlineExpansion(MethodInvocationExpression mInvoke, Expression target, AstMethodDefinition source)
		{
			ILtoASTTransformer il2astTransformer = new ILtoASTTransformer();
			AstMethodDefinition ast;
			
			AstPreInsertFixer preFixer = new AstPreInsertFixer();
			
			MethodReferenceExpression mRef = mInvoke.Method as MethodReferenceExpression;

			MethodDefinition mDef = mRef.Method.Resolve();
			var result = new CodeNodeCollection<Statement> ();
			ParameterDefinition paramDef;
			Expression arg;
			
			ReturnVariable = null;
			ReturnParameter = null;
			if (target == null) {
				if (mDef.ReturnType.ReturnType.FullName != "System.Void") {
					ReturnVariable = RegisterVariable(mDef.ReturnType.ReturnType, source.Method);
				}
			} else if (target is VariableReferenceExpression) {
				ReturnVariable = (target as VariableReferenceExpression).Variable;
			} else if (target is VariableDeclarationExpression) {
				ReturnVariable = RegisterVariable(mDef.ReturnType.ReturnType, source.Method);
			} else if (target is ArgumentReferenceExpression) {
				ReturnParameter = (target as ArgumentReferenceExpression).Parameter;
			}
			
			//заместване на this
			
			if (mRef.Target != null) {
				thisSubstitution = new VariableReferenceExpression(
												RegisterVariable(mDef.This.ParameterType, source.Method));
			}
			else {
				thisSubstitution = null;
			}
			
			for (int current = mInvoke.Arguments.Count - 1; current >= 0 ; current--) {
				arg = mInvoke.Arguments[current];
				paramDef = mRef.Method.Parameters[current];
				paramVarSubstitution[paramDef] = arg;
			}
		
			//Ако вече е inline-вано
			if (mDef.Body.Variables.Count != 0 && (!localVarSubstitution.ContainsKey(mDef.Body.Variables[0]))) {
				foreach (VariableDefinition variable in mDef.Body.Variables) {
					localVarSubstitution[variable] = RegisterVariable(variable.VariableType, source.Method);
				}
			}
			
//			foreach (KeyValuePair<VariableDefinition, VariableDefinition> pair in localVarSubstitution) {
//				Console.WriteLine("!!! {0} -> {1}", pair.Key.Name, pair.Value.Name);
//			}
			
			ast = preFixer.FixUp(il2astTransformer.Transform(mDef), paramVarSubstitution, thisSubstitution);
			
			
			//this
			if (thisSubstitution != null) {
				result.Add(new ExpressionStatement(new AssignExpression(thisSubstitution, mRef.Target)));
			}
			
			for (int i = 0; i < ast.Block.Statements.Count; i++ ) {
				result.Add(ast.Block.Statements[i]);
			}
			
			return result;
		}
		
		/// <summary>
		/// Добавя новата променлива към тялото на зададен метод
		/// </summary>
		/// <param name="type">Тип на променливата</param>
		/// <param name="method">Методът, където ще бъде добавена</param>
		/// <returns>Новата променлива</returns>
		internal VariableDefinition RegisterVariable(TypeReference type, MethodDefinition method)
		{
			VariableDefinition variable = new VariableDefinition(type);
			variable.Method = method;
			variable.Index = method.Body.Variables.Count;
			variable.Name = variable.ToString();
			method.Body.Variables.Add(variable);
			
			return variable;
		}
		
		
		/// <summary>
		/// Извършва предварителни трансформации върху inline-вания метод, 
		/// за да го подготви за вграждане в извикващия метод.
		/// </summary>
		internal class AstPreInsertFixer : BaseCodeTransformer 
		{
			#region Fields
			
			private Dictionary<ParameterDefinition, Expression> paramVarSubstitution;
			private AstMethodDefinition source;
			private LabeledStatement exitLabel;
			private static int exitNumber = 0;
			private BlockStatement currentBlock;
			private Expression thisSubstitution;
			
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
			public AstMethodDefinition FixUp(AstMethodDefinition source, Dictionary<ParameterDefinition, Expression> paramVarSubstitution, Expression thisSubstitution)
			{
				this.source = source;
				this.paramVarSubstitution = paramVarSubstitution;
				this.thisSubstitution = thisSubstitution;
				
				exitNumber++;
				this.exitLabel = this.source.Block.Statements[this.source.Block.Statements.Count-1] as LabeledStatement;
				
				if (exitLabel == null) {
					this.exitLabel = new LabeledStatement("@_exit" + exitNumber);
					this.source.Block.Statements.Add(exitLabel);
					this.source.Block = (BlockStatement) Visit(this.source.Block);
				}
				else {
					this.source.Block = (BlockStatement) Visit(this.source.Block);	
				}
				
				return this.source;
			}
			
			#region Model Transformers
			
			
			public override ICodeNode VisitBlockStatement(BlockStatement node)
			{
				currentBlock = node;
				return base.VisitBlockStatement(node);
			}
			
			
			/// <summary>
			/// Заменя срещанията на return с goto в края на метода.
			/// </summary>
			public override ICodeNode VisitReturnStatement(ReturnStatement node)
			{
				GotoStatement @goto = new GotoStatement(exitLabel.Label);
				if (node.Expression != null) {
//					BlockStatement block = new BlockStatement();
//					block.Statements.Add(new ExpressionStatement(new AssignExpression(new VariableReferenceExpression(ReturnVariable), node.Expression)));
//					block.Statements.Add(@goto);
					CodeNodeCollection<Statement> collection = new CodeNodeCollection<Statement>();
					if (ReturnVariable != null) {
						collection.Add(new ExpressionStatement(
							new AssignExpression(new VariableReferenceExpression(ReturnVariable), node.Expression)));
					}
					else if (ReturnParameter != null) {
						collection.Add(new ExpressionStatement(
							new AssignExpression(new ArgumentReferenceExpression(ReturnParameter), node.Expression)));
					}
					else {
						collection.Add(new ExpressionStatement(node.Expression));
					}
					collection.Add(@goto);
					return Visit<CodeNodeCollection<Statement>, Statement>(collection);
//					return collection;
					
//					return (BlockStatement) Visit(block);
					
//					currentBlock.Statements.Add(new ExpressionStatement(
//						new AssignExpression(new VariableReferenceExpression(ReturnVariable), node.Expression)));
//					currentBlock.Statements.Add(@goto);
//					
//					return null;
				}
				
				return @goto;
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
				Expression exp;
				if (paramVarSubstitution.TryGetValue(node.Parameter.Resolve(), out exp)) {
					return exp;
				}
				return base.VisitArgumentReferenceExpression(node);
			}
			
			public override ICodeNode VisitFieldReferenceExpression(FieldReferenceExpression node)
			{
				if (thisSubstitution != null && node.Target == null
				    && node.Field.DeclaringType == source.Method.DeclaringType) {
					
					node.Target = thisSubstitution;					
				}
				
				return base.VisitFieldReferenceExpression(node);
			}
			
			public override ICodeNode VisitMethodReferenceExpression(MethodReferenceExpression node)
			{
				if (thisSubstitution != null && node.Target == null
				    && node.Method.DeclaringType == source.Method.DeclaringType) {
					
					node.Target = thisSubstitution;					
				}
				
				return base.VisitMethodReferenceExpression(node);
			}
			
			public override ICodeNode VisitThisReferenceExpression(ThisReferenceExpression node)
			{
				return thisSubstitution;
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
	
	public class SideEffectsAttribute : Attribute
	{
		public bool HasSideEffects = false;
		
		public SideEffectsAttribute(bool HasSideEffects)
		{
			this.HasSideEffects = HasSideEffects;
		}
	}
	
	internal class SideEffectInfo
	{
		public MethodInvocationExpression mInvokeNode = null;
		public VariableDefinition mInvokeNodeVar = null;
		public List<MethodInvocationExpression> SideEffectsInNode = new List<MethodInvocationExpression>();
		public List<VariableDefinition> SideEffectsInNodeVar = new List<VariableDefinition>();
		
		
		public SideEffectInfo ()
		{
		}

		public SideEffectInfo (MethodInvocationExpression mInvokeNode, VariableDefinition mInvokeNodeVar,
		                       List<MethodInvocationExpression> SideEffectsInNode, List<VariableDefinition> SideEffectsInNodeVar)
		{
			this.mInvokeNode = mInvokeNode;
			this.mInvokeNodeVar = mInvokeNodeVar;
			this.SideEffectsInNode = SideEffectsInNode;
			this.SideEffectsInNodeVar = SideEffectsInNodeVar;
		}
	}
}


