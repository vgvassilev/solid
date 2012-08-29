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
    /// The caller. Null if root.
    /// </summary>
    private CGNode caller = null;
    public CGNode Caller {
      get { return this.caller; }
    }

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

    public CGNode(MethodDefinition method, CGNode caller) {
      this.method = method;
      this.caller = caller;
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();

      CGNode parent = Caller;
      do {
        sb.Append("   ");
      }
      while(parent != null && (parent = parent.Caller) != null);
        sb.Append("+--");
      sb.AppendLine(method.ToString());

      foreach (CGNode node in methodCalls)
        sb.Append(node.ToString());

      return sb.ToString();
    }
  }
}

