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
  public class ASTTestFixture : BaseTestFixture<MethodDefinition, AstMethodDefinition, ILtoASTTransformer>
  {
    protected override string GetTestCaseFileExtension()
    {
      return "cs";
    }
    
    protected override string GetTestCaseResultFileExtension()
    {
      return "cs.ast";
    }
    
    [Test, TestCaseSource("GetTestCases")] /*Comes from the base class*/
    public void Cases(string filename)
    {
      RunTestCase(filename, LoadTestCaseMethod(filename));
    }
  }
}

