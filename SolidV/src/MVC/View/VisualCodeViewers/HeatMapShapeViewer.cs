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
      HeatMapShape hms = (HeatMapShape)shape;
      context.Rectangle(hms.Rectangle);

      if (view.Mode == ViewMode.Render) {
        context.HeatMap(0, 0, hms.HeatMap, (int)hms.Width, (int)hms.Height,
          hms.BlurFactor, hms.Alpha, hms.ColorScheme);
      }
    }
  }
}
