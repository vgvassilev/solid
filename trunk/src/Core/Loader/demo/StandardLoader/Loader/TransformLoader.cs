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
using SolidOpt.Services.Transformations;
using SolidOpt.Services.Transformations.Optimizations;
using SolidOpt.Services.Transformations.Multimodel;

using SolidOpt.Core.Loader;

using SolidOpt.Services.Transformations.Multimodel.AstMethodDefinitionModel;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

using SolidOpt.Services.Transformations.Multimodel.CilToControlFlowGraph;

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
		
#if (DEBUG)
		const string build_type = "Debug";
#else
		const string build_type = "Release";
#endif
		
		public override void LoadServices(string[] args)
		{
			string basepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine("..", ".."));
			StreamReader sr = new StreamReader(Path.Combine(basepath, "plugins.list"));
			while (!sr.EndOfStream) {
				string s = sr.ReadLine();
//				foreach (string s in GetPlatformDependentPath(sr))
				if (!string.IsNullOrEmpty(s)) {
					s = s.Replace("\\", Path.DirectorySeparatorChar.ToString());
					s = s.Replace("/", Path.DirectorySeparatorChar.ToString());
					string name = Path.Combine(basepath, s);
					//TODO: For files in plugins.list use "name" with $(Configuration) before filename, on fail use "name"
					if (File.Exists(name)) {
						ServicesContainer.AddPlugin(name);
					} else {
						string s1 = Path.Combine(name, build_type);
						if (Directory.Exists(s1)) {
							ServicesContainer.AddPlugins(s1);
						} else if (Directory.Exists(name)) {
							ServicesContainer.AddPlugins(name);
						}
					}
				}
			}
			
			base.LoadServices(args);
		}
		
//		/// <summary>
//		/// Replaces the directory separator in the file with the current dir separator.  
//		/// </summary>
//		/// <param name="sr">
//		/// A <see cref="StreamReader"/>
//		/// </param>
//		/// <returns>
//		/// A <see cref="IEnumerable"/>
//		/// </returns>
//		private static IEnumerable GetPlatformDependentPath(StreamReader sr)
//		{
//			char separator = Path.DirectorySeparatorChar;
//			
//			while (!sr.EndOfStream) {
//				string s = sr.ReadLine();
//				if ( '/' != separator && s.Contains("/") ) {
//					yield return s.Replace('/', separator);
//				} 
//				else if ('\\' != separator && s.Contains("\\")) {
//					yield return s.Replace('\\', separator);
//				}
//			}
//		}
		
		public List<Type> TransformGraphNodes = new List<Type>();
		public Dictionary<KeyValuePair<Type, Type>, object> TransformGraphLinks = new Dictionary<KeyValuePair<Type, Type>, object>();
		
		public void CreateTransformGraph()
		{
			TransformGraphNodes.Add(typeof(MethodDefinition));
			TransformGraphNodes.Add(typeof(AstMethodDefinition));
			TransformGraphNodes.Add(typeof(ControlFlowGraph));

			
			TransformGraphLinks.Add(new KeyValuePair<Type,Type>(typeof(MethodDefinition),typeof(AstMethodDefinition)), ServicesContainer.GetService<IDecompile<MethodDefinition, AstMethodDefinition>>());
			TransformGraphLinks.Add(new KeyValuePair<Type,Type>(typeof(AstMethodDefinition),typeof(MethodDefinition)), ServicesContainer.GetService<ICompile<AstMethodDefinition, MethodDefinition>>());

			TransformGraphLinks.Add(new KeyValuePair<Type,Type>(typeof(MethodDefinition),typeof(ControlFlowGraph)), ServicesContainer.GetService<IDecompile<MethodDefinition, ControlFlowGraph>>());
			//TransformGraphLinks.Add(new KeyValuePair<Type,Type>(typeof(ControlFlowGraph),typeof(MethodDefinition)), ServicesContainer.GetService<ICompile<ControlFlowGraph, MethodDefinition>>());

			//TransformGraphLinks.Add(new KeyValuePair<Type,Type>(typeof(ControlFlowGraph),typeof(AstMethodDefinition)), ServicesContainer.GetService<IDecompile<ControlFlowGraph, AstMethodDefinition>>());
			//TransformGraphLinks.Add(new KeyValuePair<Type,Type>(typeof(AstMethodDefinition),typeof(ControlFlowGraph)), ServicesContainer.GetService<ICompile<AstMethodDefinition, ControlFlowGraph>>());
		}
		
		public override void Transform(string[] args)
		{
			AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(args[0]);
//			assembly.MainModule.LoadSymbols();
			
			CreateTransformGraph();
			
			List<object> optList = new List<object>();
			
			IService[] services = ServicesContainer.GetServices();
			
			StreamReader sr = new StreamReader(args[1]);
			while (!sr.EndOfStream) {
				string s = sr.ReadLine();
				if (!string.IsNullOrEmpty(s) && s[0] != '#') {
					foreach (IService service in services) {
						if (service.GetType().FullName == s) {
							optList.Add(service);
						}
					}
				}
			}
			
			Console.WriteLine(optList);
			
			
			IDecompile<MethodDefinition, ControlFlowGraph> ILtoCfgTransformer = 
				ServicesContainer.GetService<IDecompile<MethodDefinition, ControlFlowGraph>>();
			
//			IService[] transformers = (IService[]) ServicesContainer.GetServices(typeof(ITransform<MethodDefinition>));
			List<IOptimize<MethodDefinition>> IL2ILTransformers = 
				ServicesContainer.GetServices<IOptimize<MethodDefinition>>();
			IDecompile<MethodDefinition, AstMethodDefinition> IL2ASTtransformer = 
				ServicesContainer.GetService<IDecompile<MethodDefinition, AstMethodDefinition>>();
			
			List<IOptimize<AstMethodDefinition>> AST2ASTTransformers = 
				ServicesContainer.GetServices<IOptimize<AstMethodDefinition>>();
			ICompile<AstMethodDefinition, MethodDefinition> AST2ILtransformer = 
				ServicesContainer.GetService<ICompile<AstMethodDefinition, MethodDefinition>>();
			
			foreach (ModuleDefinition module in assembly.Modules)
				foreach (TypeDefinition type in module.Types) 
					for (int i = 0; i < type.Methods.Count; i++) {
						Console.WriteLine("----------------------------------------");
//						if ( i != 1 ) continue;
						MethodDefinition method = type.Methods[i];
						Console.WriteLine(method.ToString());
						
						//CFG Test section
							
						if (ILtoCfgTransformer != null) {
							ControlFlowGraph cfg = ILtoCfgTransformer.Decompile(method);
							cfg.FormatControlFlowGraph(Console.Out);
						}
					
						//end
					
						foreach (IOptimize<MethodDefinition> transformer in IL2ILTransformers) {
							method = transformer.Optimize(method);
						}
						
 						foreach (Instruction instruction in method.Body.Instructions) {
								Console.Write ("\t\t");
								Cecil.Decompiler.Cil.Formatter.WriteInstruction (Console.Out, instruction);
								Console.WriteLine ();
						}
						
						AstMethodDefinition ast = null;
						if (IL2ASTtransformer != null) {
							ast = IL2ASTtransformer.Decompile(method);
						}
						
//						Console.WriteLine("{0}", ast.Method.ToString());
//						WriteAST(ast.Block);
						
						List<string> list = new List<string>(){"InlineTransformer", "SimplifyExpressionTransformer", "ConstantFoldingTransformer"};
						foreach (string s in list) {
							foreach (IOptimize<AstMethodDefinition> transformer in AST2ASTTransformers) {
//								Console.WriteLine(transformer.GetType().Name);
								if (transformer.GetType().Name == s) {
									ast = transformer.Optimize(ast);
								}
							}
							
						}
						
						
						
//						Console.WriteLine(method.ToString());
//						WriteCode(ast.Block);
//						Console.WriteLine("Abstract Syntax Tree");
//						WriteAST(ast.Block);
						
						
						if (AST2ILtransformer != null)
							method = AST2ILtransformer.Compile(ast);
						
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
