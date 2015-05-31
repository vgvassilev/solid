/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{

  public class FocusRectangleShapeViewer : ShapeViewer
  {
    public FocusRectangleShapeViewer()
    {
    }

    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      context.Rectangle(shape.Rectangle);

      if (view.Mode == ViewMode.Render) {
        context.Save();

        context.SetSource(shape.Style.Fill);
        context.FillPreserve();

        context.SetDash(new double[]{1.0, 1.0}, 0);
        context.Operator = Operator.Over;
        context.SetSource(shape.Style.Border);
        context.Stroke();

        context.Restore();
      }
    }

  }
}
