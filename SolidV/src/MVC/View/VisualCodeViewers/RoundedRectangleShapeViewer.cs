/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  public class RoundedRectangleShapeViewer : ShapeViewer
  {
    public RoundedRectangleShapeViewer ()
    {
    }

    public override void DrawShape (IView<Context, Model> view, Context context, Shape shape)
    {
      double corner_radius = shape.Rectangle.Height / 10.0;
      double aspect = 1;
      double radius = corner_radius / aspect;
      double degrees = Math.PI / 180.0;

      

      context.NewPath ();
      context.Arc (shape.Rectangle.X + shape.Rectangle.Width - radius, shape.Location.Y + radius, radius, -90 * degrees, 0 * degrees);
      context.Arc (shape.Rectangle.X + shape.Rectangle.Width - radius, shape.Location.Y + shape.Height - radius, radius, 0 * degrees, 90 * degrees);
      context.Arc (shape.Rectangle.X + radius, shape.Location.Y + shape.Height - radius, radius, 90 * degrees, 180 * degrees);
      context.Arc (shape.Rectangle.X + radius, shape.Location.Y + radius, radius, 180 * degrees, 270 * degrees);
      context.ClosePath();

      

      if (view.Mode == ViewMode.Render) {
        context.SetSource (shape.Style.Fill);
        context.FillPreserve ();
        context.SetSource (shape.Style.Border);
        context.Stroke ();

        EllipseShape sh = (EllipseShape)shape;
        if (sh.Title != null) {
          context.SetSourceRGB (.15, .15, .15);
          context.SetFontSize (16);
          context.SelectFontFace ("Arial", FontSlant.Normal, FontWeight.Normal);

          double titleWidth = context.TextExtents (sh.Title).Width;
          double titleHeight = context.TextExtents (sh.Title).Height;
          // center the title in the box
          double titleX = shape.Rectangle.X + shape.Rectangle.Width / 2 - titleWidth / 2;
          double titleY = shape.Rectangle.Y + shape.Rectangle.Height / 2 + titleHeight / 2;

          context.MoveTo (titleX, titleY);
          context.ShowText (sh.Title);
        }
      }
    }

  }

}
