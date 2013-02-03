/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Diagnostics;
using System.IO;

using NUnit.Framework;

using Mono.Cecil;

using SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree;
using SolidOpt.Services.Transformations.Multimodel.Test;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoAST.Test
{
  [TestFixture()]
  public sealed class ASTTestFixture 
    : BaseTestFixture<MethodDefinition, AstMethodDefinition, ILtoASTTransformer>
  {
    private string testCasesDirCache = null;
    protected override string GetTestCaseFileExtension()
    {
      return "cs";
    }
    
    protected override string GetTestCaseResultFileExtension()
    {
      return "cs.ast";
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
      testCasesDirCache = Path.Combine(testCasesDirCache, "ILtoAST");
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

