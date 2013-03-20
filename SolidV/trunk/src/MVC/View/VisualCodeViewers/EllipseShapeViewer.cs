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
      context.Save();
      context.Matrix = shape.Matrix;
      context.Scale(shape.Width, shape.Height);
      context.Arc(shape.Location.X/shape.Width+0.5, shape.Location.Y/shape.Height+0.5, 0.5, 0, 2 * Math.PI);
      context.Restore();
      context.Color = shape.Style.FillColor;
      context.FillPreserve();
      context.Color = shape.Style.BorderColor;
      context.Stroke();
    }
  }
}
