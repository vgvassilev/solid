/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidOpt.Services.Transformations.CodeModel.CallGraph
{
  /// <summary>
  /// Structure that represents method calls as graph. Each method is represented as a node and
  /// each method call as edge from caller to the callee.
  /// </summary>
  public class CallGraph
  {
    private CGNode root;
    public CGNode Root {
      get { return this.root; }
    }

    public CallGraph(CGNode root) {
      this.root = root;
    }

    public override string ToString () {
      return Root.ToString();
    }
  }
}

