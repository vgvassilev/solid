// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */

/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  
  public class DiamondShapeViewer : ShapeViewer
  {
    public DiamondShapeViewer()
    {
    }

    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      context.MoveTo(shape.Location.X + shape.Width / 2, shape.Location.Y);
      context.RelLineTo(shape.Width / 2, shape.Height / 2);
      context.RelLineTo(-shape.Width / 2, shape.Height / 2);
      context.RelLineTo(-shape.Width / 2, -shape.Height / 2);
      context.RelLineTo(shape.Width / 2, -shape.Height / 2);
      context.ClosePath();

      if (view.Mode == ViewMode.Render) {
        context.SetSource(shape.Style.Fill);
        context.FillPreserve();
        context.SetSource(shape.Style.Border);
        context.Stroke();
      }
    }

  }

}
