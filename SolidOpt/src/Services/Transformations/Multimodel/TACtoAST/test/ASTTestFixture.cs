/*
 * $Id: CFGTestFixture.cs 616 2012-09-03 19:49:39Z apenev $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Text;


using NUnit.Framework;

using SolidOpt.Services.Transformations.Multimodel.TACtoAST;
using SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree;
using SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode;

using SolidOpt.Services.Transformations.Multimodel.Test;

namespace SolidOpt.Services.Transformations.Multimodel.TACtoAST.Test
{
  [TestFixture]
  public sealed class ASTTestFixture 
    : BaseTestFixture<ThreeAddressCode, AstMethodDefinition, TACtoASTTransformer>
  {
    private readonly string testCasesResultDirCache = Path.Combine("src",
                                                             "Services",
                                                             "Transformations",
                                                             "Multimodel",
                                                             "TACtoAST",
                                                             "test",
                                                             "TestCases");
    private readonly string testCasesDirCache = Path.Combine("src",
      "Services",
      "Transformations",
      "Multimodel",
      "ILtoTAC",
      "test",
      "TestCases");

    public ASTTestFixture()
    {
      // Do not implement. NUnit uses reflection. Moreover the base class does things 
      // on static init.
    }

    protected override string GetTestCaseFileExtension()
    {
      return "tac";
    }

    protected override string GetTestCaseResultFileExtension()
    {
      return "tac.ast";
    }

    protected override string GetTestCaseDirOffset() 
    {
      return testCasesDirCache;
    }

    /// <summary>
    /// Reimplement to reuse the tests in ILtoTAC transformer.
    /// </summary>
    /// <returns>The test case result dir offset.</returns>
    protected override string GetTestCaseResultDirOffset()
    {
      return testCasesResultDirCache;
    }

    [Test, TestCaseSource("GetTestCases")] /*Comes from the base class*/
    public void Cases(string filename)
    {
      RunTestCase(filename, LoadTestCaseMethodTAC(filename));
    }

    public ThreeAddressCode[] LoadTestCaseMethodTAC(string filename) {
      StringBuilder errors = new StringBuilder();
      ThreeAddressCode[] tac = {ThreeAddressCode.FromString(File.ReadAllText(filename), ref errors)};
      Assert.IsTrue(errors.Length == 0, errors.ToString());

      return tac;
    }
  }
}
