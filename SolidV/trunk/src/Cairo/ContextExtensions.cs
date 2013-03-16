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
    
   private static void DrawArrowHead(Context context) {
      //TODO: Use property for current arrow head type
      context.RelMoveTo(1, 0);
      context.RelLineTo(-1, 0);
      context.RelLineTo(0, 1);
    }
    
    private static void DrawArrowTail(Context context) {
      //TODO: Use property for current arrow tail type
      //RelMoveTo(1, 0);
      //RelLineTo(-1, 0);
      //RelLineTo(0, 1);
    }

    public static void ArrowLineTo(this Context context, double x, double y) {
      DrawArrowTail(context);
      context.LineTo(x, y);
      DrawArrowHead(context);
    }

    public static void ArrowLineTo(this Context context, PointD p) {
      DrawArrowTail(context);
      context.LineTo(p);
      DrawArrowHead(context);
    }
    
    public static void ArrowCurveTo(this Context context, double x1, double y1, double x2, double y2, double x3, double y3)
    {
      DrawArrowTail(context);
      context.CurveTo(x1, y1, x2, y2, x3, y3);
      DrawArrowHead(context);
    }

    public static void ArrowCurveTo(this Context context, PointD p1, PointD p2, PointD p3) {
      DrawArrowTail(context);
      context.CurveTo(p1, p2, p3);
      DrawArrowHead(context);
    }

    public static void RelArrowLineTo(this Context context, double x, double y) {
      DrawArrowTail(context);
      context.RelLineTo(x, y);
      DrawArrowHead(context);
    }
    
    public static void RelArrowLineTo(this Context context, Distance d) {
      DrawArrowTail(context);
      context.RelLineTo(d);
      DrawArrowHead(context);
    }
    
    public static void RelArrowCurveTo(this Context context, double dx1, double dy1, double dx2, double dy2, double dx3, double dy3)
    {
      DrawArrowTail(context);
      context.RelCurveTo(dx1, dy1, dx2, dy2, dx3, dy3);
      DrawArrowHead(context);
    }
    
    public static void RelArrowCurveTo(this Context context, Distance d1, Distance d2, Distance d3) {
      DrawArrowTail(context);
      context.RelCurveTo(d1, d2, d3);
      DrawArrowHead(context);
    }
    
  }
}
