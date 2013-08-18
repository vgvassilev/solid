/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

using SolidV.Cairo;

namespace SolidV.MVC
{
  public class ArrowShapeViewer : ShapeViewer
  {
    public ArrowShapeViewer()
    {
    }

    public override void DrawItem(IView<Context, Model> view, Context context, object item)
    {
      ArrowShape shape = (ArrowShape)item;
      SolidV.Cairo.DrawArrowDelegate head = SolidV.Cairo.ArrowType.NoArrow;
      SolidV.Cairo.DrawArrowDelegate tail = SolidV.Cairo.ArrowType.NoArrow;

      switch(shape.ArrowKindHead) {
        case ArrowShape.ArrowKinds.Rounded:
          head = SolidV.Cairo.ArrowType.CircleArrow;
          break;
        case ArrowShape.ArrowKinds.Squared:
          head = SolidV.Cairo.ArrowType.SquareArrow;
          break;
      }

      switch(shape.ArrowKindTail) {
        case ArrowShape.ArrowKinds.Rounded:
          tail = SolidV.Cairo.ArrowType.CircleArrow;
          break;
        case ArrowShape.ArrowKinds.Squared:
          tail = SolidV.Cairo.ArrowType.SquareArrow;
          break;
      }

      context.Color = shape.Style.FillColor;
      context.FillPreserve();
      context.Color = shape.Style.BorderColor;
      context.LineWidth = 1.5;

      context.Stroke();
      context.MoveTo(shape.From.Location);
      context.ArrowLineTo(shape.To.Location, head, null);
    }
    
  }
}
