/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;

using Mono.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoCFG
{
	/// <summary>
	/// Control flow graph builder.
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
	class ControlFlowGraphBuilder {
		
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

			return new ControlFlowGraph(root);
		}
		
		#endregion
		
		void CreateBlocks()
		{
			BasicBlock curBlock = null;
			Instruction instr;
			for(int i = 0; i < body.Instructions.Count; i++) {
				instr = body.Instructions[i];
				if (IsBlockLeader(instr))
					curBlock = new BasicBlock(i.ToString());
				
				if (root == null)
					root = curBlock;
				
				curBlock.Add(instr);
				
				if (IsBlockTerminator(instr))
					rawBlocks.Add(curBlock);
			}
		}

		bool IsBlockLeader(Instruction i)
		{
			if (i.Previous == null)
				return true;
			
			if (IsBlockTerminator(i.Previous))
				return true;
	
			// Check whether the instruction has label
			return HasLabel(i);
		}
		
		bool IsBlockTerminator(Instruction i) {
			// first instruction in the collection starts a block
			switch (i.OpCode.FlowControl) {
				case FlowControl.Break:
				case FlowControl.Branch:
				case FlowControl.Return:
				case FlowControl.Cond_Branch:
				case FlowControl.Throw:
					return true;
			}
			
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
				ConnectBlock(node);
			}
		}
		
		void ConnectBlock(BasicBlock block)
		{
			if (block.Last == null)
				throw new ArgumentException ("Undelimited node at offset " + block.Last.Offset);

			Instruction i = block.Last;
			switch (i.OpCode.FlowControl) {
				case FlowControl.Branch: {
					var targets = GetTargetInstructions(i);
					foreach (var target in targets) {
						if (target.Next != null) {
							BasicBlock successor = GetNodeContaining(target.Next);
							block.Successors.Add(successor);
							successor.Predecessors.Add(block);
						}
					}
					
					break;
				}
				case FlowControl.Next:
				case FlowControl.Cond_Branch: {
					var targets = GetTargetInstructions(i);
					foreach (var target in targets) {
						if (target.Next != null) {
							BasicBlock successor = GetNodeContaining(target.Next);
							block.Successors.Add(successor);
							successor.Predecessors.Add(block);
						}
					}
					if (block.Last.Next != null) {
						BasicBlock successor = GetNodeContaining(block.Last.Next);
						block.Successors.Add(successor);
						successor.Predecessors.Add(block);
					}
					break;
				}

				case FlowControl.Call:
				case FlowControl.Return:
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
			foreach (BasicBlock node in rawBlocks) {
				if (node.Contains(i))
					return node;
			}
			return null;
		}
		
	}
}