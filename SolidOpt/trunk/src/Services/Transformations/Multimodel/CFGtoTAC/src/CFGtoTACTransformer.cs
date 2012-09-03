// /*
//  * $Id: $
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using System;

using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;
using SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode;

namespace SolidOpt.Services.Transformations.Multimodel.CFGtoTAC
{
  public class CFGtoTACTransformer : DecompilationStep, IDecompile<ControlFlowGraph, Triplet>
  {

    #region Constructors

    public CFGtoTACTransformer() {
    }

    #endregion

    public override object Process(object codeModel)
    {
      return Process(codeModel as ControlFlowGraph);
    }

    public override Type GetSourceType()
    {
      return typeof(ControlFlowGraph);
    }

    public override Type GetTargetType()
    {
      return typeof(Triplet);
    }

    public Triplet Process(ControlFlowGraph source)
    {
      if (source == null)
        throw new ArgumentNullException ("method");

      var builder = new ThreeAddressCodeBuilder(source);
      return builder.Create();
   }


    #region IDecompile[ControlFlowGraph, Triplet] implementation
    public Triplet Decompile (ControlFlowGraph source)
    {
      return Process(source);
    }
    #endregion




  }
}

