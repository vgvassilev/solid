// /*
//  * $Id: CFGTestFixture.cs 616 2012-09-03 19:49:39Z apenev $
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */

using System;
using Mono.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Cil;

using NUnit.Framework;

using SolidOpt.Services.Transformations.Multimodel.ILtoCFG;
using SolidOpt.Services.Transformations.Multimodel.CFGtoTAC;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;
using SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode;

using SolidOpt.Services.Transformations.Multimodel.Test;

namespace SolidOpt.Services.Transformations.Multimodel.CFGtoTAC.Test
{
  [TestFixture]
  public class TACTestFixture : BaseTestFixture<ControlFlowGraph, ThreeAdressCode, CFGtoTACTransformer> {

    protected override string GetTestCaseFileExtension()
    {
      return "il";
    }

    protected override string GetTestCaseResultFileExtension()
    {
      return "il.tac";
    }

    private ControlFlowGraph getCfg(string testCaseName) {
      MethodDefinition mainMethodDef = LoadTestCaseMethod(testCaseName);
      return new CilToControlFlowGraph().Decompile(mainMethodDef);
    }

    [Test, TestCaseSource("GetTestCases")] /*Comes from the base class*/
    public void Cases(string filename)
    {
      RunTestCase(filename, getCfg(filename));
    }
  }
}
