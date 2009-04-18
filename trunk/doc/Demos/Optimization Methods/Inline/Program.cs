/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 17.4.2009 г.
 * Time: 12:25
 * 
 */
using System;
using System.IO;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Cecil.Decompiler;
using Cecil.Decompiler.Cil;
using Cecil.Decompiler.Languages;



namespace Inline
{
	class Program
	{
		public static void Main(string[] args)
		{
			var method = GetProgramMethod ("Inliner");
			var cfg = ControlFlowGraph.Create (method);
			FormatControlFlowGraph (Console.Out, cfg);
			Console.WriteLine ("--------------------");

//			var store = AnnotationStore.CreateStore (cfg, BlockOptimization.Detailed);
//			PrintAnnotations (method, store);

			var language = CSharp.GetLanguage (CSharpVersion.V1);
			//var body = method.Body.Decompile (language);
			var writer = language.GetWriter (new PlainTextFormatter (Console.Out));
			writer.Write (method);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		public static void FormatControlFlowGraph (TextWriter writer, ControlFlowGraph cfg)
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
		
		static MethodDefinition GetProgramMethod (string name)
		{
			return GetProgramAssembly ().MainModule.Types ["Inline.Program"].Methods.GetMethod (name) [0];
		}
		static IAssemblyResolver resolver = new DefaultAssemblyResolver ();

		static AssemblyDefinition GetProgramAssembly ()
		{
			var assembly = AssemblyFactory.GetAssembly (typeof (Program).Module.FullyQualifiedName);
			assembly.Resolver = resolver;
			return assembly;
		}
		
		
		public static void Inliner()
		{
			int a = 5;
			int b = a + 2;
			int c = SubAB(a + b, b);
			if (c > 10)
				Console.Write(c);
			else
				Console.Write(a + b + c);
		}
		
		public static int SubAB(int a, int b)
		{
			return a - b;
		}
	}
}