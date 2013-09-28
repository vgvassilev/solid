/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Mono.Cecil.Cil;

using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;
using SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode;

namespace SolidOpt.Services.Transformations.Multimodel.CFGtoTAC
{
  public class CFGtoTACTransformer : DecompilationStep, ITransform<ControlFlowGraph<Instruction>, ThreeAddressCode>
  {

    #region Constructors

    public CFGtoTACTransformer() {
    }

    #endregion

    public override object Process(object codeModel)
    {
      return Process(codeModel as ControlFlowGraph<Instruction>);
    }

    public override Type GetSourceType()
    {
      return typeof(ControlFlowGraph<Instruction>);
    }

    public override Type GetTargetType()
    {
      return typeof(ThreeAddressCode);
    }

    public ThreeAddressCode Process(ControlFlowGraph<Instruction> source)
    {
      if (source == null)
        throw new ArgumentNullException ("method");

      var builder = new ThreeAddressCodeBuilder(source);
      return builder.Create();
    }

    public ThreeAddressCode Transform(ControlFlowGraph<Instruction> source)
    {
      return Decompile(source);
    }

    public ThreeAddressCode Decompile (ControlFlowGraph<Instruction> source)
    {
      return Process(source);
    }
  }
}
