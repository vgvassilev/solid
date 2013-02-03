/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Cil;

using NUnit.Framework;

using SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree;
using SolidOpt.Services.Transformations.Multimodel.ILtoAST;
using SolidOpt.Services.Transformations.Multimodel.Test;
using SolidOpt.Services.Transformations.Optimizations.IL.MethodInline;

namespace SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Test
{
  [TestFixture]
  public sealed class MethodInlineTestFixture
    : BaseTestFixture<MethodDefinition, MethodDefinition, InlineTransformer>
  {
    private string testCasesDirCache = null;
    protected override string GetTestCaseFileExtension()
    {
      return "il";
    }
    
    protected override string GetTestCaseResultFileExtension()
    {
      return "il.inlined";
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
      testCasesDirCache = Path.Combine(testCasesDirCache, "Optimizations");
      testCasesDirCache = Path.Combine(testCasesDirCache, "IL");
      testCasesDirCache = Path.Combine(testCasesDirCache, "MethodInline");
      testCasesDirCache = Path.Combine(testCasesDirCache, "test");
      testCasesDirCache = Path.Combine(testCasesDirCache, "TestCases");
      return testCasesDirCache;
    }

    public override string TargetToString(MethodDefinition target) {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine(target.FullName);
      sb.AppendLine("{");
      foreach (Instruction instr in target.Body.Instructions)
        sb.AppendFormat("  {0}\n", instr.ToString());

      sb.AppendLine("}");
      return sb.ToString();
    }

    [Test, TestCaseSource("GetTestCases")] /*Comes from the base class*/
    public void Cases(string filename)
    {
      RunTestCase(filename, LoadTestCaseMethod(filename));
    }
  }
}

