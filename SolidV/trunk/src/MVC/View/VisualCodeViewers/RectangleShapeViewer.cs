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
        context.Pattern = shape.Style.Fill;
        context.FillPreserve();
        context.Pattern = shape.Style.Border;
        context.Stroke();
      }
    }
    
  }
}
