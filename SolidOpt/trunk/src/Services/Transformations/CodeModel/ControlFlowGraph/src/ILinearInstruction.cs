/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph
{
  public interface ILinearInstruction
  {
    ILinearInstruction GetPrevious();
    ILinearInstruction GetNext();
    object Operand { get; }
    Mono.Cecil.Cil.FlowControl FlowControl { get; }
  }
}
