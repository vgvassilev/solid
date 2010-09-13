/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 02.7.2009 г.
 * Time: 15:21
 * 
 */
using System;

using Mono.Cecil;

using Cecil.Decompiler.Ast;

namespace SolidOpt.Services.Transformations.Multimodel.AstMethodDefinitionModel
{
	/// <summary>
	/// Description of AstMethodDefinition.
	/// </summary>
	public class AstMethodDefinition
	{
		
		private MethodDefinition method;		
		public MethodDefinition Method {
			get { return method; }
			set { method = value; }
		}
		
		private BlockStatement block;
		public BlockStatement Block {
			get { return block; }
			set { block = value; }
		}
		
		public AstMethodDefinition()
		{
		}
		
		public AstMethodDefinition(MethodDefinition method, BlockStatement block)
		{
			this.method = method;
			this.block = block;
		}
	}
}
