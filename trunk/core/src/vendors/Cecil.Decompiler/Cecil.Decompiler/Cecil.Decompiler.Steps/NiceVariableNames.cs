/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 31.7.2009 г.
 * Time: 19:38
 * 
 */
using System;
using System.Collections.Generic;

using Mono.Cecil.Cil;

using Cecil.Decompiler.Ast;

namespace Cecil.Decompiler.Steps
{
	/// <summary>
	/// Това е стъпка за използване на "по-четими" имена на локалните променливи.
	/// Имената ще бъдат от вида име_на_тип___индекс (string string_1)
	/// </summary>
	public class NiceVariableNames : BaseCodeTransformer, IDecompilationStep
	{
		public static readonly IDecompilationStep Instance = new NiceVariableNames ();

		private DecompilationContext context;
		
		/// <summary>
		/// Хеш-таблица, в която ще се запомня последно срещнат индекс от типа.
		/// </summary>
		private Dictionary<string, int> typeIndex = new Dictionary<string, int>(8);
		
		public BlockStatement Process (DecompilationContext context, BlockStatement block)
		{
			this.context = context;
			GenerateNames();
			return block;
		}
		
		/// <summary>
		/// Генерира имената на променливите от тяхния тип. За типовете декларирани в System 
		/// не използва псевдоними (int int_1), a името на типа (int int32_1). В анализа са 
		/// включени и генеричните типове.
		/// </summary>
		private void GenerateNames()
		{
			typeIndex.Clear();
			string variableType, name;
			int dotIndex, genericIndex, index;
			
			foreach (VariableDefinition variable in context.Variables) {
				variableType = variable.VariableType.FullName;
				dotIndex = variableType.LastIndexOf(".") + 1;
				genericIndex = variableType.IndexOf("<") - 1;
				
				if (genericIndex > 0) {
					variableType = variableType.Substring(0, genericIndex - 1);
					dotIndex = variableType.LastIndexOf(".") + 1;
				}
				
				if (!typeIndex.TryGetValue(variableType, out index)) {
					typeIndex[variableType] = 1;
				}
				else {
					typeIndex[variableType]++;
				}
				
				name = variableType.Substring(dotIndex)+ "_" + ++index;
				name = name.Replace(name[0], char.ToLower(name[0]));

				variable.Name = name;
			}
		}
		
	}
}
