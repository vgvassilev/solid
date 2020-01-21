/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;
using SolidV.Cairo;

namespace SolidV.MVC
{
  public class HeatMapShapeViewer : ShapeViewer
  {
    public HeatMapShapeViewer()
    {
    }
    
    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      HeatMapShape hms = (HeatMapShape) shape;
      context.Rectangle(hms.Rectangle);

      if (view.Mode == ViewMode.Render) {
        foreach (var item in hms.HeatMap) {
        }
        context.Save();
        context.Scale(shape.Width, shape.Height);
        context.NewPath();
        context.Arc(shape.Location.X / shape.Width + 0.5, shape.Location.Y/shape.Height + 0.5, 0.5, 0, 2 * Math.PI);
        context.ClosePath();
        context.Restore();

//        context.SetSource(shape.Style.Fill);
//        context.FillPreserve();
//        context.SetSource(shape.Style.Border);
//
//        context.Stroke();

//        context.NewPath();
//        context.MoveTo(titleX, titleY);
//        context.ShowText(es.Title);
        //context.ClosePath();
      }
    }
  }
}
