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
      EllipseShape es = (EllipseShape) shape;
      context.Rectangle(es.Rectangle);

      double titleX = es.Rectangle.X + es.Width / 2 - context.TextExtents(es.Title).Width / 2;
      double titleY = es.Rectangle.Y + es.Height / 2 + context.TextExtents(es.Title).Height / 2;

      if (view.Mode == ViewMode.Render) {
        context.Save();
        context.Scale(shape.Width, shape.Height);
        context.NewPath();
        context.Arc(shape.Location.X / shape.Width + 0.5, shape.Location.Y/shape.Height + 0.5, 0.5, 0, 2 * Math.PI);
        context.ClosePath();
        context.Restore();

        context.SetSource(shape.Style.Fill);
        context.FillPreserve();
        context.SetSource(shape.Style.Border);

        context.Stroke();

        //context.NewPath();
        context.MoveTo(titleX, titleY);
        context.ShowText(es.Title);
        //context.ClosePath();
      }
    }
  }
}
