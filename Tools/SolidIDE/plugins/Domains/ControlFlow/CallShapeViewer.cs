/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using Cairo;

using SolidV;
using SolidV.MVC;

namespace SolidIDE.Domains.ControlFlow
{

  public class CallShapeViewer : ShapeViewer
  {
    public CallShapeViewer() {}

    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      context.Rectangle(shape.Rectangle);

      if (view.Mode == ViewMode.Render) {
        context.SetSource(shape.Style.Fill);
        context.FillPreserve();
        context.SetSource(shape.Style.Border);
        context.MoveTo(shape.Location.X + 5, shape.Location.Y);
        context.LineTo(shape.Location.X + 5, shape.Location.Y + shape.Height);
        context.MoveTo(shape.Location.X + shape.Width - 5, shape.Location.Y);
        context.LineTo(shape.Location.X + shape.Width - 5, shape.Location.Y + shape.Height);
        context.Stroke();
      }
    }

  }

}
