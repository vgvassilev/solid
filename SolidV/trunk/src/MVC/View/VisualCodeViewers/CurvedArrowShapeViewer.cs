/*
 * $Id:
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
    
    public override void DrawItem(IView<Context, Model> view, Context context, object item)
    {
      PolylineArrowShape shape = (PolylineArrowShape)item;
      context.MoveTo(shape.From.Location);
      // t 0 1 2 3 4 5 6 7 h
      int i = 0;
      while (i < shape.Points.Count - 3) {
        context.RelCurveTo(shape.Points[i], shape.Points[i+1], shape.Points[i+2]);
        i += 3; 
      }
      PointD cp = context.CurrentPoint;
      context.ArrowCurveTo(cp.X + shape.Points[i].Dx, cp.Y + shape.Points[i].Dy, cp.X + shape.Points[i+1].Dx, cp.Y + shape.Points[i+1].Dy, shape.To.Location.X, shape.To.Location.Y, ArrowKinds.DefaultArrow, ArrowKinds.DefaultArrow);
      //context.Color = shape.Style.FillColor;
      //context.FillPreserve();
      context.Color = shape.Style.BorderColor;
      context.Stroke();
    }
    
  }
}
