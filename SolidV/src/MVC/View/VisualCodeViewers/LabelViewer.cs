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
  public class LabelViewer : ShapeViewer
  {
    public LabelViewer() { }

    public override void DrawShape(IView<Context, Model> view, Context context, Shape shape)
    {
      Label lb = (Label)shape;
      if (lb == null) return;
      ArrowShape arrow = (ArrowShape)lb.Parent;

      if (view.Mode == ViewMode.Render) {
        context.Save();
        context.Matrix = shape.Matrix;

        PointD pointFrom = (arrow.FromGlue != null) ?
          arrow.FromGlue.TransformPointToGlobal(arrow.FromGlue.Center, context) : arrow.From.Center;
        pointFrom = lb.TransformPointToLocal(pointFrom, context);

        PointD pointTo = (arrow.ToGlue != null) ?
          arrow.ToGlue.TransformPointToGlobal(arrow.ToGlue.Center, context) : arrow.To.Center;
        pointTo = lb.TransformPointToLocal(pointTo, context);

        double x = Math.Min(pointFrom.X, pointTo.X) + Math.Abs(pointFrom.X - pointTo.X)/2;
        double y = Math.Min(pointFrom.Y, pointTo.Y) + Math.Abs(pointFrom.Y - pointTo.Y)/2;

        PointD p = new PointD(x, y);

        double titleWidth = context.TextExtents(lb.TextLabel).Width;
        double titleHeight = context.TextExtents(lb.TextLabel).Height;

        double titleX = p.X - titleWidth / 2;
        double titleY = p.Y - titleHeight * 2;

        double titleBoxWidth = 2 * titleWidth;
        double titleBoxHeight = 3 * titleHeight;

        double toRadians = Math.PI / 180;
        // arc data
        double xCoord, yCoord, radius, arcStart, arcEnd;

        context.SetSourceRGB(0, 0, 0);
        context.NewPath();

        // upper right
        radius = 5;
        xCoord = p.X - radius + titleBoxWidth;
        yCoord = -titleBoxHeight / 2 + p.Y + radius;
        arcStart = 270 * toRadians;
        arcEnd = 0;
        context.Arc(xCoord, yCoord, radius, arcStart, arcEnd);

        // lower right
        radius = 5;
        xCoord = p.X - radius + titleBoxWidth;
        yCoord = p.Y;
        arcStart = 0;
        arcEnd = 90 * toRadians;
        context.Arc(xCoord, yCoord, radius, arcStart, arcEnd);

        // lower left
        radius = 5;
        xCoord = p.X + radius - titleBoxWidth / 2;
        yCoord = p.Y;
        arcStart = 90 * toRadians;
        arcEnd = 180 * toRadians;
        context.Arc(xCoord, yCoord, radius, arcStart, arcEnd);

        // upper left
        radius = 5;
        xCoord = p.X + radius - titleBoxWidth / 2;
        yCoord = -titleBoxHeight / 2 + p.Y + radius;
        arcStart = 180 * toRadians;
        arcEnd = 270 * toRadians;
        context.Arc(xCoord, yCoord, radius, arcStart, arcEnd);

        context.ClosePath();
        context.SetSourceRGB(.4, .4, .4);
        context.Stroke();

        context.MoveTo(p);
        context.SetFontSize(12);
        context.SetSourceRGB(.2, .2, .2);
        context.SelectFontFace("Arial", FontSlant.Normal, FontWeight.Bold) ;
        context.ShowText(lb.TextLabel);

        context.Restore();
      }
    }
  }
}

