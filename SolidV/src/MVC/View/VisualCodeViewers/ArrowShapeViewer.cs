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

      context.Save();
      context.Matrix = shape.Matrix;

      PointD p = (sh.FromGlue != null) ?
        sh.FromGlue.TransformPointToGlobal(sh.FromGlue.Center, context) : sh.From.TransformPointToGlobal(sh.From.Center, context);
      //p = sh.TransformPointToGlobal(p, context);
      context.MoveTo(p);

      p = (sh.ToGlue != null) ?
        sh.ToGlue.TransformPointToGlobal(sh.ToGlue.Center, context) : sh.To.TransformPointToGlobal(sh.To.Center, context);
      //p = sh.TransformPointToGlobal(p, context);
      context.ArrowLineTo(p, sh.ArrowKindHead, sh.ArrowKindTail);

      if (view.Mode == ViewMode.Render) {
        context.SetSource(sh.Style.Fill);
        context.FillPreserve();
        context.SetSource(sh.Style.Border);
        context.LineWidth = 1.5;
        context.Stroke();
      }

      context.Restore();
    }
  }
}
