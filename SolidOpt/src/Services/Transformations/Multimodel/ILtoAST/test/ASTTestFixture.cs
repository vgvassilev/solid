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
    private readonly string testCasesDirCache = Path.Combine("src",
                                                             "Services",
                                                             "Transformations",
                                                             "Multimodel",
                                                             "ILtoAST",
                                                             "test",
                                                             "TestCases");
    
    public ASTTestFixture()
    {
      // Do not implement. NUnit uses reflection. Moreover the base class does things 
      // on static init.
    }

    protected override string GetTestCaseFileExtension()
    {
      return "cs";
    }
    
    protected override string GetTestCaseResultFileExtension()
    {
      return "cs.ast";
    }

    protected override string GetTestCaseDirOffset() 
    {
      return testCasesDirCache;
    }
    
    [Test, TestCaseSource("GetTestCases")] /*Comes from the base class*/
    public void Cases(string filename)
    {
      RunTestCase(filename, LoadTestCaseMethod(filename));
    }
  }
}

