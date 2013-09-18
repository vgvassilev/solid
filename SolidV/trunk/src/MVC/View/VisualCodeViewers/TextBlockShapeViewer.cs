/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Cairo;

namespace SolidV.MVC
{
  public class TextBlockShapeViewer : ShapeViewer
  {
    public TextBlockShapeViewer() {
    }

    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      TextBlockShape sh = (TextBlockShape)shape;
      context.Rectangle(sh.Rectangle);

      if (view.Mode == ViewMode.Render) {
        context.Pattern = sh.Style.Fill;
        context.FillPreserve();

        context.Pattern = sh.Style.Border;
        double lineY = sh.Rectangle.Y + 15;
        double lineX = sh.Rectangle.X + 5;

        if (sh.Title != null) {
          double titleX = sh.Rectangle.X;
          double titleY = sh.Rectangle.Y;
          if (titleY - 10 > 0)
            titleY -= 10;
          context.SetFontSize(sh.FontSize);
          context.SelectFontFace("Arial", FontSlant.Normal, FontWeight.Bold) ;
          context.MoveTo(titleX, titleY);
          context.ShowText(sh.Title);
        }

        context.SelectFontFace("Arial", FontSlant.Normal, FontWeight.Normal);

        context.SetFontSize(sh.FontSize);
        context.MoveTo(lineX, lineY);

        foreach (string line in sh.Lines) {
          if (line.Length > 28)
            context.ShowText(line.Substring(0, 27));
          else
            context.ShowText(line);
          
          lineY += 15;
          context.MoveTo(lineX, lineY);
        }
        context.Stroke();
      }
    }
  }
}
