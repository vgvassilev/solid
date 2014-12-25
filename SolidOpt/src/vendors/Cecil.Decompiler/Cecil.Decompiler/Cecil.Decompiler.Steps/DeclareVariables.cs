/*
 * User: Vassil Vassilev
 * Date: 30.7.2009 г.
 * Time: 11:03
 */
 
using System;
using System.Linq;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Cecil.Decompiler.Ast;
using Cecil.Decompiler.Cil;

namespace Cecil.Decompiler.Steps
{
	/// <summary>
	/// Декомпилационната стъпка анализира всички променливи и намира най-подходящото място за тяхното 
	/// деклариране. Алгоритъмът става на два паса. Първият анализира и съхранява общият блок на видимост 
	/// на променливите като игнорира методите с out параметри. Вторият декларира променливите на описаните 
	/// в помощните структури от данни места.
	/// </summary>
	public class DeclareVariables : BaseCodeTransformer, IDecompilationStep
	{
	#region Private Fields
	
		/// <summary>
		/// Класът трябва да се използва като СЕК.
		/// </summary>
		public static readonly IDecompilationStep Instance = new DeclareVariables ();

		private DecompilationContext context;
		
		/// <summary>
		/// Хеш-таблица съдържа указател към конкретна променлива и текущото състояние на стека.
		/// </summary>
		private Dictionary<VariableDefinition, Stack<BlockStatement>> variables = new Dictionary<VariableDefinition, Stack<BlockStatement>>(8);
		
		/// <summary>
		/// Хеш-таблица съдържа указател към конкретна променлива и информация за мястото 
		/// на първото присвояване на стойност към нея.
		/// <see cref="FirstAssignmentInfo" />
		/// </summary>
		private Dictionary<VariableDefinition, FirstAssignmentInfo> firstAssignment = new Dictionary<VariableDefinition, FirstAssignmentInfo>(8);
		
		/// <summary>
		/// Стек, отразяващ текущият път, достигнат при обхождането.
		/// </summary>
		private Stack<BlockStatement> blockStack = new Stack<BlockStatement>();
		
		/// <summary>
		/// Ще използваме полето когато имаме out променливи.
		/// <example>
		/// Например ако декомпилираме:
		/// <code>
		/// string s1;
		/// dict.TryGetValue(0, out s1);
		/// s1 = "Cecil.Decompiler";
		/// </code>
		/// Бихме получили:
		/// <code>
		/// V_0.TryGetValue(0, out V_1);
		/// string V_1 = "Cecil.Decompiler";
		/// </code>
		/// В случая декомпилираният код се различава сематично от първоначалния код.
		/// </example>
		/// </summary>
		private static readonly FirstAssignmentInfo ignoreFirstAssignment = new FirstAssignmentInfo(
																new AssignExpression(), null);
		
	#endregion
		
	#region BaseCodeTransformer Overrides
	
		/// <summary>
		/// Обхожда абстрактното синтактично дърво, поддържайки стек показващ пътя до конкретния връх.
		/// </summary>
		/// <param name="node">Текущ връх</param>
		/// <returns>Обработен връх</returns>
		public override ICodeNode VisitBlockStatement(BlockStatement node)
		{
			blockStack.Push(node);
			var result = base.VisitBlockStatement(node);
			blockStack.Pop();
			return result;
		}
		
		/// <summary>
		/// При наличие на референция към променлива се проверява дали няма стек в таблицата с 
		/// променливите, ако няма се се записва текущият път (стек).
		/// Ако има записан вече стек се обхождат двата стека (записаният и текущият) и се оставя на върха 
		/// блокът, който е общ предшественик и на двата. По този начин се позволява на променливата да бъде 
		/// декларирана така че да е видима за всички блокове, които я използват.
		/// </summary>
		/// <param name="node">Текущ връх</param>
		/// <returns>Обработен връх</returns>
		public override ICodeNode VisitVariableReferenceExpression (VariableReferenceExpression node)
		{
			var variable = (VariableDefinition) node.Variable;
			BlockStatement[] blockStackArray = blockStack.ToArray();
			int blockArrayIndex = blockStackArray.Count() - 1;
			
			if (variables[variable] == null) {
				variables[variable] = new Stack<BlockStatement>();
//				for(int i = blockStackArray.Count() - 1; i >= 0; i--) {
//					variables[variable].Push(blockStackArray[i]);
//				}
				while (blockArrayIndex >= 0) {
					variables[variable].Push(blockStackArray[blockArrayIndex]);
					blockArrayIndex--;
				}
			}
			else {
//				Console.WriteLine(variable.Name);
				var varStackArray = variables[variable].ToArray();
				
				int varArrayIndex = varStackArray.Count() - 1;
				while (varArrayIndex >= 0 && blockArrayIndex >= 0) {
					if (!(blockStackArray[blockArrayIndex].Equals(varStackArray[varArrayIndex]))) {
						break;
					}
					blockArrayIndex--;
					varArrayIndex--;
				}
				while (varArrayIndex >= 0) {
					variables[variable].Pop();
					varArrayIndex--;
				}
				
			}
			
			return base.VisitVariableReferenceExpression(node);
		}
		
		/// <summary>
		/// Ако срещнем присвояване стойност на променлива, която няма записан стек в таблицата 
		/// означава, че нейната декларация е заедно с присвояването.
		/// <example>
		/// Например:
		/// <code>int a = 5</code>
		/// </example>
		/// В такъв случай записваме променливата в таблицата за променливи декларация-на-първо-присвояване.
		/// </summary>
		/// <param name="node">Текущ връх</param>
		/// <returns>Обработен връх</returns>
		public override ICodeNode VisitAssignExpression(AssignExpression node)
		{
			VariableReferenceExpression varRefExp = node.Target as VariableReferenceExpression;
			if (varRefExp != null) {
				VariableDefinition varDef = varRefExp.Variable.Resolve();
				if (firstAssignment[varDef].assignExpression == null) {
					firstAssignment[varDef]= new FirstAssignmentInfo(node, blockStack.Peek());
				}
			}
			
			return base.VisitAssignExpression(node);
		}
		
		/// <summary>
		/// При срещане на метод с out параметър, записваме стойност, чрез която ще се игнорира 
		/// евентуална декларация при срещане на първо присвояване.
		/// </summary>
		/// <param name="node">Текущ връх</param>
		/// <returns>Обработен връх</returns>
		public override ICodeNode VisitAddressOfExpression(AddressOfExpression node)
		{
			VariableReferenceExpression varRefExp = node.Expression as VariableReferenceExpression;
			if (varRefExp != null) {
				VariableDefinition varDef = varRefExp.Variable.Resolve();
				if (firstAssignment[varDef].assignExpression == null) {
					firstAssignment[varDef] = ignoreFirstAssignment;
				}
			}
			
			return base.VisitAddressOfExpression(node);
		}
	
	#endregion
	
	/// <summary>
	/// Методът е входна точка за декомпилационната стъпка. Извиква метод за запълване на полетата, 
	/// активира Visitor-а и декларира на променливите.
	/// </summary>
	/// <param name="context">Декомпилационен контекст</param>
	/// <param name="block">Конкретния блок</param>
	/// <returns>Новогенериран блок</returns>
	public BlockStatement Process (DecompilationContext context, BlockStatement block)
	{
		this.context = context;
		PopulateVariables ();
		var result = (BlockStatement) VisitBlockStatement (block);
		FixUpVariables();
		return result;
	}
	
	
	#region Private Methods and Classes
		
		/// <summary>
		/// Зарежда нужната информация в стуктурите от данни.
		/// </summary>
		private void PopulateVariables ()
		{
			variables.Clear ();
			firstAssignment.Clear();
			blockStack.Clear();
		
			foreach (VariableDefinition variable in context.Variables) {
				variables[variable] = null;
				firstAssignment[variable] = new FirstAssignmentInfo(null, null);
			}
		}
		
		/// <summary>
		/// Обработва на второ минаване таблиците като вмъква на подходящите места декларациите 
		/// на използваните променливи. Обхождането става в обратна посока за да може индексите 
		/// променливите да са в нарастващ ред.
		/// </summary>
		private void FixUpVariables() 
		{
			foreach (KeyValuePair<VariableDefinition, FirstAssignmentInfo> pair in firstAssignment.Reverse()) {
				
				Stack<BlockStatement> stack = variables[pair.Key];
				
				if (stack == null)
					continue;
				
				BlockStatement block = stack.Peek();
				
				if (pair.Value != ignoreFirstAssignment  && 
				    block.Equals(firstAssignment[pair.Key].inBlock)) {
					pair.Value.assignExpression.Target = 
						new VariableDeclarationExpression(pair.Key);
				}
				else {
					block.Statements.Insert(0, new ExpressionStatement(
						new VariableDeclarationExpression(pair.Key)));
				}
			}
		}
		
		/// <summary>
		/// Класът съдържа информация за първото присвояване на променливата и блока в който се намира то.
		/// </summary>
		private class FirstAssignmentInfo
		{
			public AssignExpression assignExpression;
			public BlockStatement inBlock;
			
			public FirstAssignmentInfo(AssignExpression assignExpression, BlockStatement inBlock)
			{
				this.assignExpression = assignExpression;
				this.inBlock = inBlock;
			}
		}
		
	#endregion
	
	}
	
	
}
