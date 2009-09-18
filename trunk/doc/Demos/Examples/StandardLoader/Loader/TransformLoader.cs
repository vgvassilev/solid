/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 15.7.2009 г.
 * Time: 12:31
 * 
 */
using System;
using System.IO;

using System.Collections.Generic;
//using System.Runtime.Remoting;

using Mono.Cecil.Cil;
using Mono.Cecil;

using SolidOpt.Core.Services;
using SolidOpt.Optimizer.Transformers;

using SolidOpt.Core.Loader;

using AstMethodDefinitionModel;

//
using System.Collections;
using Cecil.Decompiler.Ast;

namespace TransformLoader
{
	/// <summary>
	/// Description of TransformLoader.
	/// </summary>
	public class TransformLoader : Loader
	{
		public TransformLoader()
		{
		}
		
		public override void Transform(string[] args)
		{
//			IService[] transformers = (IService[]) ServicesContainer.GetServices(typeof(ITransform<MethodDefinition>));
			List<ITransform<MethodDefinition>> IL2ILTransformers = 
				ServicesContainer.GetServices<ITransform<MethodDefinition>>();
			ITransform<MethodDefinition, AstMethodDefinition> IL2ASTtransformer = 
				ServicesContainer.GetService<ITransform<MethodDefinition, AstMethodDefinition>>();
			
			List<ITransform<AstMethodDefinition>> AST2ASTTransformers = 
				ServicesContainer.GetServices<ITransform<AstMethodDefinition>>();
			ITransform<AstMethodDefinition, MethodDefinition> AST2ILtransformer = 
				ServicesContainer.GetService<ITransform<AstMethodDefinition, MethodDefinition>>();
			
			
			
			AssemblyDefinition assembly = AssemblyFactory.GetAssembly(args[0]);
			
//			assembly.MainModule.LoadSymbols();
			
			foreach (ModuleDefinition module in assembly.Modules)
				foreach (TypeDefinition type in module.Types) 
					for (int i = 0; i < type.Methods.Count; i++) {
						Console.WriteLine("----------------------------------------");
//						if ( i != 1 ) continue;
						MethodDefinition method = type.Methods[i];
						Console.WriteLine(method.Name);
						foreach (ITransform<MethodDefinition> transformer in IL2ILTransformers) {
							method = transformer.Transform(method);
							
						}
						foreach (Instruction instruction in method.Body.Instructions) {
								Console.Write ("\t\t");
								Cecil.Decompiler.Cil.Formatter.WriteInstruction (Console.Out, instruction);
								Console.WriteLine ();
						}
						
						AstMethodDefinition ast = IL2ASTtransformer.Transform(method);
						
						Console.WriteLine("Before AST2ASTTransformation");
						WriteAST(ast.Block);
						
//						AST2ASTTransformers.Reverse();
						List<string> list = new List<string>(){"InlineTransformer", "ConstantFoldingTransformer"};
						foreach (string s in list) {
							foreach (ITransform<AstMethodDefinition> transformer in AST2ASTTransformers) {
								Console.WriteLine(transformer.GetType().Name);
								if (transformer.GetType().Name == s) {
									
									ast = transformer.Transform(ast);
								}
							}
							
						}
						
						
						Console.WriteLine("After AST2ASTTransformation");
						WriteAST(ast.Block);
						
//						method = AST2ILtransformer.Transform(ast);
//						
//						foreach (ITransform<MethodDefinition> transformer in IL2ILTransformers) {
//							method = transformer.Transform(method);
//						}
						
						type.Methods[i] = method;
					}
			
			
//			assembly.MainModule.Accept(new StructureVisitor(transformers));
//			AssemblyFactory.SaveAssembly(assembly, Path.ChangeExtension(args[0], ".modified" + Path.GetExtension(args[0])));
		}
		
		private void WriteAST(Cecil.Decompiler.Ast.Statement stmt)
		{
			CodeVisitor codeVisitor = new CodeVisitor();
			codeVisitor.Visit(stmt);
			
			Cecil.Decompiler.Languages.ILanguage csharpLang = Cecil.Decompiler.Languages.CSharp.GetLanguage(
				Cecil.Decompiler.Languages.CSharpVersion.V1);
			
			var writer = csharpLang.GetWriter (new Cecil.Decompiler.Languages.PlainTextFormatter (Console.Out));

			writer.Write (stmt);
		}
		
		public class CodeVisitor : Cecil.Decompiler.Ast.BaseCodeVisitor {

			public override void Visit (ICodeNode node)
			{
				if (null == node)
					return;
				
				
				System.Diagnostics.Trace.WriteLine(node.CodeNodeType.ToString());
				
				System.Diagnostics.Trace.Indent();
				base.Visit(node);
				System.Diagnostics.Trace.Unindent();
			}
		}
	}
}
