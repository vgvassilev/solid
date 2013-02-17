/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Cil;

using NUnit.Framework;

using SolidOpt.Services.Transformations.Multimodel.CFGtoIL;
using SolidOpt.Services.Transformations.Multimodel.ILtoCFG;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

using SolidOpt.Services.Transformations.Multimodel.Test;

namespace SolidOpt.Services.Transformations.Multimodel.CFGtoIL.Test
{
  [TestFixture]
  public sealed class CFGtoILTestFixture
    : BaseTestFixture<ControlFlowGraph, MethodDefinition, ControlFlowGraphToCil>
  {
    private string testCasesDirCache = null;

    protected override string GetTestCaseFileExtension()
    {
      return "il";
    }

    protected override string GetTestCaseResultFileExtension()
    {
      return "il.stored";
    }

    protected override string GetTestCasesDir() 
    {
      if (testCasesDirCache != null)
        return testCasesDirCache;
      
      testCasesDirCache = base.GetTestCasesDir();
      testCasesDirCache = Path.Combine(testCasesDirCache, "..");
      testCasesDirCache = Path.Combine(testCasesDirCache, "src");
      testCasesDirCache = Path.Combine(testCasesDirCache, "Services");
      testCasesDirCache = Path.Combine(testCasesDirCache, "Transformations");
      testCasesDirCache = Path.Combine(testCasesDirCache, "Multimodel");
      testCasesDirCache = Path.Combine(testCasesDirCache, "CFGtoIL");
      testCasesDirCache = Path.Combine(testCasesDirCache, "test");
      testCasesDirCache = Path.Combine(testCasesDirCache, "TestCases");
      return testCasesDirCache;
    }

    [Test, TestCaseSource("GetTestCases")] /*Comes from the base class*/
    public void Cases(string filename)
    {
      MethodDefinition mDef = LoadTestCaseMethod(filename);
      CilToControlFlowGraph transformer = new CilToControlFlowGraph();
      ControlFlowGraph cfg = transformer.Transform(mDef);
      RunTestCase(filename, cfg);
    }

    public override string TargetToString(MethodDefinition target) {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine(target.FullName);
      sb.AppendLine("{");
      foreach(Instruction instr in target.Body.Instructions)
        sb.AppendFormat("  {0}\n", instr.ToString());
      sb.AppendLine("}");
      return sb.ToString();
    }
  }
}
