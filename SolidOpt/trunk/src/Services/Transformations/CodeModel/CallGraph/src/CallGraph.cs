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
    /// <summary>
    /// The depth of the call graph. 0 means unlimited.
    /// </summary>
    private int depth = 0  ;
    public int Depth {
      get { return this.depth; }
    }

    public CallGraph(CGNode root, int depth) {
      this.root = root;
      this.depth = depth;
    }

    public CallGraph(CGNode root) {
      this.root = root;
    }

    public override string ToString () {
      return Root.ToString();
    }
  }
}

