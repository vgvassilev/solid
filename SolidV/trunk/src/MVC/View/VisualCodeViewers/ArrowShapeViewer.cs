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

      PointD p = (sh.FromGlue != null) ?
        sh.FromGlue.TransformPointToGlobal(sh.FromGlue.Center, context) : sh.From.Center;
      p = sh.TransformPointToLocal(p, context);
      context.MoveTo(p);

      p = (sh.ToGlue != null) ?
        sh.ToGlue.TransformPointToGlobal(sh.ToGlue.Center, context) : sh.To.Center;
      p = shape.TransformPointToLocal(p, context);
      context.ArrowLineTo(p, sh.ArrowKindHead, sh.ArrowKindTail);

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
