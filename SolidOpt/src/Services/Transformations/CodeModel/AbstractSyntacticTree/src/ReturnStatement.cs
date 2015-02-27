/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
*/

using System;

namespace SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree.Nodes
{
  public class ReturnStatement : Statement
  {
    private Expression returnValue;
    public Expression ReturnValue {
      get { return returnValue; }
      set { returnValue = value; }
    }

    public ReturnStatement(Expression returnValue) {
      this.returnValue = returnValue;
    }
  }
}

