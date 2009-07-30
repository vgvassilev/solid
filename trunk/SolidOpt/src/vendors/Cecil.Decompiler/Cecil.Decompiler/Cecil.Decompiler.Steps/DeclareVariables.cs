/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 30.7.2009 г.
 * Time: 11:03
 * 
 */
using System;
using System.Linq;
using System.Collections.Generic;

using Mono.Cecil.Cil;

using Cecil.Decompiler.Ast;
using Cecil.Decompiler.Cil;

namespace Cecil.Decompiler.Steps
{
	/// <summary>
	/// Description of DeclareVariables.
	/// </summary>
	public class DeclareVariables : BaseCodeTransformer, IDecompilationStep
	{
		public static readonly IDecompilationStep Instance = new DeclareVariables ();

		DecompilationContext context;
		Dictionary<string, Stack<BlockStatement>> variables = new Dictionary<string, Stack<BlockStatement>>(8);
		Stack<BlockStatement> blockStack = new Stack<BlockStatement>();
		
		public override ICodeNode VisitBlockStatement(BlockStatement node)
		{
			blockStack.Push(node);
			base.VisitBlockStatement(node);
			blockStack.Pop();
			return node;
		}
		
		public override ICodeNode VisitVariableReferenceExpression (VariableReferenceExpression node)
		{
			var variable = (VariableDefinition) node.Variable;
			
			if (variables[variable.Name] == null) {
				variables[variable.Name] = blockStack.Take(blockStack.Count);
				//blockStack.CopyTo(variables[variable.Name].ToArray(), 0);
			}
			else {
				var blockStackArray = blockStack.ToArray();
				var varStackArray = variables[variable.Name].ToArray();
				int max = Math.Min(blockStackArray.Count(), varStackArray.Count());
				for (int i = 0; i < max; i++) {
					if (blockStackArray[i] != varStackArray[i]) {
						variables[variable.Name] = variables[variable.Name].Take(i);
						break;
						
						//blockStackArray[i].Statements.Insert(0, new VariableDeclarationExpression(variable));
					}
				}
			}
			
			return node;
		}

		public override ICodeNode VisitVariableDeclarationExpression (VariableDeclarationExpression node)
		{
			
			return node;
		}
//		
//		
//
//		private bool TryDiscardVariable (VariableDefinition variable)
//		{
//			if (!not_assigned.Contains (variable))
//				return false;
//
//			RemoveVariable (variable);
//			return true;
//		}
//
//		void RemoveVariable (VariableDefinition variable)
//		{
//			context.RemoveVariable (variable);
//			not_assigned.Remove (variable);
//		}

		public BlockStatement Process (DecompilationContext context, BlockStatement block)
		{
			this.context = context;
			PopulateVariables ();
			return (BlockStatement) VisitBlockStatement (block);
		}

		void PopulateVariables ()
		{
			variables.Clear ();
		
			foreach (VariableDefinition variable in context.Variables) {
				variables[variable.Name] = null;
			}
		}
		
		private bool IsUsedInBlock(VariableDefinition variable, InstructionBlock block)
		{
			Instruction instruction = block.First;
			while (instruction.Next != null){
				switch (instruction.OpCode.Code) {
					case Code.Ldloc :
					case Code.Ldloc_S :
					case Code.Stloc:
					case Code.Stloc_S :
						VariableDefinition var_operand = instruction.Operand as VariableDefinition;
						if (var_operand != null)
							return var_operand.Index == variable.Index;
						break;
					case Code.Ldloc_0 :
					case Code.Stloc_0 :
						return variable.Index == 0;
					case Code.Ldloc_1 :
					case Code.Stloc_1 :
						return variable.Index == 1;
//???					case Code.Ldloca :
//???					case Code.Ldloca_S : 
					case Code.Stloc_2 :
					case Code.Ldloc_2 :
						return variable.Index == 2;
					case Code.Stloc_3 :
					case Code.Ldloc_3 :
						return variable.Index == 3;
				}
				instruction = instruction.Next;
			}
			return false;
		}
		private void WriteVars() {
			
		}
	}
}
