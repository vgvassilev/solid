/*
 * $Id$
 * * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;
using System.Text;

using Mono.Cecil;

namespace SolidOpt.Services.Transformations.CodeModel.CallGraph
{

  /// <summary>
  /// Represents a single node in a call graph.
  /// </summary>
  public class CGNode
  {

    /// <summary>
    /// The method which is represented by the node.
    /// </summary>
    private MethodDefinition method;
    public MethodDefinition Method {
      get { return this.method; }
      set { method = value; }
    }

    /// <summary>
    /// Methods that this method can possibly call.
    /// </summary>
    private List<CGNode> methodCalls = new List<CGNode>();
    public List<CGNode> MethodCalls {
      get { return this.methodCalls; }
      set { methodCalls = value; }
    }

    public CGNode(MethodDefinition method) {
      this.method = method;
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine(method.ToString());
      sb.Append("+--");
      foreach (CGNode node in methodCalls) {
        sb.Append(node.ToString());
      }
      return sb.ToString();
    }
  }
}

