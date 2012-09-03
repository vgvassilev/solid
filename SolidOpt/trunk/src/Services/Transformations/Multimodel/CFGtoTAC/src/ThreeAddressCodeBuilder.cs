/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

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
      System.Collections.Generic.List<Triplet> triplets = new System.Collections.Generic.List<Triplet>();
      Instruction instr = cfg.Root.First;
      while (instr != null) {
        //switch (instr.OpCode) {
        //case OpCodes.Ldarg:
        //    instr = instr.Next;

            //triplets.Add(Triplet.Create(OpCode.Push, ))
        //    break;
        //}
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
