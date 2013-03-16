/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  public class ShapeViewer : Viewer<Context, Model>
  {
    public ShapeViewer()
    {
    }
    
    public override void DrawItem(IView<Context, Model> view, Context context, object item)
    {
      Shape shape = (Shape)item;
      context.Rectangle(shape.Rectangle);
      context.Color = shape.Style.FillColor;
      context.FillPreserve();
      context.Color = shape.Style.BorderColor;
      context.Stroke();
    }
  }
}
