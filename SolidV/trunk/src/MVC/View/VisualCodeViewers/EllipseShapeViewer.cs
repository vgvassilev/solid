/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  public class EllipseShapeViewer : ShapeViewer
  {
    public EllipseShapeViewer()
    {
    }
    
    public override void DrawItem(IView<Context, Model> view, Context context, object item)
    {
      Shape shape = (Shape)item;
      //context.Scale(shape.Width / shape.Height, 1);
      context.Arc(shape.Location.X + shape.Width / 2, shape.Location.Y + shape.Height / 2, shape.Width / 2, 0, 2 * Math.PI);
      //context.Arc(0, 0, 1, 0, 2 * Math.PI);
      context.Color = shape.Style.FillColor;
      context.FillPreserve();
      context.Color = shape.Style.BorderColor;
      context.Stroke();
    }
  }
}
