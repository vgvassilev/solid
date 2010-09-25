/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;

using System.Collections.Generic;
//using System.Runtime.Remoting;

using Mono.Cecil.Cil;
using Mono.Cecil;

using SolidOpt.Services;
using SolidOpt.Services.Transformations.Optimizations;
using SolidOpt.Services.Transformations.Multimodel;

using SolidOpt.Core.Loader;

using SolidOpt.Services.Transformations.Multimodel.AstMethodDefinitionModel;

//
using System.Collections;
using Cecil.Decompiler.Ast;

namespace SolidOpt.Core.Loader.Demo.TransformLoader
{
	/// <summary>
	/// Description of TransformLoader.
	/// </summary>
	public class TransformLoader : Loader
	{
		public TransformLoader()
		{
		}
		
		public virtual void LoadServices(string[] args)
		{
			//TODO: use file plugins.list as config
			//TODO: for folders in plugins.list use plugin + $(Configuration), on fail use plugin
			//TODO: for files in plugins.list use plugin with $(Configuration) before filename, on fail use plugin
			//ServicesContainer.AddPlugins(AppDomain.CurrentDomain.BaseDirectory + "plugins");
			base.LoadServices(args);
		}
		
		public override void Transform(string[] args)
		{
//			IService[] transformers = (IService[]) ServicesContainer.GetServices(typeof(ITransform<MethodDefinition>));
			List<IOptimize<MethodDefinition>> IL2ILTransformers = 
				ServicesContainer.GetServices<IOptimize<MethodDefinition>>();
			IDecompile<MethodDefinition, AstMethodDefinition> IL2ASTtransformer = 
				ServicesContainer.GetService<IDecompile<MethodDefinition, AstMethodDefinition>>();
			
			List<IOptimize<AstMethodDefinition>> AST2ASTTransformers = 
				ServicesContainer.GetServices<IOptimize<AstMethodDefinition>>();
			ICompile<AstMethodDefinition, MethodDefinition> AST2ILtransformer = 
				ServicesContainer.GetService<ICompile<AstMethodDefinition, MethodDefinition>>();
			
			
			//Mono.Cecil 0.9.3 migration: AssemblyDefinition assembly = AssemblyFactory.GetAssembly(args[0]);
			AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(args[0]);
			
//			assembly.MainModule.LoadSymbols();
			
			foreach (ModuleDefinition module in assembly.Modules)
				foreach (TypeDefinition type in module.Types) 
					for (int i = 0; i < type.Methods.Count; i++) {
						Console.WriteLine("----------------------------------------");
//						if ( i != 1 ) continue;
						MethodDefinition method = type.Methods[i];
						Console.WriteLine(method.ToString());
						foreach (IOptimize<MethodDefinition> transformer in IL2ILTransformers) {
							method = transformer.Optimize(method);
						}
/* 						foreach (Instruction instruction in method.Body.Instructions) {
								Console.Write ("\t\t");
								Cecil.Decompiler.Cil.Formatter.WriteInstruction (Console.Out, instruction);
								Console.WriteLine ();
						}
						
						AstMethodDefinition ast = IL2ASTtransformer.Decompile(method);
						
						Console.WriteLine("{0}", ast.Method.ToString());
						WriteAST(ast.Block);
						
						List<string> list = new List<string>(){"InlineTransformer", "SimplifyExpressionTransformer", "ConstantFoldingTransformer"};
						foreach (string s in list) {
							foreach (IOptimize<AstMethodDefinition> transformer in AST2ASTTransformers) {
//								Console.WriteLine(transformer.GetType().Name);
								if (transformer.GetType().Name == s) {
									
									ast = transformer.Optimize(ast);
								}
							}
							
						}
						
						
						
						Console.WriteLine(method.ToString());
						WriteCode(ast.Block);
						Console.WriteLine("Abstract Syntax Tree");
						WriteAST(ast.Block);
*/						
						
//						method = AST2ILtransformer.Transform(ast);
//						
//						foreach (ITransform<MethodDefinition> transformer in IL2ILTransformers) {
//							method = transformer.Transform(method);
//						}
						
						type.Methods[i] = method;
					}
			
			
//			assembly.MainModule.Accept(new StructureVisitor(transformers));
//			AssemblyFactory.SaveAssembly(assembly, Path.ChangeExtension(args[0], ".modified" + Path.GetExtension(args[0])));
			assembly.Write(Path.ChangeExtension(args[0], ".modified" + Path.GetExtension(args[0])));
		}
		
		private void WriteAST(Cecil.Decompiler.Ast.Statement stmt)
		{
			CodeVisitor codeVisitor = new CodeVisitor();
			codeVisitor.Visit(stmt);
		}
		
		private void WriteCode(Cecil.Decompiler.Ast.Statement stmt)
		{
			Cecil.Decompiler.Languages.ILanguage csharpLang = Cecil.Decompiler.Languages.CSharp.GetLanguage(
				Cecil.Decompiler.Languages.CSharpVersion.V1);
			
			var writer = csharpLang.GetWriter (new Cecil.Decompiler.Languages.ColoredConsoleFormatter());
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
