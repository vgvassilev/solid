// /*
//  * $Id$
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

using SolidOpt.Services.Multimodel.Test;

namespace SolidOpt.Services.Transformations.Multimodel.CFGtoTAC.Test
{
  [TestFixture]
	public class CFGTestFixture : BaseTestFixture {

    public override void RunTestCase(string testCaseName)
    {
      string testCaseFile = GetTestCaseFullPath(testCaseName);
      // Check whether the file exists first.
      Assert.IsTrue(File.Exists(testCaseFile),
                    String.Format("{0} does not exist.", testCaseName));
    
      string testCaseResultFile = GetTestCaseResultFullPath(testCaseName);
      // Check whether the result file exists first.
      Assert.IsTrue(File.Exists(testCaseResultFile),
                    String.Format("{0} does not exist.", testCaseResultFile));

      MethodDefinition mainMethodDef = LoadTestCaseMethod(testCaseName);
      ControlFlowGraph cfg = new CilToControlFlowGraph().Decompile(mainMethodDef);

      CFGtoTACTransformer transformCFG = new CFGtoTACTransformer();
      Triplet triplet = transformCFG.Decompile(cfg);

      string errMsg = String.Empty;
      string seen = triplet.ToString();
      string expected = File.ReadAllText(GetTestCaseResultFullPath(testCaseName));
      Assert.IsTrue(Validate(seen, expected, ref errMsg), errMsg);
    }

    protected override string GetTestCaseFileExtension()
    {
      return "il";
    }

    protected override string GetTestCaseResultFileExtension()
    {
      return "il.tac";
    }

		[Test]
    public void BoolAndGreaterOrEqualThan()
    {
      RunTestCase("BoolAndGreaterOrEqualThan");
    }

    [Test]
    public void SimpleExpression1()
    {
        RunTestCase("SimpleExpression1");
    }

    [Test]
    public void SimpleExpression2()
    {
        RunTestCase("SimpleExpression2");
    }

    [Test]
    public void SimpleExpressionIfThen()
    {
       RunTestCase("SimpleExpressionIfThen");
    }

    [Test]
    public void SimpleExpressionIfThenElse()
    {
        RunTestCase("SimpleExpressionIfThenElse");
    }

    [Test]
    public void LocalVariables()
    {
        RunTestCase("LocalVariables");
    }
    
    /*
    [Test]
    public void BoolOrLessOrEqualThan()
    {
      RunTestCase("BoolOrLessOrEqualThan");
    }

    [Test]
    public void DoWhile()
    {
      RunTestCase("DoWhile");
    }

    [Test]
    public void Empty()
    {
      RunTestCase("Empty");
    }

    [Test]
    public void FalseIf()
    {
      RunTestCase("FalseIf");
    }

    [Test]
    public void FieldAccessor()
    {
      RunTestCase("FieldAccessor");
    }

    [Test]
    public void FloatEquals()
    {
      RunTestCase("FloatEquals");
    }

    [Test]
    public void FloatGreaterThan()
    {
      RunTestCase("FloatGreaterThan");
    }

    [Test]
    public void FlowTest()
    {
      RunTestCase("FlowTest");
    }

    [Test]
    public void GreaterThanOrEqual()
    {
      RunTestCase("GreaterThanOrEqual");
    }

    [Test]
    public void IfNestedCondition()
    {
      RunTestCase("IfNestedCondition");
    }

    [Test]
    public void InPlaceAdd()
    {
      RunTestCase("InPlaceAdd");
    }

    [Test]
    public void InRange()
    {
      RunTestCase("InRange");
    }

    [Test]
    public void IntPropertyEquals1()
    {
      RunTestCase("IntPropertyEquals1");
    }

    [Test]
    public void IntPropertyEquals2()
    {
      RunTestCase("IntPropertyEquals2");
    }

    [Test]
    public void IsNull()
    {
      RunTestCase("IsNull");
    }

    [Test]
    public void LessThanOrEqual()
    {
      RunTestCase("LessThanOrEqual");
    }

    [Test]
    public void MathOperators()
    {
      RunTestCase("MathOperators");
    }

    [Test]
    public void MixedAndOr()
    {
      RunTestCase("MixedAndOr");
    }

    [Test]
    public void MultipleAndOr()
    {
      RunTestCase("MultipleAndOr");
    }

    [Test]
    public void MultipleOr()
    {
      RunTestCase("MultipleOr");
    }

    [Test]
    public void NestedOrGreaterThan()
    {
      RunTestCase("NestedOrGreaterThan");
    }

    [Test]
    public void NotEqual()
    {
      RunTestCase("NotEqual");
    }

    [Test]
    public void NotStringEquality()
    {
      RunTestCase("NotStringEquality");
    }

    [Test]
    public void OptimizedAnd()
    {
      RunTestCase("OptimizedAnd");
    }

    [Test]
    public void OptimizedNestedOr()
    {
      RunTestCase("OptimizedNestedOr");
    }

    [Test]
    public void OptimizedOr()
    {
      RunTestCase("OptimizedOr");
    }

    [Test]
    public void PropertyPredicate()
    {
      RunTestCase("PropertyPredicate");
    }

    [Test]
    public void SideEffectExpression()
    {
      RunTestCase("SideEffectExpression");
    }

    [Test]
    public void SimpleCalculation()
    {
      RunTestCase("SimpleCalculation");
    }

    [Test]
    public void SimpleCondition()
    {
      RunTestCase("SimpleCondition");
    }

    [Test]
    public void SimpleIf()
    {
      RunTestCase("SimpleIf");
    }

    [Test]
    public void SimpleReturn()
    {
      RunTestCase("SimpleReturn");
    }

    [Test]
    public void SimpleSwitch()
    {
      RunTestCase("SimpleSwitch");
    }

    [Test]
    public void SimpleWhile()
    {
      RunTestCase("SimpleWhile");
    }

    [Test]
    public void SingleAnd()
    {
      RunTestCase("SingleAnd");
    }

    [Test]
    public void SingleOr()
    {
      RunTestCase("SingleOr");
    }

    [Test]
    public void StaticField()
    {
      RunTestCase("StaticField");
    }

    [Test]
    public void StringCast()
    {
      RunTestCase("StringCast");
    }

    [Test]
    public void StringPredicate()
    {
      RunTestCase("StringPredicate");
    }

    [Test]
    public void StringTryCast()
    {
      RunTestCase("StringTryCast");
    }

    [Test]
    public void Switch()
    {
      RunTestCase("Switch");
    }

    [Test]
    public void TernaryExpression()
    {
      RunTestCase("TernaryExpression");
    }

    [Test]
    public void ThreeReturns()
    {
      RunTestCase("ThreeReturns");
    }

    [Test]
    public void TwoIfs() {
      RunTestCase("TwoIfs");
    }

    [Test]
    public void ConditionalBranchToNext() {
      RunTestCase("ConditionalBranchToNext");
    }

    [Test]
    public void InfiniteLoopBranchToSelf() {
      RunTestCase("InfiniteLoopBranchToSelf");
    }

    [Test]
    public void ThrowNewException() {
      RunTestCase("ThrowNewException");
    }

    [Test]
    public void TryCatchException() {
      RunTestCase("TryCatchException");
    }

    [Test]
    public void TryCatchFinallyException() {
      RunTestCase("TryCatchFinallyException");
    }

    [Test]
    public void TryMultipleCatchException() {
      RunTestCase("TryMultipleCatchException");
    }

    [Test]
    public void NestedTryCatchException() {
      RunTestCase("NestedTryCatchException");
    }

    [Test]
    public void NestedTryCatchFinallyException() {
      RunTestCase("NestedTryCatchFinallyException");
    }

    [Test]
    public void CatchFaultException() {
      RunTestCase("CatchFaultException");
    }

    [Test]
    public void CatchFilterException() {
      RunTestCase("CatchFilterException");
    }

    [Test]
    public void CatchTwoFiltersException() {
      RunTestCase("CatchTwoFiltersException");
    }

    [Test]
    public void TryFinallyException() {
      RunTestCase("TryFinallyException");
    }

    [Test]
    public void TryFinallyExceptionWithNops() {
      RunTestCase("TryFinallyExceptionWithNops");
    }

    [Test]
    public void TryFinallyExceptionFinallyAtEnd() {
      RunTestCase("TryFinallyExceptionFinallyAtEnd");
    }
    */
	}
}
