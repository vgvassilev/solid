/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

using SolidOpt.Services.Transformations;
using SolidOpt.Services.Transformations.Multimodel;

using SolidOpt.Services.Transformations.Multimodel.ILtoCFG;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

namespace SolidOpt.Documentation.Samples.Decompiler
{
  class Program
  {
    
    #region Sample Methods
    
    static void Foo (int a)
    {
      Console.WriteLine (a);
      if ((a > 0 && a < 120) && a > 160) {
        a = 42;
      } else {
        a = 24;
      }
      Console.WriteLine (a);
    }
    
    #endregion
    
    public static void Main (string[] args)
    {
      MethodDefinition method = GetProgramMethod ("Foo");
      
      List<DecompilationStep> steps = new List<DecompilationStep>();
      steps.Add(new CilToControlFlowGraph());
      
      DecompilationPipeline<MethodBody, ControlFlowGraph> dpipeline
        = new DecompilationPipeline<MethodBody, ControlFlowGraph>(steps);
      
      ControlFlowGraph cfg = dpipeline.Run(method.Body);
      
      Console.WriteLine(cfg);
      Console.ReadKey(true);
      
    }
    
    static MethodDefinition GetProgramMethod (string name)
    {
      return GetProgramAssembly().MainModule.GetType("SolidOpt.Documentation.Samples.Decompiler.Program").Methods[1];
    }
  
    static AssemblyDefinition GetProgramAssembly ()
    {
      var assembly = AssemblyDefinition.ReadAssembly(typeof(Program).Module.FullyQualifiedName);
      return assembly;
    }
    
  }
}
