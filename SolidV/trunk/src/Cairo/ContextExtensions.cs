/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.Cairo
{
  public static class ContextExtensions
  {
    //public ContextExtensions(IntPtr state): base(state) {}
    //public ContextExtensions(Surface surface): base(surface) {}
    
   private static void DrawArrowHead(Context context, double angle, double scale) {
      //TODO: Use param for current arrow head type
      context.Save();
      context.Rotate(angle);
      context.Scale(scale, scale);
      context.RelMoveTo(1, 0);
      context.RelLineTo(-1, 0);
      context.RelLineTo(0, -1);
      //context.RelMoveTo(0, 1);
      context.Restore();
    }
    
    private static void DrawArrowTail(Context context, double angle, double scale) {
      //TODO: Use param for current arrow tail type
      //context.Save();
      //context.Rotate(angle);
      //context.Scale(scale, scale);
      //context.RelMoveTo(1, 0);
      //context.RelLineTo(-1, 0);
      //context.RelLineTo(0, -1);
      //context.RelMoveTo(0, 1);
      //context.Restore();'
    }

    public static void ArrowLineTo(this Context context, double x, double y) {
      double angle = Math.Atan2(context.CurrentPoint.X - x, context.CurrentPoint.Y - y) + Math.PI/4;
      double scale = 10;
      double angle1 = Math.PI - angle;
      double scale1 = scale;
      DrawArrowTail(context, angle, scale);
      context.LineTo(x, y);
      DrawArrowHead(context, angle1, scale1);
    }

    public static void ArrowLineTo(this Context context, PointD p) {
      context.ArrowLineTo(p.X, p.Y);
    }
    
    public static void ArrowCurveTo(this Context context, double x1, double y1, double x2, double y2, double x3, double y3)
    {
      //DrawArrowTail(context);
      context.CurveTo(x1, y1, x2, y2, x3, y3);
      //DrawArrowHead(context);
    }

    public static void ArrowCurveTo(this Context context, PointD p1, PointD p2, PointD p3) {
      //DrawArrowTail(context);
      context.CurveTo(p1, p2, p3);
      //DrawArrowHead(context);
    }

    public static void RelArrowLineTo(this Context context, double x, double y) {
      //DrawArrowTail(context);
      context.RelLineTo(x, y);
      //DrawArrowHead(context);
    }
    
    public static void RelArrowLineTo(this Context context, Distance d) {
      //DrawArrowTail(context);
      context.RelLineTo(d);
      //DrawArrowHead(context);
    }
    
    public static void RelArrowCurveTo(this Context context, double dx1, double dy1, double dx2, double dy2, double dx3, double dy3)
    {
      //DrawArrowTail(context);
      context.RelCurveTo(dx1, dy1, dx2, dy2, dx3, dy3);
      //DrawArrowHead(context);
    }
    
    public static void RelArrowCurveTo(this Context context, Distance d1, Distance d2, Distance d3) {
      //DrawArrowTail(context);
      context.RelCurveTo(d1, d2, d3);
      //DrawArrowHead(context);
    }
    
  }
}
