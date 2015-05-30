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
    public RoundedTextBlockShapeViewer() {
    }

    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      TextBlockShape sh = (TextBlockShape)shape;

      int titleBoxHeight = 25;
      int textOffsetY = 5;

      context.Rectangle(sh.Location.X, sh.Location.Y + titleBoxHeight, sh.Width,
                        sh.Height - titleBoxHeight);
      context.Save();
      context.Matrix = shape.Matrix;

      if (view.Mode == ViewMode.Render) {
        context.SetSourceRGB(.31, .31, .31);
        context.FillPreserve();
        context.SetSourceRGB(0, 0, 0);
        context.Stroke();

        if (sh.Title != null) {
          double titleWidth = context.TextExtents(sh.Title).Width;
          double titleHeight = context.TextExtents(sh.Title).Height;
          // center the title in the box
          double titleX = sh.Rectangle.X + sh.Rectangle.Width / 2 - titleWidth / 2;
          double titleY = sh.Rectangle.Y + titleBoxHeight / 2 + titleHeight / 2;

          context.SetSourceRGB(.15, .15, .15);
          context.SetFontSize(sh.FontSize);
          context.SelectFontFace("Arial", FontSlant.Normal, FontWeight.Bold) ;
          context.NewPath();
          context.MoveTo(titleX, titleY);
          context.ShowText(sh.Title);
          context.ClosePath();
        }

        double toRadians = Math.PI / 180;
        // arc data
        double xCoord, yCoord, radius, arcStart, arcEnd;

        context.SetSourceRGB(0, 0, 0);
        context.NewPath();

        // upper right
        radius = 9;
        xCoord = sh.Location.X + sh.Width - radius;
        yCoord = sh.Location.Y + radius;
        arcStart = 270 * toRadians;
        arcEnd = 0;
        context.Arc(xCoord, yCoord, radius, arcStart, arcEnd);

        // lower right
        radius = 0;
        xCoord = sh.Location.X + sh.Width;
        yCoord = titleBoxHeight + sh.Location.Y;
        arcStart = 0;
        arcEnd = 90 * toRadians;
        context.Arc(xCoord, yCoord, radius, arcStart, arcEnd);

        // lower left
        radius = 0;
        xCoord = sh.Location.X;
        yCoord = titleBoxHeight + sh.Location.Y;
        arcStart = 90 * toRadians;
        arcEnd = 180 * toRadians;
        context.Arc(xCoord, yCoord, radius, arcStart, arcEnd);

        // upper left
        radius = 9;
        xCoord = sh.Location.X + radius;
        yCoord = sh.Location.Y + radius;
        arcStart = 180 * toRadians;
        arcEnd = 270 * toRadians;
        context.Arc(xCoord, yCoord, radius, arcStart, arcEnd);

        context.ClosePath();
        context.Stroke();

        // Body
        context.LineWidth = .5;
        context.SetSource(sh.Style.Fill);
        context.SetSourceRGB(1, 0.6, 0.2);

        double instrBoxHeight = 0;
        double lineY = sh.Rectangle.Y + titleBoxHeight + 15;
        double lineX = sh.Rectangle.X + 5;

        foreach (string line in sh.Lines) {
          context.MoveTo(lineX, lineY);
          context.ShowText(line);
          lineY += 15;
          instrBoxHeight += context.TextExtents(line).Height + textOffsetY;
        }
        context.ClosePath();
      }
      context.Restore();
    }
  }
}
