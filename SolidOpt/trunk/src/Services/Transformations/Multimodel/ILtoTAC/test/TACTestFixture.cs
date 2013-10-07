/*
 * $Id: CFGTestFixture.cs 616 2012-09-03 19:49:39Z apenev $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Mono.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Cil;

using NUnit.Framework;

using SolidOpt.Services.Transformations.Multimodel.ILtoCFG;
using SolidOpt.Services.Transformations.Multimodel.ILtoTAC;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;
using SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode;

using SolidOpt.Services.Transformations.Multimodel.Test;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoTAC.Test
{
  [TestFixture]
  public sealed class TACTestFixture 
    : BaseTestFixture<MethodDefinition, ThreeAddressCode, ILtoTACTransformer>
  {
    private readonly string testCasesDirCache = Path.Combine("src",
                                                             "Services",
                                                             "Transformations",
                                                             "Multimodel",
                                                             "ILtoTAC",
                                                             "test",
                                                             "TestCases");

    public TACTestFixture()
    {
      // Do not implement. NUnit uses reflection. Moreover the base class does things 
      // on static init.
    }

    protected override string GetTestCaseFileExtension()
    {
      return "il";
    }

    protected override string GetTestCaseResultFileExtension()
    {
      return "il.tac";
    }

    protected override string GetTestCaseDirOffset() 
    {
      return testCasesDirCache;
    }

    [Test]
    public void CpblkAndInitblk() {
      string testCaseName = "CpblkAndInitblk";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName, "Init", "Copy"));
    }

    [Test]
    public void StInd_RefLdInd_Ref() {
      string testCaseName = "StInd_RefLdInd_Ref";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName, "ByRef"));
    }

    [Test]
    public void Generics_ReadOnlyPrefixAndConstrainedPrefix() {
      string testCaseName = "Generics_ReadOnlyPrefixAndConstrainedPrefix";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName, "ReadOnlyAndConstrained"));
    }

    [Test]
    public void StInd_LdInd() {
      string testCaseName = "StInd_LdInd";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName, "ByRef"));
    }

    [Test, TestCaseSource("GetTestCases")] /*Comes from the base class*/
    public void Cases(string filename)
    {
      RunTestCase(filename, LoadTestCaseMethod(filename));
    }
  }
}
