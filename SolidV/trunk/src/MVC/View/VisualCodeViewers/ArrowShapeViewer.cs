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
  public class ArrowShapeViewer : ShapeViewer
  {
    public ArrowShapeViewer()
    {
    }

    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      ArrowShape sh = (ArrowShape)shape;

      double x, y;

      if (sh.FromGlue != null) {
        x = sh.FromGlue.Center.X;
        y = sh.FromGlue.Center.Y;
      } else {
        x = 0;
        y = 0;
      }
      if (sh.FromGlue != null) sh.FromGlue.Matrix.TransformPoint(ref x, ref y);
      sh.From.Matrix.TransformPoint(ref x, ref y);
      context.MoveTo(x, y);

      if (sh.ToGlue != null) {
        x = sh.ToGlue.Center.X;
        y = sh.ToGlue.Center.Y;
      } else {
        x = 0;
        y = 0;
      }
      if (sh.ToGlue != null) sh.ToGlue.Matrix.TransformPoint(ref x, ref y);
      sh.To.Matrix.TransformPoint(ref x, ref y);

      context.ArrowLineTo(x, y, sh.ArrowKindHead, sh.ArrowKindTail);

      if (view.Mode == ViewMode.Render) {
        context.Pattern = sh.Style.Fill;
        context.FillPreserve();
        context.Pattern = sh.Style.Border;
        context.LineWidth = 1.5;
        context.Stroke();
      }
    }
  }
}
