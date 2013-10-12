/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Mono.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoCFG
{

  /// <summary>
  /// This adapter class is used by the control flow graph builder to build CFG
  /// out of instructions that don't implement ILinearInstruction interface. 
  /// For example Mono.Cecil.Cil.Instruction.
  /// </summary>
  internal static class LinearInstructionAdapter<T> where T : class {
    public static T GetPrevious(T instruction) {
      Instruction cilInst = instruction as Instruction;
      if (cilInst != null)
        return cilInst.Previous as T;
      ILinearInstruction linearInst = instruction as ILinearInstruction;
      if (linearInst != null)
        return linearInst.GetPrevious() as T;
      throw new NotSupportedException(string.Format("Unsupported adaptee"));
    }

    public static T GetNext(T instruction) {
      Instruction cilInst = instruction as Instruction;
      if (cilInst != null)
        return cilInst.Next as T;
      ILinearInstruction linearInst = instruction as ILinearInstruction;
      if (linearInst != null)
        return linearInst.GetNext() as T;
      throw new NotSupportedException(string.Format("Unsupported adaptee"));
    }

    public static object GetOperand(T instruction) {
      Instruction cilInst = instruction as Instruction;
      if (cilInst != null)
        return cilInst.Operand;
      ILinearInstruction linearInst = instruction as ILinearInstruction;
      if (linearInst != null)
        return linearInst.Operand;
      throw new NotSupportedException(string.Format("Unsupported adaptee"));
    }

    public static FlowControl GetFlowControl(T instruction) {
      Instruction cilInst = instruction as Instruction;
      if (cilInst != null)
        return cilInst.OpCode.FlowControl;
      ILinearInstruction linearInst = instruction as ILinearInstruction;
      if (linearInst != null)
        return linearInst.FlowControl;
      throw new NotSupportedException(string.Format("Unsupported adaptee"));
    }
  }

  /// <summary>
  /// Control flow graph builder. Builds 
  /// <seealso cref="SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph"/>
  /// ControlFlowGraph of a method body given set of instructions.
  /// </summary>
  /// <description>
  /// Dragon Book [8.4.1]
  /// First, we determine those instructions in the intermediate code that 
  /// are leaders, that is, the first instructions in some basic block. The
  /// instruction just past the end of the intermediate program is not 
  /// included as a leader. The rules for finding leaders are:
  /// 1. The first three-address instruction in the intermediate code is a 
  /// leader.
  /// 2. Any instruction that is the target of a conditional or unconditional
  /// jump is a leader.
  /// 3. Any instruction that immediately follows a conditional or 
  /// unconditional jump is a leader.
  /// Then, for each leader, its basic block consists of itself and all 
  /// instructions up to but not including the next leader or the end of the
  /// intermediate program
  /// </description>
  public class ControlFlowGraphBuilder<T> where T : class {

    #region Fields & Properties
    private IEnumerable<T> instructions;
    private BasicBlock<T> root = null;
    private HashSet<T> labels;
    private List<BasicBlock<T>> rawBlocks = new List<BasicBlock<T>>();
    //TODO: HashSet<> is .net 4.0 class. May be we need use some 2.0 class (Dictionary<,>) or bool array?
    private HashSet<T> exceptionHandlersStarts = new HashSet<T>();
    private HashSet<T> exceptionHandlersEnds = new HashSet<T>();
    private MethodDefinition methodDefinition;

    #endregion
    
    #region Constructors
    
    public ControlFlowGraphBuilder(MethodDefinition methodDefinition,
                                   IEnumerable<T> instructions, 
                                   IEnumerable<T> ehStarts,
                                   IEnumerable<T> ehEnds) {
      this.methodDefinition = methodDefinition;
      this.instructions = instructions;
      this.exceptionHandlersStarts = new HashSet<T>(ehStarts);
      this.exceptionHandlersEnds = new HashSet<T>(ehEnds);
    }

    public ControlFlowGraph<T> Create()
    {
      CreateBlocks();
      ConnectBlocks();

      return new ControlFlowGraph<T>(root, rawBlocks, methodDefinition);
    }
    
    #endregion

    void CreateBlocks()
    {
      BasicBlock<T> curBlock = null;
      foreach(T instr in instructions) {
        if (IsBlockLeader(instr))
          curBlock = new BasicBlock<T>(rawBlocks.Count.ToString());
        
        if (root == null)
          root = curBlock;
        
        curBlock.Add(instr);

        if (IsBlockTerminator(instr)) {
          if (exceptionHandlersEnds.Contains(instr) || exceptionHandlersStarts.Contains(curBlock.First))
            curBlock.Kind =  BlockKind.SEH;
          else
            curBlock.Kind =  BlockKind.Structure;
          rawBlocks.Add(curBlock);
        }
      }
    }

    bool IsBlockLeader(T i)
    {
      if (LinearInstructionAdapter<T>.GetPrevious(i) == null)
        return true;

      // Check whether this was a start of an exception protected area
      if (exceptionHandlersStarts.Contains(i))
        return true;
      
      if (IsBlockTerminator(LinearInstructionAdapter<T>.GetPrevious(i)))
        return true;
  
      // Check whether the instruction has label
      if (HasLabel(i))
        return true;

      return false;
    }
    
    bool IsBlockTerminator(T i) {
      // first instruction in the collection starts a block
      switch (LinearInstructionAdapter<T>.GetFlowControl(i)) {
        // Ensures leave and endfinally do not create new block in structure CFG
        case FlowControl.Branch:
        case FlowControl.Return:
        case FlowControl.Break:
        case FlowControl.Cond_Branch:
        case FlowControl.Throw:
          return true;
      }

      T nextInstr = LinearInstructionAdapter<T>.GetNext(i);
      if (nextInstr != null)
        return HasLabel(nextInstr) || exceptionHandlersStarts.Contains(nextInstr) || exceptionHandlersEnds.Contains(nextInstr);

      return false;
    }

    bool HasLabel(T i)
    {
      if (labels == null)
        ComputeLabels(instructions);
      
      return labels.Contains(i);
    }
    
    void ComputeLabels(IEnumerable<T> instructions)
    {
      Debug.Assert(labels == null, "Labels must be null");
      labels = new HashSet<T>();
      foreach(T i in instructions) {
        switch (LinearInstructionAdapter<T>.GetFlowControl(i)) {
          case FlowControl.Cond_Branch:
          case FlowControl.Branch:
          var targets = GetTargetInstructions(i);
          foreach(T target in targets)
            labels.Add(target);
          break;        
        }
      }
    }

    IEnumerable<T> GetTargetInstructions(T i)
    {
      List<T> result = new List<T>(1);
        
      // if there are more multiple branches
      T[] targets = LinearInstructionAdapter<T>.GetOperand(i) as T[];
      if (targets == null) {
        T target = LinearInstructionAdapter<T>.GetOperand(i) as T;
        if (target != null) {
          result.Add(target);
           return result;
        }
      }
      else {
        foreach (T instr in targets)
          result.Add(instr);

        return result;
      }

      return result;
    }
        
    void ConnectBlocks()
    {
      foreach (BasicBlock<T> node in rawBlocks) {
        ConnectBlock(node);
      }
    }
    
    void ConnectBlock(BasicBlock<T> block)
    {
      if (block.Last == null)
        throw new ArgumentException ("Undelimited node at " + block.Last);

      T i = block.Last;
      switch (LinearInstructionAdapter<T>.GetFlowControl(i)) {
        case FlowControl.Return:
        case FlowControl.Branch: {
          var targets = GetTargetInstructions(i);
          foreach (var target in targets) {
            Debug.Assert(target != null, "Target cannot be null!");

            BasicBlock<T> successor = GetNodeContaining(target);
            block.Successors.Add(successor);
            successor.Predecessors.Add(block);
          }

          break;
        }
        // treat the call as next
        case FlowControl.Call:
        case FlowControl.Next:
        case FlowControl.Cond_Branch: {
          var targets = GetTargetInstructions(i);
          foreach (var target in targets) {
            Debug.Assert(target != null, "Target cannot be null!");

            BasicBlock<T> successor = GetNodeContaining(target);
            // Check whether the successor already exists. Can happen when having branches pointing
            // to one and the same block. Eg. switch with no break.
            if (block.Successors.IndexOf(successor) < 0) {
              block.Successors.Add(successor);
              successor.Predecessors.Add(block);
            }
          }
          // Make sure we don't have a branch pointing to the next instruction as a target
          if (LinearInstructionAdapter<T>.GetNext(block.Last) != null) {
            BasicBlock<T> successor = GetNodeContaining(LinearInstructionAdapter<T>.GetNext(block.Last));
            if (block.Successors.IndexOf(successor) < 0) {
              block.Successors.Add(successor);
              successor.Predecessors.Add(block);
            }
          }
          break;
        }

        case FlowControl.Throw:
          break;
        default:
          throw new NotSupportedException (
            string.Format("Unhandled instruction flow behavior {0}: {1}",
                          LinearInstructionAdapter<T>.GetFlowControl(i),
                          i.ToString(),                                                        
                          i.ToString()));
      }
    }

    BasicBlock<T> GetNodeContaining(T i)
    {
      foreach (BasicBlock<T> block in rawBlocks) {
        if (block.Contains(i))
          return block;
      }
      return null;
    }
  }
}
