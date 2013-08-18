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
  public class PolylineArrowShapeViewer : ShapeViewer
  {
    public PolylineArrowShapeViewer()
    {
    }
    
    public override void DrawItem(IView<Context, Model> view, Context context, object item)
    {
      PolylineArrowShape shape = (PolylineArrowShape)item;
      context.MoveTo(shape.From.Location);
      foreach (Distance d in shape.Points) {
        context.RelLineTo(d);
      }
      context.ArrowLineTo(shape.To.Location, ArrowKinds.TriangleRoundArrow, null);
      //context.Color = shape.Style.FillColor;
      //context.FillPreserve();
      context.Color = shape.Style.BorderColor;
      context.Stroke();
    }
    
  }
}
