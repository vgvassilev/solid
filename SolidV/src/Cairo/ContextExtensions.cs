/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.Cairo
{
  using Cairo = global::Cairo;
  public static class ContextExtensions
  {
    //public ContextExtensions(IntPtr state): base(state) {}
    //public ContextExtensions(Surface surface): base(surface) {}

    private static Matrix IdentityMatrix = new Matrix();

    private static void DrawArrow(Context context, DrawArrowDelegate arrow, double angle, 
                                  Matrix matrix) {
      if (arrow == null) return;

      context.Save();
      context.Rotate(angle);
      context.Transform(matrix);
      arrow(context);
      context.Restore();
    }

    public static void ArrowLineTo(this Context context, double x, double y, 
                                   DrawArrowDelegate headArrow, DrawArrowDelegate tailArrow, 
                                   Matrix headMatrix, Matrix tailMatrix) {
      double headAngle = Math.PI/2 - Math.Atan2(context.CurrentPoint.X - x, 
                                                context.CurrentPoint.Y - y);
      double tailAngle = headAngle + Math.PI;
      DrawArrow(context, tailArrow, tailAngle, tailMatrix);
      context.LineTo(x, y);
      DrawArrow(context, headArrow, headAngle, headMatrix);
    }

    public static void ArrowLineTo(this Context context, double x, double y, 
                                   DrawArrowDelegate headArrow, DrawArrowDelegate tailArrow, 
                                   Matrix headMatrix) {
      ArrowLineTo(context, x, y, headArrow, tailArrow, headMatrix, IdentityMatrix);
    }

    public static void ArrowLineTo(this Context context, double x, double y, 
                                   DrawArrowDelegate headArrow, DrawArrowDelegate tailArrow) {
      ArrowLineTo(context, x, y, headArrow, tailArrow, IdentityMatrix, IdentityMatrix);
    }

    public static void ArrowLineTo(this Context context, PointD p, DrawArrowDelegate headArrow, 
                                   DrawArrowDelegate tailArrow, Matrix headMatrix, 
                                   Matrix tailMatrix) {
      ArrowLineTo(context, p.X, p.Y, headArrow, tailArrow, headMatrix, tailMatrix);
    }
    
    public static void ArrowLineTo(this Context context, PointD p, DrawArrowDelegate headArrow, 
                                   DrawArrowDelegate tailArrow, Matrix headMatrix) {
      ArrowLineTo(context, p.X, p.Y, headArrow, tailArrow, headMatrix, IdentityMatrix);
    }
    
    public static void ArrowLineTo(this Context context, PointD p, DrawArrowDelegate headArrow, 
                                   DrawArrowDelegate tailArrow) {
      ArrowLineTo(context, p.X, p.Y, headArrow, tailArrow, IdentityMatrix, IdentityMatrix);
    }

//

    public static void ArrowCurveTo(this Context context, 
                                    double x1, double y1, double x2, double y2, double x3, double y3,
                                    DrawArrowDelegate headArrow, DrawArrowDelegate tailArrow, 
                                    Matrix headMatrix, Matrix tailMatrix)
    {
      double headAngle = Math.PI/2 - Math.Atan2(x2 - x3, y2 - y3);
      double tailAngle = Math.PI/2 - Math.Atan2(x1 - context.CurrentPoint.X, 
                                                y1 - context.CurrentPoint.Y);
      DrawArrow(context, tailArrow, tailAngle, tailMatrix);
      context.CurveTo(x1, y1, x2, y2, x3, y3);
      DrawArrow(context, headArrow, headAngle, headMatrix);
    }

    public static void ArrowCurveTo(this Context context,
                                    double x1, double y1, double x2, double y2, double x3, double y3,
                                    DrawArrowDelegate headArrow, DrawArrowDelegate tailArrow, 
                                    Matrix headMatrix) {
      ArrowCurveTo(context, x1, y1, x2, y2, x3, y3, headArrow, tailArrow, headMatrix, 
                   IdentityMatrix);
    }

    public static void ArrowCurveTo(this Context context, 
                                    double x1, double y1, double x2, double y2, double x3, double y3,
                                    DrawArrowDelegate headArrow, DrawArrowDelegate tailArrow) {
      ArrowCurveTo(context, x1, y1, x2, y2, x3, y3, headArrow, tailArrow, IdentityMatrix, 
                   IdentityMatrix);
    }

    public static void ArrowCurveTo(this Context context, PointD p1, PointD p2, PointD p3, 
                                    DrawArrowDelegate headArrow, DrawArrowDelegate tailArrow) {
      ArrowCurveTo(context, p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, headArrow, tailArrow, 
                   IdentityMatrix, IdentityMatrix);
    }

    public static Cairo.Color ToCairoColor (this Gdk.Color color)
    {
      return new Cairo.Color ((double)color.Red / ushort.MaxValue, (double)color.Green / ushort.MaxValue, (double)color.Blue / ushort.MaxValue);
    }
  }
}
