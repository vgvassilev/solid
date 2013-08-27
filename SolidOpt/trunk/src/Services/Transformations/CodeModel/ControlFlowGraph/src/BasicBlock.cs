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
  public class BasicBlock : IEnumerable<Instruction>
  {
    public BlockKind Kind;
    private List<Instruction> body = new List<Instruction>();
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
    public Instruction Last {
      get { return body[body.Count - 1]; }
    }
    
    /// <summary>
    /// Gets the first instruction, which is basic block leader.
    /// </summary>
    /// <value>
    /// The first instruction of the given block or null if not set.
    /// </value>
    public Instruction First {
      get { 
        if (body.Count > 0)
          return body[0];
        else
          return null;
      }
    }
    
    /// <summary>
    /// The successors a basic block may have.
    /// </summary>
    private List<BasicBlock> successors = new List<BasicBlock>();
    public List<BasicBlock> Successors {
      get { return this.successors; }
      set { successors = value; }
    }
    
    /// <summary>
    /// The predecessors a basic block may have.
    /// </summary>
    private List<BasicBlock> predecessors = new List<BasicBlock>();
    public List<BasicBlock> Predecessors {
      get { return this.predecessors;  }
      set { predecessors = value;  }
    }

    public BasicBlock(string name)
    {
      this.name = name;
    }
    
    public BasicBlock(List<BasicBlock> successors, List<BasicBlock> predecessors)
    {
      this.successors = successors;
      this.predecessors = predecessors;
    }

    public void Add(Instruction i)
    {
      //TODO: enforce checking whether we are adding non linear instr
      body.Add(i);
    }
    
    public bool Contains(Instruction i)
    {
      return body.Contains(i);  
    }
    
    #region IEnumerable
    
    IEnumerator<Instruction> IEnumerable<Instruction>.GetEnumerator()
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

      foreach (Instruction instr in body) {
        sb.AppendLine(instr.ToString());
      }

      return sb.ToString();
    }
  }
}

