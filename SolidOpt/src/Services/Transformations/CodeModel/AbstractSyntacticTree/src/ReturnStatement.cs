/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree
{
  public class ReturnStatement : Statement
  {
    #region implemented abstract members of Statement

    public override StatementKinds Kind {
      get { return StatementKinds.ReturnStatement; }
    }

    #endregion

    private Expression returnExpr;
    public Expression ReturnExpr {
      get { return returnExpr; }
    }

    public ReturnStatement(Expression returnExpr) {
      this.returnExpr = returnExpr;
    }
  }
}

