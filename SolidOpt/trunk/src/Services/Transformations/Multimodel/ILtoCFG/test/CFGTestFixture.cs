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
  public class CFGTestFixture : BaseTestFixture<MethodDefinition, ControlFlowGraph, CilToControlFlowGraph> {

    protected override string GetTestCaseFileExtension()
    {
      return "il";
    }

    protected override string GetTestCaseResultFileExtension()
    {
      return "il.cfg";
    }

    [Test, TestCaseSource("GetTestCases")] /*Comes from the base class*/
    public void Cases(string filename)
    {
      RunTestCase(filename, LoadTestCaseMethod(filename));
    }
  }
}
