/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree.Nodes
{
  public abstract class Expression : Statement
  {
  }

  public abstract class UnaryExpression : Expression {
  }

  public sealed class SimpleNameExpression : UnaryExpression {
    #region implemented abstract members of Statement
    public override StatementKinds Kind {
      get { return StatementKinds.SimpleNameExpression;
      }
    }
    #endregion

    private string name;
    public string Name {
      get { return name; }
    }

    public SimpleNameExpression(string name) {
      this.name = name;
    }
  }

  // FIXME: Implement all literals: boolean, integer...
  public abstract class Literal : Expression {

  }

  public sealed class IntegerLiteral : Literal {
    #region implemented abstract members of Statement

    public override StatementKinds Kind {
      get { return StatementKinds.IntegerLiteral; }
    }

    #endregion

    private Int64 value;
    public Int64 Value {
      get { return value; }
    }

    public IntegerLiteral(Int64 value) {
      this.value = value;
    }
  }
}

