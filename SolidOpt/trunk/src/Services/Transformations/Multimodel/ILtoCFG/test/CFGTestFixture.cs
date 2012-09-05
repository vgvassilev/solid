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

		[Test]
    public void BoolAndGreaterOrEqualThan()
    {
      string testCaseName = "BoolAndGreaterOrEqualThan";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void BoolOrLessOrEqualThan()
    {
      string testCaseName = "BoolOrLessOrEqualThan";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void DoWhile()
    {
      string testCaseName = "DoWhile";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void Empty()
    {
      string testCaseName = "Empty";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void FalseIf()
    {
      string testCaseName = "FalseIf";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void FieldAccessor()
    {
      string testCaseName = "FieldAccessor";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void FloatEquals()
    {
      string testCaseName = "FloatEquals";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void FloatGreaterThan()
    {
      string testCaseName = "FloatGreaterThan";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void FlowTest()
    {
      string testCaseName = "FlowTest";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void GreaterThanOrEqual()
    {
      string testCaseName = "GreaterThanOrEqual";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void IfNestedCondition()
    {
      string testCaseName = "IfNestedCondition";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void InPlaceAdd()
    {
      string testCaseName = "InPlaceAdd";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void InRange()
    {
      string testCaseName = "InRange";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void IntPropertyEquals1()
    {
      string testCaseName = "IntPropertyEquals1";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void IntPropertyEquals2()
    {
      string testCaseName = "IntPropertyEquals2";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void IsNull()
    {
      string testCaseName = "IsNull";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void LessThanOrEqual()
    {
      string testCaseName = "LessThanOrEqual";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void MathOperators()
    {
      string testCaseName = "MathOperators";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void MixedAndOr()
    {
      string testCaseName = "MixedAndOr";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void MultipleAndOr()
    {
      string testCaseName = "MultipleAndOr";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void MultipleOr()
    {
      string testCaseName = "MultipleOr";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void NestedOrGreaterThan()
    {
      string testCaseName = "NestedOrGreaterThan";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void NotEqual()
    {
      string testCaseName = "NotEqual";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void NotStringEquality()
    {
      string testCaseName = "NotStringEquality";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void OptimizedAnd()
    {
      string testCaseName = "OptimizedAnd";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void OptimizedNestedOr()
    {
      string testCaseName = "OptimizedNestedOr";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void OptimizedOr()
    {
      string testCaseName = "OptimizedOr";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void PropertyPredicate()
    {
      string testCaseName = "PropertyPredicate";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void SideEffectExpression()
    {
      string testCaseName = "SideEffectExpression";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void SimpleCalculation()
    {
      string testCaseName = "SimpleCalculation";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void SimpleCondition()
    {
      string testCaseName = "SimpleCondition";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void SimpleIf()
    {
      string testCaseName = "SimpleIf";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void SimpleReturn()
    {
      string testCaseName = "SimpleReturn";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void SimpleSwitch()
    {
      string testCaseName = "SimpleSwitch";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void SimpleWhile()
    {
      string testCaseName = "SimpleWhile";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void SingleAnd()
    {
      string testCaseName = "SingleAnd";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void SingleOr()
    {
      string testCaseName = "SingleOr";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void StaticField()
    {
      string testCaseName = "StaticField";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void StringCast()
    {
      string testCaseName = "StringCast";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void StringPredicate()
    {
      string testCaseName = "StringPredicate";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void StringTryCast()
    {
      string testCaseName = "StringTryCast";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void Switch()
    {
      string testCaseName = "Switch";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void TernaryExpression()
    {
      string testCaseName = "TernaryExpression";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void ThreeReturns()
    {
      string testCaseName = "ThreeReturns";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void TwoIfs() {
      string testCaseName = "TwoIfs";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void ConditionalBranchToNext() {
      string testCaseName = "ConditionalBranchToNext";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void InfiniteLoopBranchToSelf() {
      string testCaseName = "InfiniteLoopBranchToSelf";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void ThrowNewException() {
      string testCaseName = "ThrowNewException";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void TryCatchException() {
      string testCaseName = "TryCatchException";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void TryCatchFinallyException() {
      string testCaseName = "TryCatchFinallyException";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void TryMultipleCatchException() {
      string testCaseName = "TryMultipleCatchException";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void NestedTryCatchException() {
      string testCaseName = "NestedTryCatchException";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void NestedTryCatchFinallyException() {
      string testCaseName = "NestedTryCatchFinallyException";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void CatchFaultException() {
      string testCaseName = "CatchFaultException";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void CatchFilterException() {
      string testCaseName = "CatchFilterException";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void CatchTwoFiltersException() {
      string testCaseName = "CatchTwoFiltersException";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void TryFinallyException() {
      string testCaseName = "TryFinallyException";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void TryFinallyExceptionWithNops() {
      string testCaseName = "TryFinallyExceptionWithNops";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }

    [Test]
    public void TryFinallyExceptionFinallyAtEnd() {
      string testCaseName = "TryFinallyExceptionFinallyAtEnd";
      RunTestCase(testCaseName, LoadTestCaseMethod(testCaseName));
    }
	}
}
