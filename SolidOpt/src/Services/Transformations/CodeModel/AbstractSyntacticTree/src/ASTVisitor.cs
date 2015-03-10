/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Text;

namespace SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree
{
  public class ASTVisitor
  {
    public ASTVisitor() {
    }

    public virtual void Visit(Statement stmt) {
      switch (stmt.Kind) {
        case StatementKinds.Statement:
          throw new NotImplementedException();
        case StatementKinds.BlockStatement:
          VisitBlockStatement((BlockStatement)stmt);
          break;
        case StatementKinds.SimpleNameExpression:
          VisitSimpleNameExpression((SimpleNameExpression)stmt);
          break;
        case StatementKinds.IntegerLiteral:
          VisitIntegerLiteral((IntegerLiteral)stmt);
          break;
        case StatementKinds.ReturnStatement:
          VisitReturnStatement((ReturnStatement)stmt);
          break;
        case StatementKinds.AssignmentStatement:
          VisitAssignmentStatement((AssignmentStatement)stmt);
          break;
        default:
          throw new NotImplementedException();
      }
    }

    protected virtual void VisitBlockStatement(BlockStatement stmt) {
      foreach (Statement item in stmt.Statements)
        Visit(item);
    }

    protected virtual void VisitSimpleNameExpression(SimpleNameExpression expr) {
      // do nothing it is a leaf.
    }

    protected virtual void VisitIntegerLiteral(IntegerLiteral expr) {
      // do nothing it is a leaf.
    }

    protected virtual void VisitReturnStatement(ReturnStatement stmt) {
      // if a non-void return
      if (stmt.ReturnExpr != null)
        Visit(stmt.ReturnExpr);
    }

    protected virtual void VisitAssignmentStatement(AssignmentStatement stmt) {
      Visit(stmt.Lhs);
      Visit(stmt.Rhs);
    }
  }

  public sealed class ASTDumper : ASTVisitor {
    private StringBuilder sb = new StringBuilder();
    private int indent = 0;

    public override string ToString() {
      return sb.ToString();
    }

    private void AppendStringLine(string str) {
      sb.Append(' ', indent);
      sb.AppendLine(str);
    }

    protected override void VisitBlockStatement(BlockStatement stmt) {
      AppendStringLine("`<BlockStatement>");
      ++indent;
      base.VisitBlockStatement(stmt);
      --indent;
    }

    protected override void VisitSimpleNameExpression(SimpleNameExpression expr) {
      AppendStringLine(string.Format("`<SimpleNameExpression name={0}>", expr.Name));
      // do nothing it is a leaf.
    }

    protected override void VisitIntegerLiteral(IntegerLiteral expr) {
      AppendStringLine(string.Format("`<IntegerLiteral value={0}>", expr.Value));
    }

    protected override void VisitReturnStatement(ReturnStatement stmt) {
      AppendStringLine(string.Format("`<ReturntStatement>"));
      ++indent;
      base.VisitReturnStatement(stmt);
      --indent;
    }

    protected override void VisitAssignmentStatement(AssignmentStatement stmt) {
      AppendStringLine(string.Format("`<AssignmentStatement kind={0}>", stmt.OpKind));
      ++indent;
      base.VisitAssignmentStatement(stmt);
      --indent;
    }
  }
}

