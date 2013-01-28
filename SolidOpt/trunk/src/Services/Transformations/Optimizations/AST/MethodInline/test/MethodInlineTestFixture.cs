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
    protected override string GetTestCaseFileExtension()
    {
      return "cs";
    }
    
    protected override string GetTestCaseResultFileExtension()
    {
      return "cs.ast";
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

