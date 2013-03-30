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
      string[] lines = shape.BlockText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
      context.Rectangle(shape.Rectangle);
      context.Color = shape.Style.FillColor;
      context.FillPreserve();
      context.Color = shape.Style.BorderColor;
      double lineY = shape.Rectangle.Y + 20;
      double lineX = shape.Rectangle.X + 10;
      foreach (string line in lines) {
        if (line.Length > 28)
          context.ShowText(line.Substring(0, 27));
        else
          context.ShowText(line);

        context.MoveTo(lineX, lineY);
        lineY += 20;
      }
      context.Stroke();
    }
  }
}
