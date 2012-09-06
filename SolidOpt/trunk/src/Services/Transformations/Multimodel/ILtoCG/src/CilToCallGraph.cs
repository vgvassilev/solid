/*
 * $Id: ILtoCFGTransformer.cs 593 2012-08-24 11:35:30Z vvassilev $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

using SolidOpt.Services.Transformations.Multimodel;
using SolidOpt.Services.Transformations.CodeModel.CallGraph;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoCG
{
 /// <summary>
 /// Description of CilToControlFlowGraph.
 /// </summary>
 public class CilToCallGraph : DecompilationStep, IDecompile<MethodDefinition, CallGraph>
 {   
    #region Constructors
    
    public CilToCallGraph ()
    {
    }
    
    #endregion
    
    public override object Process(object codeModel)
    {
      return Process (codeModel as MethodBody);
    }
    
    public override Type GetSourceType()
    {
      return typeof(MethodDefinition);
    }
    
    public override Type GetTargetType()
    {
      return typeof(CallGraph);
    }
    
    public CallGraph Process(MethodBody source)
    {
      if (source == null)
        throw new ArgumentNullException ("method");
      if (!source.Method.HasBody)
        throw new ArgumentException();
    
      var builder = new CallGraphBuilder(source.Method);
      return builder.Create();
    }
    
    public CallGraph Decompile(MethodDefinition source)
    {
      return Process(source.Body);
    }
  }
}
