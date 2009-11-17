/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 14.7.2009 г.
 * Time: 09:45
 * 
 */
using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Cecil.Decompiler;
using Cecil.Decompiler.Cil;
using Cecil.Decompiler.Languages;

using AstMethodDefinitionModel; 

using SolidOpt.Services.Transformations.Multimodel;

namespace ASTtoIL 
{
	/// <summary>
	/// Description of ASTtoILTransformer.
	/// </summary>
	public class ASTtoILTransformer : ICompile<AstMethodDefinition, MethodDefinition>
	{
		public ASTtoILTransformer()
		{
		}
		
		public MethodDefinition Compile(AstMethodDefinition source)
		{
			Compiler compiler = new Compiler(source.Block, source.Method.Body.CilWorker);
			compiler.Compile();
			return source.Method;
		}
	}
}
