/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph
{
  /// <summary>
  /// Indicates the type of the block, whether it is structural or exceptional.
  /// </summary>
  public enum BlockKind {Structure, SEH};

  /// <summary>
  /// Dragon Book [8.4.1] Basic Blocks:
  /// Basic block starts with the first instruction and keeps adding 
  /// instructions until we meet either a jump, a conditional jump, or a 
  /// label on the following instruction. In the absence of jumps and 
  /// labels, control proceeds sequentially from one instruction to the next.
  /// </summary>
  public class BasicBlock<T> : IEnumerable<T>
  {
    public BlockKind Kind;
    private List<T> body = new List<T>();
    private string name;
    public string Name {
      get { return this.name;  }
      set { name = value;  }
    }
    
    /// <summary>
    /// Gets the last instruction, which is basic block terminator.
    /// </summary>
    /// <value>
    /// The last instruction of the given basic block.
    /// </value>
    public T Last {
      get { return body[body.Count - 1]; }
    }
    
    /// <summary>
    /// Gets the first instruction, which is basic block leader.
    /// </summary>
    /// <value>
    /// The first instruction of the given block or null if not set.
    /// </value>
    public T First {
      get { 
        if (body.Count > 0)
          return body[0];
        else
          return default(T);
      }
    }
    
    /// <summary>
    /// The successors a basic block may have.
    /// </summary>
    private List<BasicBlock<T>> successors = new List<BasicBlock<T>>();
    public List<BasicBlock<T>> Successors {
      get { return this.successors; }
      set { successors = value; }
    }
    
    /// <summary>
    /// The predecessors a basic block may have.
    /// </summary>
    private List<BasicBlock<T>> predecessors = new List<BasicBlock<T>>();
    public List<BasicBlock<T>> Predecessors {
      get { return this.predecessors;  }
      set { predecessors = value;  }
    }

    public BasicBlock(string name)
    {
      this.name = name;
    }
    
    public BasicBlock(List<BasicBlock<T>> successors, List<BasicBlock<T>> predecessors)
    {
      this.successors = successors;
      this.predecessors = predecessors;
    }

    public void Add(T i)
    {
      //TODO: enforce checking whether we are adding non linear instr
      body.Add(i);
    }
    
    public bool Contains(T i)
    {
      return body.Contains(i);  
    }
    
    #region IEnumerable
    
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      return body.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      yield return this;
    }

    #endregion

    public override string ToString() {
      StringBuilder sb = new StringBuilder();

      if (body.Count >= 4) {
        sb.AppendLine (body [0].ToString ());
        sb.AppendLine (body [1].ToString ());
        sb.AppendLine ("...");
        sb.AppendLine (body [body.Count - 2].ToString ());
        sb.AppendLine (body [body.Count - 1].ToString ());
      } else if ((body.Count > 1) && (body.Count < 4)) {
        sb.AppendLine (body [0].ToString ());
        sb.AppendLine ("...");
        sb.AppendLine (body [body.Count - 1].ToString ());
      } else if (body.Count == 1) {
        sb.AppendLine (body [0].ToString ());
      }

        // foreach (T instr in body) {
        //   sb.AppendLine(instr.ToString());
        // }

        return sb.ToString();
    }
  }
}
