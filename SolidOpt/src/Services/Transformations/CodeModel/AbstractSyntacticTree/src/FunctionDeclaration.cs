/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree.Nodes
{
  public class FunctionDeclaration : Declaration
  {
    private List<Statement> statements = new List<Statement>();
    public List<Statement> Statements {
      get { return statements; }
    }

    public FunctionDeclaration() {
    }
  }
}

