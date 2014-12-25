/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;
using Cairo;
using SolidV.Cairo;

namespace SolidV.MVC
{
  public class CurvedArrowShapeViewer : ShapeViewer
  {
    public CurvedArrowShapeViewer()
    {
    }
    
    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      PolylineArrowShape sh = (PolylineArrowShape)shape;
      context.MoveTo(sh.From.Location);
      // t 0 1 2 3 4 5 6 7 h
      int i = 0;
      while (i < sh.Points.Count - 3) {
        context.RelCurveTo(sh.Points[i], sh.Points[i+1], sh.Points[i+2]);
        i += 3; 
      }
      PointD cp = context.CurrentPoint;
      context.ArrowCurveTo(cp.X + sh.Points[i].Dx, cp.Y + sh.Points[i].Dy, cp.X + sh.Points[i+1].Dx, cp.Y + sh.Points[i+1].Dy, sh.To.Location.X, sh.To.Location.Y, ArrowKinds.DefaultArrow, ArrowKinds.DefaultArrow);


      if (view.Mode == ViewMode.Render) {
        //context.Color = shape.Style.FillColor;
        //context.FillPreserve();
        context.SetSource(sh.Style.Border);
        context.Stroke();
      }
    }
    
  }
}
