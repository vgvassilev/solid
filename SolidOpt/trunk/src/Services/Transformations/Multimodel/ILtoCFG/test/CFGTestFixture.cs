/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;

using Mono.Cecil;

using NUnit.Framework;

using SolidOpt.Services.Transformations.Multimodel.ILtoCFG;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

using SolidOpt.Services.Transformations.Multimodel.Test;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoCFG.Test
{
  [TestFixture]
  public sealed class CFGTestFixture
    : BaseTestFixture<MethodDefinition, ControlFlowGraph, CilToControlFlowGraph>
  {
    private string testCasesDirCache = null;

    protected override string GetTestCaseFileExtension()
    {
      return "il";
    }

    protected override string GetTestCaseResultFileExtension()
    {
      return "il.cfg";
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
      testCasesDirCache = Path.Combine(testCasesDirCache, "ILtoCFG");
      testCasesDirCache = Path.Combine(testCasesDirCache, "test");
      testCasesDirCache = Path.Combine(testCasesDirCache, "TestCases");
      return testCasesDirCache;
    }

    [Test, TestCaseSource("GetTestCases")] /*Comes from the base class*/
    public void Cases(string filename)
    {
      RunTestCase(filename, LoadTestCaseMethod(filename));
    }
  }
}
