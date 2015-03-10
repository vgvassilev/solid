/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Text;

using Mono.Cecil;

using Cecil.Decompiler.Ast;

namespace SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree
{
  /// <summary>
  /// Description of AstMethodDefinition.
  /// </summary>
  public class AstMethodDefinition
  {
    
    private MethodDefinition method;    
    public MethodDefinition Method {
      get { return method; }
      set { method = value; }
    }
    
    private BlockStatement block;
    public BlockStatement Block {
      get { return block; }
      set { block = value; }
    }
    
    public AstMethodDefinition(MethodDefinition method, BlockStatement block)
    {
      this.method = method;
      this.block = block;
    }

    public override string ToString()
    {
      ASTDumper dumper = new ASTDumper();
      dumper.Visit(block);
      return dumper.ToString();
    }
  }
}
