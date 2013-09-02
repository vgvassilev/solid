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
  public class PolylineArrowShapeViewer : ShapeViewer
  {
    public PolylineArrowShapeViewer()
    {
    }
    
    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      PolylineArrowShape sh = (PolylineArrowShape)shape;
      context.MoveTo(sh.From.Location);
      foreach (Distance d in sh.Points) {
        context.RelLineTo(d);
      }
      context.ArrowLineTo(sh.To.Location, ArrowKinds.TriangleRoundArrow, null);

      if (view.Mode == ViewMode.Render) {
        //context.Color = shape.Style.FillColor;
        //context.FillPreserve();
        context.Pattern = sh.Style.Border;
        context.Stroke();
      }
    }
    
  }
}
