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
 public class CGTestFixture : BaseTestFixture<MethodDefinition, CallGraph, CilToCallGraph> {

    protected override string GetTestCaseFileExtension()
    {
      return "il";
    }

    protected override string GetTestCaseResultFileExtension()
    {
      return "il.cg";
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
