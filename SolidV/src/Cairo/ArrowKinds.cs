/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.Cairo
{
  public delegate void DrawArrowDelegate(Context context);

  /// <summary>
  /// Set of predefined arrows, ready for use in the visualization.
  /// </summary>
  public static class ArrowKinds
  {
    public static void NoArrow(Context context) {
    }
    
    public static void DefaultArrow(Context context) {
      context.RelMoveTo(10, 10);
      context.RelLineTo(-10, -10);
      context.RelLineTo(10, -10);
      context.RelMoveTo(-10, 10);
    }

    public static void TriangleArrow(Context context) {
      context.RelLineTo(10, 10);
      context.RelLineTo(0, -20);
      context.RelLineTo(-10, 10);
      //context.ClosePath();
    }
    
    public static void TriangleRoundArrow(Context context) {
      context.RelLineTo(10, 10);
      context.RelLineTo(-3, -6);
      context.RelLineTo(0, -8);
      context.RelLineTo(3, -6);
      context.RelLineTo(-10, 10);
      //context.ClosePath();
    }
    
    public static void CircleArrow(Context context) {
      PointD cp = context.CurrentPoint;
      context.NewSubPath();
      context.Arc(cp.X + 10, cp.Y, 10, 0, 2 * Math.PI);
      context.ClosePath();
      context.MoveTo(cp);
    }
    
    public static void DiamondArrow(Context context) {
      context.RelLineTo(10, 10);
      context.RelLineTo(10, -10);
      context.RelLineTo(-10, -10);
      context.RelLineTo(-10, 10);
      context.ClosePath();
    }
    
    public static void SquareArrow(Context context) {
      context.RelLineTo(0, 5);
      context.RelLineTo(10, 0);
      context.RelLineTo(0, -10);
      context.RelLineTo(-10, 0);
      context.RelLineTo(0, 5);
      context.ClosePath();
    }
    public static void SharpArrow(Context context) {
      context.RelLineTo(10, 2);
      context.RelLineTo(0, -4);
      context.RelLineTo(-10, 2);
      //context.ClosePath();
    }
    
  }
}
