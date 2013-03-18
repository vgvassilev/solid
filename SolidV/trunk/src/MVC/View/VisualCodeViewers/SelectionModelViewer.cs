/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  public class SelectionModelViewer : Viewer<Context, Model>
  {
    public SelectionModelViewer()
    {
    }
    
    public override void DrawItem(IView<Context, Model> view, Context context, object item)
    {
      foreach (Shape shape in ((SelectionModel)item).Selected) {
        context.Rectangle(shape.Rectangle.X - 5, shape.Rectangle.Y - 5, 
                          shape.Rectangle.Width + 10, shape.Rectangle.Height + 10);
        //context.so
        context.SetDash(new double[]{1.0, 1.0}, 0);
        context.Operator = Operator.Over;
        context.Stroke();
      }
    }
  }
}
