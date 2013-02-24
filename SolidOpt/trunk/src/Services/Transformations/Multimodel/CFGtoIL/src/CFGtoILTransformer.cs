/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;

using SolidOpt.Services.Transformations.Multimodel;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

using Mono.Cecil;
using Mono.Cecil.Cil;

/// <summary>
/// The namespace contains classes helping the multimodel transformation from control flow graph
/// into intermediate language.
/// </summary>
namespace SolidOpt.Services.Transformations.Multimodel.CFGtoIL
{
  /// <summary>
  /// Control flow graph to CIL transforms a given control flow graph into a method definition.
  /// </summary>
  public class ControlFlowGraphToCil : ITransform<ControlFlowGraph, MethodDefinition>
  {

    #region ITransform implementation
    public MethodDefinition Transform (ControlFlowGraph source)
    {
      var result = CloneMethodWithoutIL(source.Method);
      result.Body = new MethodBody(result);
      ILProcessor cil = result.Body.GetILProcessor();
      BasicBlock bb = source.Root;
      Instruction instr = null;
      List<BasicBlock> notVisited = new List<BasicBlock>();
      List<BasicBlock> visited = new List<BasicBlock>();
      notVisited.Add(bb);
      while(bb != null) {
        instr = bb.First;
        while (instr != bb.Last.Next) {
          cil.Append(instr);
          instr = instr.Next;
        }
        notVisited.Remove(bb);
        visited.Add(bb);
        bb.Successors.Sort(new BBComparer());
        notVisited.AddRange(bb.Successors);
        GetExclusiveSet(ref notVisited, visited);
        bb = (notVisited.Count > 0) ? notVisited[0] : null;
      }
      return result;
    }

    #endregion

    private static MethodDefinition CloneMethodWithoutIL(MethodDefinition method)
    {
      var df = new MethodDefinition(method.Name, method.Attributes, method.ReturnType);
      //df.Body = new Mono.Cecil.Cil.MethodBody(df);
      //df.Attributes = method.Attributes;

      /// Attributes
      //public bool IsCompilerControlled {
      //public bool IsPrivate {
      //public bool IsFamilyAndAssembly {
      //public bool IsAssembly {
      //public bool IsFamily {
      //public bool IsFamilyOrAssembly {
      //public bool IsPublic {
      //public bool IsStatic {
      //public bool IsFinal {
      //public bool IsVirtual {
      //public bool IsHideBySig {
      //public bool IsReuseSlot {
      //public bool IsNewSlot {
      //public bool IsCheckAccessOnOverride {
      //public bool IsAbstract {
      //public bool IsSpecialName {
      //public bool IsPInvokeImpl {
      //public bool IsUnmanagedExport {
      //public bool IsRuntimeSpecialName {
      //public bool HasSecurity {
      /// MethodImplAttributes
      //public bool IsIL {
      //public bool IsNative {
      //public bool IsRuntime {
      //public bool IsUnmanaged {
      //public bool IsManaged {
      //public bool IsForwardRef {
      //public bool IsPreserveSig {
      //public bool IsInternalCall {
      //public bool IsSynchronized {
      //public bool NoInlining {
      //public bool NoOptimization {
      /// MethodSemanticsAttributes
      //public bool IsSetter {
      //public bool IsGetter {
      //public bool IsOther {
      //public bool IsAddOn {
      //public bool IsRemoveOn {
      //public bool IsFire {
      
      df.CallingConvention = method.CallingConvention;
      foreach (var cattr in method.CustomAttributes)
        df.CustomAttributes.Add(cattr);
      df.DeclaringType = method.DeclaringType;
      df.ExplicitThis = method.ExplicitThis;
      foreach (var gparam in method.GenericParameters)
        df.GenericParameters.Add(gparam);
      //df.HasSecurity
      df.HasThis = method.HasThis;
      df.ImplAttributes = method.ImplAttributes;
      //df.Module = method.Module;
      foreach (var param in method.Parameters) {
        ParameterDefinition p = new ParameterDefinition(param.Name, param.Attributes, param.ParameterType);
        if (param.HasConstant) p.Constant = param.Constant;
        foreach (var pcattr in param.CustomAttributes)
          p.CustomAttributes.Add(pcattr);
        //p.HasConstant = param.HasConstant;
        //p.HasDefault = param.HasDefault;
        //p.IsIn = param.IsIn;
        //p.IsOptional = param.IsOptional;
        //p.IsOut = param.IsOut;
        p.MarshalInfo = param.MarshalInfo;
        p.Name = param.Name;
        df.Parameters.Add(p);
      }
      if (method.IsPInvokeImpl) df.PInvokeInfo = method.PInvokeInfo;
      df.SemanticsAttributes = method.SemanticsAttributes;
      
      df.Body = new Mono.Cecil.Cil.MethodBody(df);
      
      return df;
    }

    private void GetExclusiveSet(ref List<BasicBlock> excludeFrom, List<BasicBlock> excludes) {
      foreach (BasicBlock bb in excludes)
        excludeFrom.Remove(bb);
    }
  }
  
  internal sealed class BBComparer : IComparer<BasicBlock> {
    public int Compare(BasicBlock x, BasicBlock y) {
      return x.First.Offset.CompareTo(y.First.Offset);
    }
  }
}
