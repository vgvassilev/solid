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
  public static class PointExtensions
  {
    public static PointD Addition(this PointD p1, PointD p2) {
      return new PointD(p1.X+p2.X, p1.Y+p2.Y);
    }
    
    public static void AdditionAssignment(this PointD p1, PointD p2) {
      p1.X += p2.X;
      p1.Y += p2.Y;
    }
    
    public static PointD Substract(this PointD p1, PointD p2) {
      return new PointD(p1.X-p2.X, p1.Y-p2.Y);
    }
    
    public static void SubstractAssignment(this PointD p1, PointD p2) {
      p1.X -= p2.X;
      p1.Y -= p2.Y;
    }
    
    public static double Dot(this PointD p1, PointD p2) {
      return p1.X*p2.X + p1.Y*p2.Y;
    }
    
    public static PointD Multiply(this PointD p, double c) {
      return new PointD(p.X*c, p.Y*c);
    }
    
    public static void MultiplyAssignment(this PointD p, double c) {
      p.X *= c;
      p.Y *= c;
    }
    
    public static PointD Multiply(this double c, PointD p) {
      return new PointD(c*p.X, c*p.Y);
    }
    
    public static PointD Division(this PointD p, double c) {
      return new PointD(p.X/c, p.Y/c);
    }
    
    public static void DivisionAssignment(this PointD p, double c) {
      p.X /= c;
      p.Y /= c;
    }
    
    public static double Length(this PointD p) {
      return System.Math.Sqrt(p.X*p.X + p.Y*p.Y);
    }
    
    public static double LengthSqr(this PointD p) {
      return p.X*p.X + p.Y*p.Y;
    }
    
    public static PointD Lerp(this PointD p1, PointD p2, double t) {
      return new PointD(p1.X + (p2.X-p1.X)*t, p1.Y + (p2.Y-p1.Y)*t);
    }
    
    public static PointD Median(this PointD p, PointD p1, PointD p2) {
      return new PointD((p1.X+p2.X)*0.5 - p.X, (p1.Y+p2.Y)*0.5 - p.Y);
    }
  }
}
