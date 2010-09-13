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

using SolidOpt.Services.Transformations.Multimodel.AstMethodDefinitionModel; 

using SolidOpt.Services.Transformations.Multimodel;

namespace SolidOpt.Services.Transformations.Multimodel.ASTtoIL 
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
			//Mono.Cecil 0.9.3 migration: Compiler compiler = new Compiler(source.Block, source.Method.Body.CilWorker);
			Compiler compiler = new Compiler(source.Block, source.Method.Body.GetILProcessor(), source.Method.Body);
			compiler.Compile();
			return source.Method;
		}
	}
}
