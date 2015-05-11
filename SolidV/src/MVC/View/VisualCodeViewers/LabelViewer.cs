/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

using SolidV.Cairo;

namespace SolidV.MVC
{
  public class LabelViewer : ShapeViewer
  {
    public LabelViewer() { }

    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      Label lb = (Label)shape;
      ArrowShape arrow = (ArrowShape)lb.Parent;

      context.Save();
      context.Matrix = shape.Matrix;

      PointD pointFrom = (arrow.FromGlue != null) ?
        arrow.FromGlue.TransformPointToGlobal(arrow.FromGlue.Center, context) : arrow.From.Center;
      pointFrom = lb.TransformPointToLocal(pointFrom, context);

      PointD pointTo = (arrow.ToGlue != null) ?
        arrow.ToGlue.TransformPointToGlobal(arrow.ToGlue.Center, context) : arrow.To.Center;
      pointTo = lb.TransformPointToLocal(pointTo, context);

      double x = Math.Min(pointFrom.X, pointTo.X) + Math.Abs(pointFrom.X - pointTo.X)/2;
      double y = Math.Min(pointFrom.Y, pointTo.Y) + Math.Abs(pointFrom.Y - pointTo.Y)/2;

      PointD p = new PointD(x, y);

      context.MoveTo(p);
      context.ShowText(lb.TextLabel);

      context.Restore();
    }
  }
}

