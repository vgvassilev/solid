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
    public void TwoCalls()
    {
      string testCaseName = "TwoCalls";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    [ExpectedException(typeof(XFailException))]
    public void XFailTwoSystemCalls()
    {
      string testCaseName = "TwoSystemCalls";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }
  }
}
