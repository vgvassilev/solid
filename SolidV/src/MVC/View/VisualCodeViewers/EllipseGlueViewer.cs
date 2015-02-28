/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  //TODO: define visual states of Shapes (visible/invisible, maximized/normal/minimized/..., ...)

  public class EllipseGlueViewer : ShapeViewer
  {
    public EllipseGlueViewer()
    {
    }
    
    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      context.Save();
      context.Scale(shape.Width, shape.Height);
      context.Arc(shape.Location.X / shape.Width + 0.5, shape.Location.Y/shape.Height + 0.5, 0.5, 0, 2 * Math.PI);
      context.Restore();

      if (view.Mode == ViewMode.Render) {
        context.SetSource(shape.Style.Border);
        context.Stroke();
      }
    }
    
  }
}
