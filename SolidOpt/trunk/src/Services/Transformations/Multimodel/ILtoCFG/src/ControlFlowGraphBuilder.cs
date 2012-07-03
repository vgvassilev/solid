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
	/// Control flow graph builder. Builds <seealso cref="ControlFlowGraph"/>ControlFlowGraph of a
  ///  method body given set of instructions.
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
	public class ControlFlowGraphBuilder {
		
		#region Fields & Properties

		MethodBody body;
		BasicBlock root = null;
		private List<Instruction> labels = new List<Instruction>();
		private List<BasicBlock> rawBlocks = new List<BasicBlock>();
		
		#endregion
		
		#region Constructors
		
		internal ControlFlowGraphBuilder(MethodDefinition method)
		{
			body = method.Body;
		}

		public ControlFlowGraph Create()
		{
			CreateBlocks();
			ConnectBlocks();

			return new ControlFlowGraph(root, rawBlocks);
		}
		
		#endregion

		void CreateBlocks()
		{
			BasicBlock curBlock = null;
			foreach(Instruction instr in body.Instructions) {

				if (IsBlockLeader(instr))
					curBlock = new BasicBlock(rawBlocks.Count.ToString());
				
				if (root == null)
					root = curBlock;
				
				curBlock.Add(instr);
        curBlock.Kind = BlockKind.Structure;

				if (IsBlockTerminator(instr))
					rawBlocks.Add(curBlock);
			}
		}

		bool IsBlockLeader(Instruction i)
		{
			if (i.Previous == null)
				return true;

      foreach (ExceptionHandler handler in body.ExceptionHandlers)
        if (handler.HandlerStart == i)
          return true;
			
			if (IsBlockTerminator(i.Previous))
				return true;
	
			// Check whether the instruction has label
			return HasLabel(i);
		}
		
		bool IsBlockTerminator(Instruction i) {
      //if ((i.OpCode.Code == Code.Leave) || (i.OpCode.Code == Code.Endfinally)) {
      //  return false;
      //}
			// first instruction in the collection starts a block
			switch (i.OpCode.FlowControl) {
				case FlowControl.Break:
				case FlowControl.Branch:
				case FlowControl.Return:
				case FlowControl.Cond_Branch:
				case FlowControl.Throw:
					return true;
			}
			
			if (i.Next != null)
				return HasLabel(i.Next);
			return false;
		}
		
		bool HasLabel(Instruction i)
		{
			if (labels != null)
				ComputeLabels(body.Instructions);
			
			return labels.Contains(i);
		}
		
		void ComputeLabels(Collection<Instruction> instructions)
		{
			foreach(Instruction inst in instructions) {
				switch (inst.OpCode.OperandType) {
					case OperandType.ShortInlineBrTarget:
					case OperandType.InlineBrTarget:
				case OperandType.InlineSwitch:
					var targets = GetTargetInstructions(inst);
					foreach(Instruction target in targets)
						labels.Add(target);
					break;				
				}
			}
		}
		
		Collection<Instruction> GetTargetInstructions(Instruction i)
		{
			Collection<Instruction> result = new Collection<Instruction>(1);
				
			// if there are more multiple branches
			Instruction[] targets = i.Operand as Instruction[];
			if (targets == null) {
				Instruction target = i.Operand as Instruction;
				if (target != null) {
					result.Add(target);
		 			return result;
				}
			}
			else {
				foreach (Instruction instr in targets)
					result.Add(instr);
				
				return result;
			}
	
			return result;
		}

				
		void ConnectBlocks()
		{
			foreach (BasicBlock node in rawBlocks) {
        if (node.Kind == BlockKind.Structure)
				  ConnectBlock(node);
			}
		}
		
		void ConnectBlock(BasicBlock block)
		{
			if (block.Last == null)
				throw new ArgumentException ("Undelimited node at offset " + block.Last.Offset);

			Instruction i = block.Last;
			switch (i.OpCode.FlowControl) {
        case FlowControl.Return:
				case FlowControl.Branch: {
					var targets = GetTargetInstructions(i);
					foreach (var target in targets) {
						Debug.Assert(target != null, "Target cannot be null!");

						BasicBlock successor = GetNodeContaining(target);
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

						BasicBlock successor = GetNodeContaining(target);
            // Check whether the successor already exists. Can happen when having branches pointing
            // to one and the same block. Eg. switch with no break.
            if (block.Successors.IndexOf(successor) < 0) {
  					  block.Successors.Add(successor);
						  successor.Predecessors.Add(block);
            }
					}
          // Make sure we don't have a branch pointing to the next instruction as a target
          if (block.Last.Next != null) {
            BasicBlock successor = GetNodeContaining(block.Last.Next);
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
						string.Format ("Unhandled instruction flow behavior {0}: {1}",
						               i.OpCode.FlowControl,
						               i.ToString(),				                                                
						               i.ToString()));
			}
		}
		
		BasicBlock GetNodeContaining(Instruction i)
		{
			foreach (BasicBlock block in rawBlocks) {
				if (block.Contains(i))
					return block;
			}
			return null;
		}
	}
}
