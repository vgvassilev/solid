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
			
			foreach (ModuleDefinition module in assembly.Modules)
				foreach (TypeDefinition type in module.Types) 
					for (int i = 0; i < type.Methods.Count; i++) {
//						if ( i != 1 ) continue;
						MethodDefinition method = type.Methods[i];
						Console.WriteLine(method.Name);
						foreach (ITransform<MethodDefinition> transformer in IL2ILTransformers) {
							method = transformer.Transform(method);
							foreach (Instruction instruction in method.Body.Instructions) {
								Console.Write ("\t\t");
								Cecil.Decompiler.Cil.Formatter.WriteInstruction (Console.Out, instruction);
								Console.WriteLine ();
							}
						}
						
						AstMethodDefinition ast = IL2ASTtransformer.Transform(method);
						
						foreach (ITransform<AstMethodDefinition> transformer in AST2ASTTransformers) {
							ast = transformer.Transform(ast);
						}
						
						method = AST2ILtransformer.Transform(ast);
						
						foreach (ITransform<MethodDefinition> transformer in IL2ILTransformers) {
							method = transformer.Transform(method);
						}
						
						type.Methods[i] = method;
					}
			
			
//			assembly.MainModule.Accept(new StructureVisitor(transformers));
			AssemblyFactory.SaveAssembly(assembly, Path.ChangeExtension(args[0], ".modified" + Path.GetExtension(args[0])));
		}
		
		
	}
}
