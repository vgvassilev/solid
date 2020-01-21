/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Cairo;

namespace SolidV.MVC
{
  public class RoundedTextBlockShapeViewer : ShapeViewer
  {
    public RoundedTextBlockShapeViewer() { }

    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      TextBlockShape sh = (TextBlockShape)shape;

      int titleBoxHeight = 25;

      context.Rectangle (sh.Location.X, sh.Location.Y + titleBoxHeight, sh.Width, sh.Height - titleBoxHeight);

      context.Save();
      context.Matrix = shape.Matrix;

      if (view.Mode == ViewMode.Render) {
        context.SetSourceRGB (.8, .8, .8);
        context.Rectangle (sh.Location.X, sh.Location.Y, sh.Width, sh.Height);
        context.MoveTo (sh.Location.X, sh.Location.Y + 30);
        context.LineTo (sh.Location.X + sh.Width, sh.Location.Y + 30);
        context.Fill ();

        context.SetSourceRGB (.9, .9, .9);
        context.Rectangle (sh.Location.X, sh.Location.Y + titleBoxHeight, sh.Width, sh.Height - titleBoxHeight);
        context.Fill ();

        context.SetSourceRGB (.8, .8, .8);
        context.Rectangle (sh.Location.X, sh.Location.Y, sh.Width, sh.Height);
        context.Stroke ();

        context.SetSourceRGB(.9, .9, .9);
        context.FillPreserve();

        if (sh.Title != null) {
          context.SetSourceRGB (.15, .15, .15);
          context.SetFontSize (16);
          context.SelectFontFace ("Arial", FontSlant.Normal, FontWeight.Bold);

          double titleWidth = context.TextExtents(sh.Title).Width;
          double titleHeight = context.TextExtents(sh.Title).Height;
          // center the title in the box
          double titleX = sh.Rectangle.X + sh.Rectangle.Width / 2 - titleWidth / 2;
          double titleY = sh.Rectangle.Y + titleBoxHeight / 2 + titleHeight / 2 + titleBoxHeight/20;
          
          context.NewPath();
          context.MoveTo(titleX, titleY);
          context.ShowText(sh.Title);
          context.ClosePath();
        }

        context.SetSourceRGB(0, 0, 0);
        context.NewPath();

        // Body
        context.LineWidth = .5;
        context.SetSource(sh.Style.Fill);
        context.SetSourceRGB(0, 0, 0);

        double lineY = sh.Rectangle.Y + titleBoxHeight + 23;
        double lineX = sh.Rectangle.X + 5;

        foreach (string line in sh.Lines) {
          context.MoveTo(lineX, lineY);
          context.ShowText(line);
          lineY += 15;
        }
        context.ClosePath();
      }
      context.Restore();
    }
  }
}
