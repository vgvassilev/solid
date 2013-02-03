/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Diagnostics;
using System.IO;

using Mono.Cecil;

using NUnit.Framework;

using SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree;
using SolidOpt.Services.Transformations.Multimodel.ILtoAST;
using SolidOpt.Services.Transformations.Multimodel.Test;
using SolidOpt.Services.Transformations.Optimizations.AST.MethodInline;

namespace SolidOpt.Services.Transformations.Optimizations.AST.MethodInline.Test
{
  [TestFixture]
  public class MethodInlineTestFixture 
    : BaseTestFixture<AstMethodDefinition, AstMethodDefinition, InlineTransformer>
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

    protected override string GetTestCasesDir() {
      if (testCasesDirCache != null)
        return testCasesDirCache;

      testCasesDirCache = base.GetTestCasesDir();
      testCasesDirCache = Path.Combine(testCasesDirCache, "..");
      testCasesDirCache = Path.Combine(testCasesDirCache, "src");
      testCasesDirCache = Path.Combine(testCasesDirCache, "Services");
      testCasesDirCache = Path.Combine(testCasesDirCache, "Transformations");
      testCasesDirCache = Path.Combine(testCasesDirCache, "Optimizations");
      testCasesDirCache = Path.Combine(testCasesDirCache, "AST");
      testCasesDirCache = Path.Combine(testCasesDirCache, "MethodInline");
      testCasesDirCache = Path.Combine(testCasesDirCache, "test");
      testCasesDirCache = Path.Combine(testCasesDirCache, "TestCases");
      return testCasesDirCache;
    }

    private AstMethodDefinition getAstMethodDef(string testCaseName) {
      MethodDefinition mainMethodDef = LoadTestCaseMethod(testCaseName);
      return new ILtoASTTransformer().Decompile(mainMethodDef);
    }
    
    [Test, TestCaseSource("GetTestCases")] /*Comes from the base class*/
    public void Cases(string filename)
    {
      RunTestCase(filename, getAstMethodDef(filename));
    }
  }
}

