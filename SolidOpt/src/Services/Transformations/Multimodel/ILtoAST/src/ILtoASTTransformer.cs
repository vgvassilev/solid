/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

using SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree;
using SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode;

using SolidOpt.Services.Transformations.Multimodel;
using SolidOpt.Services.Transformations.Multimodel.ILtoTAC;
using SolidOpt.Services.Transformations.Multimodel.TACtoAST;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoAST
{
  /// <summary>
  /// Creates Cecil.Decompiler AST representation of the specified method body.
  /// The Cecil.Decompiler is going to be replaced in the next versions with SolidOpt.Decompiler
  /// </summary>
  public class ILtoASTTransformer : ITransform<MethodDefinition, AstMethodDefinition>
  {
    public ILtoASTTransformer()
    {
    }

    public AstMethodDefinition Transform(MethodDefinition source)
    {
      return Decompile(source);
    }

    public AstMethodDefinition Decompile(MethodDefinition source)
    {
      ILtoTACTransformer tacTransformer = new ILtoTACTransformer();
      ThreeAddressCode tac =  tacTransformer.Decompile(source);
      TACtoASTTransformer astTransformer = new TACtoASTTransformer();
      return astTransformer.Decompile(tac);
    }
  }
}
