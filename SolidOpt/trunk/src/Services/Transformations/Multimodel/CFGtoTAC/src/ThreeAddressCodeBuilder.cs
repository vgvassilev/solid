/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;
using SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode;

namespace SolidOpt.Services.Transformations.Multimodel.CFGtoTAC
{
  public class ThreeAddressCodeBuilder
  {
    private ControlFlowGraph cfg = null;

    public ThreeAddressCodeBuilder(ControlFlowGraph cfg) {
      this.cfg = cfg;
    }

    public Triplet Create() {
      List<Triplet> triplets = new List<Triplet>();
      Stack<object> simulationStack = new Stack<object>();
      Instruction instr = cfg.Root.First;
      while (instr != null) {
        switch (instr.OpCode.Code) {
          case Code.Ldarg:
            simulationStack.Push(instr.Operand); //TODO: Create Arg operand with numner instr.Operand
            break;
          case Code.Ldarg_0:
            simulationStack.Push(instr.Operand); //TODO: Create Arg operand with number 0
            break;
          //...
          case Code.Add:
            object newTemp = new object(); //TODO: Implement Temp operand generation
            triplets.Add(new Triplet(TripletOpCode.Add, newTemp, simulationStack.Pop(), simulationStack.Pop()));
            simulationStack.Push(newTemp);
            break;
          //...
          case Code.Starg:
              triplets.Add(new Triplet(TripletOpCode.Assignment, instr.Operand)); //TODO: Create Arg operand with numner instr.Operand
              simulationStack.Push(newTemp);
              break;
          //...
          default:
            throw new NotImplementedException();
        }
        instr = instr.Next;
      }

      //triplets.Add(CreateTriplet());
      // Here should be the implementation
      return null;
    }
  }

/*L0: flag = 0;
L1: PushParam "A"
L2: tmp0 = call s.StartsWith();
L3: ifFalse tmp0 goto L5;
L4: flag = s.Length >= 3;
L5: flag = false;
L6: ret flag;
*/
}
