/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Cecil.Decompiler.Ast;

using SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree;

using SolidOpt.Services.Transformations.Optimizations;

namespace SolidOpt.Services.Transformations.Optimizations.AST.ConstantPropagation
{
  /// <summary>
  /// Description of ConstantPropagationTransformer.
  /// </summary>
  public class ConstantPropagationTransformer : BaseCodeTransformer, IOptimize<AstMethodDefinition>
  {
    
    //private Dictionary<Expression, Expression> substitutions = new Dictionary<Expression, Expression>();
    
    public ConstantPropagationTransformer()
    {
    }

    public AstMethodDefinition Transform(AstMethodDefinition source)
    {
      return Optimize(source);
    }
    
    public AstMethodDefinition Optimize(AstMethodDefinition source)
    {
      source.CecilBlock = (BlockStatement) Visit(source.CecilBlock);
      return source;
    }
    
    public override ICodeNode VisitAssignExpression(AssignExpression node)
    {
      return base.VisitAssignExpression(node);
//      if (node.Target is VariableReferenceExpression ||)
      
      
    }
    
    public override ICodeNode VisitArgumentReferenceExpression(ArgumentReferenceExpression node)
    {
      return base.VisitArgumentReferenceExpression(node);
    }
    
    public override ICodeNode VisitVariableReferenceExpression(VariableReferenceExpression node)
    {
      return base.VisitVariableReferenceExpression(node);
    }
    
    
  }
}
