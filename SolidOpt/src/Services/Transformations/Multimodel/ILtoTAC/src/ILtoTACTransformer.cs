/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

//using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;
using SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoTAC
{
  public class ILtoTACTransformer : DecompilationStep, ITransform<MethodDefinition, ThreeAddressCode>
  {

    #region Constructors

    public ILtoTACTransformer() {
    }

    #endregion

    public override object Process(object codeModel)
    {
      return Process(codeModel as MethodDefinition);
    }

    public override Type GetSourceType()
    {
      return typeof(MethodDefinition);
    }

    public override Type GetTargetType()
    {
      return typeof(ThreeAddressCode);
    }

    public ThreeAddressCode Process(MethodDefinition source)
    {
      if (source == null)
        throw new ArgumentNullException ("method");

      var builder = new ThreeAddressCodeBuilder(source);
      ThreeAddressCode tac = builder.Create();
      //List<Triplet> nop = new List<Triplet>();
      //ControlFlowGraphBuilder<Triplet> cfgBuilder = new ControlFlowGraphBuilder<Triplet>(source, tac.RawTriplets, nop, nop);
      //ControlFlowGraph<Triplet> cfg = cfgBuilder.Create();

      return tac;
    }

    public ThreeAddressCode Transform(MethodDefinition source)
    {
      return Decompile(source);
    }

    public ThreeAddressCode Decompile (MethodDefinition source)
    {
      return Process(source);
    }
  }
}
