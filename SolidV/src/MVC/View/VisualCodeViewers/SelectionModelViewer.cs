/*
 * $Id$
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
      ViewMode oldViewMode = view.Mode;
      view.Mode = ViewMode.Select;
      foreach (Shape shape in ((SelectionModel)item).Selected) {
        context.Save();

        view.DrawItem(context, shape);

        // Mono cairo bug: return x1,y2-x2,y2 in rectangle instead x1,y1;w,h
        // Uncomment r.X etc. if mono bug is removed
        Rectangle r = context.StrokeExtents();
        Rectangle r1 = context.FillExtents();
        r = new Rectangle(
          Math.Min(r.X, r1.X), Math.Min(r.Y, r1.Y),
          Math.Max(/*r.X+*/r.Width, /*r1.X+*/r1.Width) - Math.Min(r.X, r1.X),
          Math.Max(/*r.Y+*/r.Height, /*r1.Y+*/r1.Height) - Math.Min(r.Y, r1.Y));
        context.NewPath();

        context.SetDash(new double[]{1.0, 1.0}, 0);
        context.Operator = Operator.Over;

        context.Rectangle(r.X - 5, r.Y - 5, r.Width + 10, r.Height + 10);
        context.Stroke();

        context.Restore();
      }
      view.Mode = oldViewMode;
    }
  }
}
