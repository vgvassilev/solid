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

      // draw a box for the title
      //context.Rectangle(sh.Location.X, sh.Location.Y - 20, sh.Width, 20);

      //if (view.Mode == ViewMode.Render) {
      //double radius = sh.Height / 20;
      double radius = 9;

      int textOffsetY = 5;
     
      context.SetSourceRGB(.19,.29,.39);
      context.Fill();

      context.Save();

      // Title box
      int titleBoxHeight = 25;


      if (sh.Title != null) {
        // center the title in the box
        double titleX = sh.Rectangle.X + sh.Rectangle.Width / 2 - context.TextExtents(sh.Title).Width / 2;
        double titleY = sh.Rectangle.Y - titleBoxHeight / 2 + context.TextExtents(sh.Title).Height / 2;
        //context.SetFontSize(sh.FontSize);
        //context.SelectFontFace("Arial", FontSlant.Normal, FontWeight.Bold) ;
        context.NewPath();
        context.MoveTo(titleX, titleY);
        context.ShowText(sh.Title);
        context.ClosePath();
      }

      double toRadians = Math.PI / 180;

      context.NewPath();
      // upper right
      context.Arc(sh.Location.X + sh.Width - radius, -titleBoxHeight + sh.Location.Y + radius, radius, 270 * toRadians, 0);
      // lower right
      context.Arc(sh.Location.X + sh.Width - 0, -titleBoxHeight + sh.Location.Y + titleBoxHeight - 0, 0, 0, 90 * toRadians);
      // lower left
      context.Arc(sh.Location.X + 0, -titleBoxHeight + sh.Location.Y + titleBoxHeight - 0, 0, 90 * toRadians, 180 * toRadians);
      // upper left
      context.Arc(sh.Location.X + radius, -titleBoxHeight + sh.Location.Y + radius, radius, 180 * toRadians, 270 * toRadians);
      context.ClosePath();

      // Body
      context.LineWidth = .5;
      context.SetSource(sh.Style.Fill);
      context.SetSourceRGB(1, 0.6, 0.2);

      double instrBoxHeight = 0;
      double lineY = sh.Rectangle.Y  + 15;
      double lineX = sh.Rectangle.X + 5;

      foreach (string line in sh.Lines) {
        context.MoveTo(lineX, lineY);
        context.ShowText(line);
        lineY += 15;
        instrBoxHeight += context.TextExtents(line).Height + textOffsetY;
      }

      context.Rectangle(sh.Location.X, sh.Location.Y, sh.Width, instrBoxHeight);
      context.ClosePath();
      context.SelectFontFace("Arial", FontSlant.Normal, FontWeight.Normal);

      context.SetFontSize(sh.FontSize);
      context.MoveTo(lineX, lineY);

      
      context.ClosePath();
      context.Restore();
    }
  }
}
