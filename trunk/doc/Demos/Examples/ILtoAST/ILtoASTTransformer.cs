/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 24.6.2009 г.
 * Time: 17:23
 * 
 */
using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Cecil.Decompiler;
using Cecil.Decompiler.Cil;
using Cecil.Decompiler.Languages;

using SolidOpt.Optimizer.Transformers;

namespace ILtoAST
{
	/// <summary>
	/// Description of NopRemoveTransformer.
	/// </summary>
	public class ILtoASTTransformer : ITransform<MethodDefinition, AstMethodDefinition>
	{
		public ILtoASTTransformer()
		{
		}

		public AstMethodDefinition Transform(MethodDefinition source)
		{
			CSharp csharpLang = new CSharp();
			DecompilationPipeline pipeline = csharpLang.CreatePipeline();
			pipeline.Run(source.Body);
			
			return new AstMethodDefinition(source, pipeline.Body);
		}
	}
}
