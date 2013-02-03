/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;

using Mono.Cecil;

using NUnit.Framework;

using SolidOpt.Services.Transformations.Multimodel.ILtoCG;
using SolidOpt.Services.Transformations.CodeModel.CallGraph;

using SolidOpt.Services.Transformations.Multimodel.Test;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoCG.Test
{
 [TestFixture]
 public sealed class CGTestFixture
   : BaseTestFixture<MethodDefinition, CallGraph, CilToCallGraph>
  {
    private string testCasesDirCache = null;

    protected override string GetTestCaseFileExtension()
    {
      return "il";
    }

    protected override string GetTestCaseResultFileExtension()
    {
      return "il.cg";
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
      testCasesDirCache = Path.Combine(testCasesDirCache, "ILtoCG");
      testCasesDirCache = Path.Combine(testCasesDirCache, "test");
      testCasesDirCache = Path.Combine(testCasesDirCache, "TestCases");
      return testCasesDirCache;
    }

    [Test]
    public void TwoSystemCalls()
    {
      string testCaseName = "TwoSystemCalls";
      MethodDefinition mDef = LoadTestCaseMethod(testCaseName);
      CallGraph cg = new CallGraphBuilder(mDef).Create(/*maxDepth*/1);
      string[] seen = cg.ToString().Split('\n');
      string errMsg = string.Empty;
      Assert.IsTrue(Validate(testCaseName, seen, ref errMsg), errMsg);
    }

    [Test, TestCaseSource("GetTestCases")] /*Comes from the base class*/
    public void Cases(string filename)
    {
      RunTestCase(filename, LoadTestCaseMethod(filename));
    }
  }
}
