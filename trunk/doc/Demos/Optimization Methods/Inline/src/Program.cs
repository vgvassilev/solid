/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Cecil.Decompiler;
using Cecil.Decompiler.Cil;
using Cecil.Decompiler.Languages;

namespace SolidOpt //.Documentation.Samples.Inline
{
	class Program
	{
		public static void Main(string[] args)
		{
			var method1 = GetProgramMethod("Inliner");
			FormatIL(Console.Out, method1);
			//var cfg = ControlFlowGraph.Create(method);
			//FormatControlFlowGraph(Console.Out, cfg);
			Console.WriteLine ("--------------------");

			var method2 = GetProgramMethod("SubAB");
			FormatIL(Console.Out, method2);
			//cfg = ControlFlowGraph.Create(method);
			//FormatControlFlowGraph(Console.Out, cfg);
			Console.WriteLine ("--------------------");
			
			var tr = new SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.InlineTransformer();
			Console.WriteLine ("--------------------");
			method1 = tr.Optimize(method1);
			Console.WriteLine ("--------------------");
			FormatIL(Console.Out, method1);
			//cfg = ControlFlowGraph.Create(method);
			//FormatControlFlowGraph(Console.Out, cfg);
			Console.WriteLine ("--------------------");
			
			
/*

//			var store = AnnotationStore.CreateStore (cfg, BlockOptimization.Detailed);
//			PrintAnnotations (method, store);

			var language = CSharp.GetLanguage(CSharpVersion.V1);
			//var body = method.Body.Decompile(language);
			var writer = language.GetWriter(new PlainTextFormatter (Console.Out));
			writer.Write(method);
*/			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		public static void FormatControlFlowGraph(TextWriter writer, ControlFlowGraph cfg)
		{
			foreach (InstructionBlock block in cfg.Blocks) {
				writer.WriteLine ("block {0}:", block.Index);
				writer.WriteLine ("\tbody:");
				foreach (Instruction instruction in block) {
					writer.Write ("\t\t");
					var data = cfg.GetData (instruction);
					writer.Write ("[{0}:{1}] ", data.StackBefore, data.StackAfter);
					Formatter.WriteInstruction (writer, instruction);
					writer.WriteLine ();
				}
				InstructionBlock [] successors = block.Successors;
				if (successors.Length > 0) {
					writer.WriteLine ("\tsuccessors:");
					foreach (InstructionBlock successor in successors) {
						writer.WriteLine ("\t\tblock {0}", successor.Index);
					}
				}
			}
		}
		
		public static void FormatIL(TextWriter writer, MethodDefinition method)
		{
			writer.WriteLine(method.ToString());
			foreach(Instruction instruction in method.Body.Instructions) {
				writer.Write("\t");
				Formatter.WriteInstruction(writer, instruction);
				writer.WriteLine();
			}
		}
		
		static MethodDefinition GetProgramMethod(string name)
		{
			foreach (MethodDefinition method in GetProgramAssembly().MainModule.GetType("SolidOpt.Program").Methods) { //.Documentation.Samples.Inline.Program").Methods) {
				if (method.Name == name) return method;
			}
			return null;
		}
		
		static IAssemblyResolver resolver = new DefaultAssemblyResolver();

		static AssemblyDefinition GetProgramAssembly()
		{
			return AssemblyDefinition.ReadAssembly(typeof(Program).Module.FullyQualifiedName);
		}
		
		
		public static void Inliner()
		{
			int a = 5;
			int b = a + 2;
			int c = 1 + SubAB(a + b, b) + 2;
			if (c > 10)
				Console.Write(c);
			else
				Console.Write(a + b + c);
		}

		[SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Inlineable]
		public static int SubAB(int a, int b)
		{
			return a - b;
		}
		
/*
 		public static void Inliner()
		{
			int a = 5;
			int b = a + 2;
			
			// int temp1 = 1;
			// int temp2 = SubAB(a + b, b);
			// int c = temp1 + temp2 + 2;

			int temp1 = 1;
			int temp2 = a + b - b;
			int c = temp1 + temp2 + 2;
			
			if (c > 10)
				Console.Write(c);
			else
				Console.Write(a + b + c);
		}		
 */
		public static void Inliner2()
		{
			int a = 5;
			int b = a + 2;
			int c = 1 + SubAB2(a + b, b) + 2;
			if (c > 10)
				Console.Write(c);
			else
				Console.Write(a + b + c);
		}
		
		public static int SubAB2(int a, int b)
		{
			if (a > 2)
				return a - b;
			else
				return a + b;
		}
		
/*
 		public static void Inliner2()
		{
			int a = 5;
			int b = a + 2;

			// int temp1 = 1;
			// int temp2 = SubAB(a + b, b);
			// int c = temp1 + temp2 + 2;

			// int temp1 = 1;
			// if (a + b > 2)
			// 	temp2 = a + b - b;
			// else
			// 	temp2 = a + b + b;
			// int c = temp1 + temp2 + 2;

			int temp1 = 1;
			int temp_p1 = a + b;
			int temp_p2 = b;
			if (temp_p1 > 2)
				temp2 = temp_p1 - temp_p2;
				goto e1;
			else
				temp2 = temp_p1 + temp_p2;
				goto e1;
			e1:
			int c = temp1 + temp2 + 2;

			if (c > 10)
				Console.Write(c);
			else
				Console.Write(a + b + c);
		}		
 */
	}
}