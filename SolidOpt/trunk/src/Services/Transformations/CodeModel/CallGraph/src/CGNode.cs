/*
 * $Id: $
 * * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

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
    private List<MethodReference> methodReferences = new List<MethodReference>();
    public List<MethodReference> MethodReferences {
      get { return this.methodReferences; }
      set { methodReferences = value; }
    }

    public CGNode(MethodDefinition method) {
      this.method = method;
    }
  }
}

