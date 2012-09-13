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

		[Test]
    public void BoolAndGreaterOrEqualThan()
    {
      string testCaseName = "BoolAndGreaterOrEqualThan";
      RunTestCase(testCaseName, getCfg(testCaseName));
    }

    [Test]
    public void SimpleExpression1()
    {
      string testCaseName = "SimpleExpression1";
      RunTestCase(testCaseName, getCfg(testCaseName));
    }

    [Test]
    public void SimpleExpression2()
    {
      string testCaseName = "SimpleExpression2";
      RunTestCase(testCaseName, getCfg(testCaseName));
    }

    [Test]
    public void SimpleExpressionIfThen()
    {
      string testCaseName = "SimpleExpressionIfThen";
      RunTestCase(testCaseName, getCfg(testCaseName));
    }

    [Test]
    public void SimpleExpressionIfThenElse()
    {
      string testCaseName = "SimpleExpressionIfThenElse";
      RunTestCase(testCaseName, getCfg(testCaseName));
    }
    
    [Test]
    public void LocalVariables()
    {
        string testCaseName = "LocalVariables";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }
    
    [Test]
    public void SimpleWhile()
    {
        string testCaseName = "SimpleWhile";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }
    
    [Test]
    public void SimpleDoWhile()
    {
        string testCaseName = "SimpleDoWhile";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }
    
    [Test]
    public void SimpleExpressionReminder()
    {
        string testCaseName = "SimpleExpressionReminder";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }
    
    [Test]
    public void SimpleSwitch()
    {
        string testCaseName = "SimpleSwitch";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }

    [Test]
    public void Nop()
    {
        string testCaseName = "Nop";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }

    [Test]
    public void SwitchDoWhile()
    {
        string testCaseName = "SwitchDoWhile";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }

    [Test]
    public void PreIncrement()
    {
        string testCaseName = "PreIncrement";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }

    [Test]
    public void PostIncrement()
    {
        string testCaseName = "PostIncrement";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }

    [Test]
    public void ILDup1()
    {
        string testCaseName = "ILDup1";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }
    
    [Test]
    public void ILDup2()
    {
        string testCaseName = "ILDup2";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }
    
    [Test]
    public void ILLoadStore()
    {
        string testCaseName = "ILLoadStore";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }
    
    [Test]
    public void CallStaticSkipResult()
    {
        string testCaseName = "CallStaticSkipResult";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }
    
    [Test]
    public void Call()
    {
        string testCaseName = "Call";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }

    [Test]
    public void Jmp()
    {
        string testCaseName = "Jmp";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }

    [Test]
    public void Cast()
    {
        string testCaseName = "Cast";
        RunTestCase(testCaseName, getCfg(testCaseName));
    }
    
  }
}
