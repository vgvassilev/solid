/*
 * $Id:
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

    public override void DrawItem(IView<Context, Model> view, Context context, object item)
    {
      TextBlockShape shape = (TextBlockShape)item;
      if (shape.AutoSize) {
        double width = 100;
        double height = shape.LineCount;
        if (shape.LongestLine < 100) {
          width = shape.LongestLine * 8;
        }
        if (shape.LineCount < 28) {
          height = shape.LineCount * 16;
        }
        //FIXME: This is absolutely wrong. This has to be moved outside of the visualization.
        // The visualization part only 'reads' the model doesn't 'write' or change it.
        shape.Rectangle = new Rectangle(shape.Location.X, shape.Location.Y, width, height);
      }

      context.Rectangle(shape.Rectangle);
      context.Color = shape.Style.FillColor;
      context.FillPreserve();

      context.Color = shape.Style.BorderColor;
      double lineY = shape.Rectangle.Y + 15;
      double lineX = shape.Rectangle.X + 5;

      if (shape.Title != null) {
        double titleX = shape.Rectangle.X;
        double titleY = shape.Rectangle.Y;
        if (titleY - 10 > 0)
          titleY -= 10;
        context.SetFontSize(14);
        context.SelectFontFace("Arial", FontSlant.Normal, FontWeight.Bold) ;
        context.MoveTo(titleX, titleY);
        context.ShowText(shape.Title);
      }

      context.SelectFontFace("Arial", FontSlant.Normal, FontWeight.Normal);
      context.SetFontSize(12);
      context.MoveTo(lineX, lineY);

      foreach (string line in shape.Lines) {
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
