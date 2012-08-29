/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

using SolidOpt.Services.Transformations.CodeModel.CallGraph;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoCG
{
  public class CallGraphBuilder
  {
    private readonly MethodDefinition rootMethod;
    private List<MethodDefinition> rawDefs = new List<MethodDefinition>();

    #region Constructors

    internal CallGraphBuilder(MethodDefinition rootMethod)
    {
      this.rootMethod = rootMethod;
    }

    public CallGraph Create()
    {
      CGNode rootNode = new CGNode(rootMethod, null);
      VisitCGNode(rootNode);
      return new CallGraph(rootNode);
    }

    #endregion

    private void VisitCGNode(CGNode node) {
      if (node.Method.HasBody) {
        MethodReference mRef;
        foreach (Instruction instr in node.Method.Body.Instructions) {
          if (instr.OpCode.FlowControl == FlowControl.Call) {
            mRef = (instr.Operand as MethodReference);
            CGNode callee = new CGNode(mRef.Resolve(), node);
            node.MethodCalls.Add(callee);
            if (!rawDefs.Contains(callee.Method)) {
              rawDefs.Add(node.Method);
              VisitCGNode(callee);
            }
          }
        }
      }
    }
  }
}

