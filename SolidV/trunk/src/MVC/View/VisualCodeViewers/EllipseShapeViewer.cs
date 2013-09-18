/*
 * $Id$
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
    
    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      context.Save();
      context.Scale(shape.Width, shape.Height);
      context.Arc(shape.Location.X / shape.Width + 0.5, shape.Location.Y/shape.Height + 0.5, 0.5, 0, 2 * Math.PI);
      context.Restore();
 
      if (view.Mode == ViewMode.Render) {
        context.Pattern = shape.Style.Fill;
        context.FillPreserve();
        context.Pattern = shape.Style.Border;
        context.Stroke();
      }
    }
  }
}
