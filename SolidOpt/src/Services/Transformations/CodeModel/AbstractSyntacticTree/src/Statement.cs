/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree
{
  public enum StatementKinds {
    Statement,
    BlockStatement,
    SimpleNameExpression,
    IntegerLiteral,
    ReturnStatement,
    AssignmentStatement,
  }
  public abstract class Statement
  {
    public abstract StatementKinds Kind { get; }
  }

  public sealed class BlockStatement : Statement {
    #region implemented abstract members of Statement

    public override StatementKinds Kind {
      get { return StatementKinds.BlockStatement; }
    }

    #endregion

    private List<Statement> statements = new List<Statement>();

    public List<Statement> Statements {
      get { return statements; }
    }

    public BlockStatement() {
    }
  }

  public enum AssignmentOperatorKind {
    Equal
  }

  public sealed class AssignmentStatement : Statement {
    #region implemented abstract members of Statement

    public override StatementKinds Kind {
      get { return StatementKinds.AssignmentStatement; }
    }

    #endregion

    private UnaryExpression lhs;
    public UnaryExpression Lhs {
      get { return lhs; }
    }

    private AssignmentOperatorKind opKind;
    public AssignmentOperatorKind OpKind {
      get { return opKind; }
    }

    private Expression rhs;
    public Expression Rhs {
      get { return rhs; }
    }

    public AssignmentStatement(UnaryExpression lhs, AssignmentOperatorKind opKind, Expression rhs) {
      this.lhs = lhs;
      this.opKind = opKind;
      this.rhs = rhs;
    }
  }
}

