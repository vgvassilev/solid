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
      Label label = (Label)shape;
      ArrowShape arrow = (ArrowShape)label.Parent;
      Glue fromGLue = arrow.FromGlue;
      Glue toGlue = arrow.ToGlue;

      context.Save();
      context.Matrix = shape.Matrix;

      PointD p1 = (fromGLue != null) ?
        fromGLue.TransformPointToGlobal(fromGLue.Center, context) : arrow.From.Center;
      p1 = label.TransformPointToLocal(p1, context);

      PointD p2 = (toGlue != null) ?
        toGlue.TransformPointToGlobal(toGlue.Center, context) : arrow.To.Center;
      p2 = label.TransformPointToLocal(p2, context);



      double x = Math.Min(p1.X, p2.X) + Math.Abs(p1.X - p2.X)/2;
      double y = Math.Min(p1.Y, p2.Y) + Math.Abs(p1.Y - p2.Y)/2;

      PointD p = new PointD(x, y);

      context.MoveTo(p);

      context.ShowText(label.TextLabel);

      context.Restore();
      if (view.Mode == ViewMode.Render) {
        Console.WriteLine(string.Format("arrow X: {0}, arrow Y: {1}", p.X.ToString(), p.Y.ToString()));
      }
    }
  }
}

