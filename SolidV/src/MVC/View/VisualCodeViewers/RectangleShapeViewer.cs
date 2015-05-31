/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  public class RectangleShapeViewer : ShapeViewer
  {
    public RectangleShapeViewer()
    {
    }
    
    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      context.Rectangle(shape.Rectangle);

      if (view.Mode == ViewMode.Render) {
        context.SetSource(shape.Style.Fill);
        context.FillPreserve();
        context.SetSource(shape.Style.Border);
        context.Stroke();
      }
    }
    
  }

}
