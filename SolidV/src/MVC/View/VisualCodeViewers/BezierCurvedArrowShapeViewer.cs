/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;
using Cairo;

namespace SolidV.MVC
{
  public class BezierCurvedArrowShapeViewer : ShapeViewer
  {
    double connectorSize = 7;
    double extraThickness = 2;
    double lineWidth = 3;

    public BezierCurvedArrowShapeViewer() { }

    void RenderArrowPoints(SolidV.MVC.IView<Context, SolidV.MVC.Model> view, Context c, double x, 
                           double y, double extra_thickness = 0)
    {
      PointD[] pts = new PointD[] {
        new PointD(x - (connectorSize + 1.0f) - extra_thickness, y + (connectorSize / 1.5f) +
                   extra_thickness),
        new PointD(x + 1.0f + extra_thickness, y),
        new PointD(x - (connectorSize + 1.0f) - extra_thickness, y - (connectorSize / 1.5f) -
                   extra_thickness)};
      
      c.MoveTo(pts[0].X, pts [0].Y);
      c.LineTo(pts[1].X, pts [1].Y);
      c.LineTo(pts[2].X, pts [2].Y);
      
      if (view.Mode == ViewMode.Render) {
        c.ClosePath();
        c.Fill();
      }
    }

    void GetArrowLinePoints(Context c, double x1, double y1, double x2, double y2,
                            double lineWidth, double extraThickness = 0)
    {
      var widthX  = (x2 - x1);
      var lengthX = Math.Max(60, Math.Abs(widthX / 2));
      var lengthY = 0;

      if (widthX < 120)
        lengthX = 60;

      var yB = ((y1 + y2) / 2) + lengthY;
      var yC = y2 + yB;
      var xC = (x1 + x2) / 2;
      var xA = x1 + lengthX;
      var xB = x2 - lengthX;

      CubicBezierSegments bezier = new CubicBezierSegments();
      bezier.AddBezierPoint(new Vector3(x1, y1, 0));
      bezier.AddBezierPoint(new Vector3(xA, y1, 0));
      bezier.AddBezierPoint(new Vector3(xB, y2, 0));
      bezier.AddBezierPoint(new Vector3(x2 - connectorSize - extraThickness, y2, 0));
      
      var t  = 1.0f;//Math.Min(1, Math.Max(0, (widthX - 30) / 60.0f));
      var yA = (yB * t) + (yC * (1 - t));
      
      if (widthX <= 12) {
        bezier.InsertBezierPoint(2, new Vector3(xB, yA, 0));
        bezier.InsertBezierPoint(2, new Vector3(xC, yA, 0));
        bezier.InsertBezierPoint(2, new Vector3(xA, yA, 0));
      }
      
      List<Vector3> points = bezier.Flatten(0.25);
      
      var angles  = new Vector3[points.Count - 1];
      var lengths = new double[points.Count - 1];
      double totalLength = 0;
      //centerX = 0;
      //centerY = 0;

      points.Add(points[points.Count - 1]);
      for (int i = 0; i < points.Count - 2; i++) {
        var pt1 = points[i];
        var pt2 = points[i + 1];
        var pt3 = points[i + 2];
        var deltaX = (pt2.x - pt1.x) + (pt3.x - pt2.x);
        var deltaY = (pt2.y - pt1.y) + (pt3.y - pt2.y);
        var length = Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));

        if (length <= 1.0f) {
          points.RemoveAt(i);
          i--;
          continue;
        }

        lengths[i] = length;
        totalLength += length;
        angles[i].x = deltaX / length;
        angles[i].y = deltaY / length;
      }
      
      double midLength = (totalLength / 2.0f);// * 0.75f;
      double startWidth = extraThickness + 0.75f;
      double endWidth = extraThickness + (connectorSize / 3.5f);
      double currentLength = 0;
      var newPoints = new List<Vector3>();
      newPoints.Add(points[0]);

      for (int i = 0; i < points.Count - 2; i++) {
        var angle = angles[i];
        var point = points[i + 1];
        var length = lengths[i];
        var width = (((currentLength * (endWidth - startWidth)) / totalLength) + startWidth);
        var angleX = angle.x * width;
        var angleY = angle.y * width;
        var newLength = currentLength + length;
        var pt1 = new Vector3(point.x - angleY, point.y + angleX, 0);
        var pt2 = new Vector3(point.x + angleY, point.y - angleX, 0);

        if (Math.Abs(newPoints[newPoints.Count - 1].x - pt1.x) > 1.0f ||
            Math.Abs(newPoints[newPoints.Count - 1].y - pt1.y) > 1.0f)
          newPoints.Add(pt1);

        if (Math.Abs(newPoints[0].x - pt2.x) > 1.0f || Math.Abs(newPoints[0].y - pt2.y) > 1.0f)
          newPoints.Insert(0, pt2);
        
        currentLength = newLength;
      }
      
      c.LineWidth = lineWidth;
      c.MoveTo(newPoints[0].x, newPoints[0].y);

      for (int i = 1; i < newPoints.Count; ++i)
        c.LineTo(newPoints[i].x, newPoints[i].y);

      c.ClosePath();
    }
    
    public override void DrawItem(SolidV.MVC.IView<Context, SolidV.MVC.Model> view, 
                                  Context context, object shape)
    {
      BezierCurvedArrowShape bArrow = (BezierCurvedArrowShape)shape;
      
      if (view.Mode == ViewMode.Render)
        context.NewPath();
      
      context.Save();

      PointD from = new PointD();
      if (bArrow.From != null) {
        from = (bArrow.FromGlue != null) ?
          bArrow.FromGlue.TransformPointToGlobal(bArrow.FromGlue.Center, context) : bArrow.From.Center;
        from = bArrow.TransformPointToLocal(from, context);
      }

      PointD to = new PointD();
      if (bArrow.To != null) {
        to = (bArrow.ToGlue != null) ?
          bArrow.ToGlue.TransformPointToGlobal(bArrow.ToGlue.Center, context) : bArrow.To.Center;
        to = bArrow.TransformPointToLocal(to, context);
      }

      GetArrowLinePoints(context, from.X, from.Y, to.X, to.Y,
                         view.Mode == ViewMode.Select ? extraThickness : lineWidth);

      if (view.Mode == ViewMode.Render) 
        context.Fill();

      RenderArrowPoints(view, context, to.X/*-m_connectorHeight/2*/, to.Y,
                        view.Mode == ViewMode.Select ? extraThickness : lineWidth);
      
      context.Restore();
    }
  }

  public class CubicBezierSegments
  {
    /// <summary>
    /// Count of bezier control points
    /// </summary>
    private int BezierPointCount
    {
      get { return bezierControlPoints.Count; }
    }

    // Bezier points
    public List<Vector3> bezierControlPoints = new List<Vector3>();
    
    public CubicBezierSegments() {}
    
    /// <summary>
    /// Flatten bezier with a given resolution
    /// </summary>
    /// <param name="tolerance">tolerance</param>
    public List<Vector3> Flatten(double tolerance)
    {
      List<Vector3> points = new List<Vector3>();
      
      // First point
      Vector3 vector = GetBezierPoint(0);
      points.Add(new Vector3(vector.x, vector.y, 0));
      
      int last = this.BezierPointCount - 4;
      
      if (0 <= last) {
        // Tolerance needs to be non-zero positive
        // if (tolerance < DoubleUtil.DBL_EPSILON)
        //    tolerance = DoubleUtil.DBL_EPSILON;
        
        // Flatten individual segments
        for (int i = 0; i <= last; i += 3)
          FlattenSegment(i, tolerance, points);
      }
      /*
        //convert from himetric to Avalon
        for (int x = 0; x < points.Count; x++) {
          Point p = points[x];
          p.X *= StrokeCollectionSerializer.HimetricToAvalonMultiplier;
          p.Y *= StrokeCollectionSerializer.HimetricToAvalonMultiplier;
          points[x] = p;
        }
      */
      return points;
    }
    
    /// <summary>
    /// Add Bezier point to the output buffer
    /// </summary>
    /// <param name="point">In: The point to add</param>
    public void AddBezierPoint(Vector3 point)
    {
      bezierControlPoints.Add(point);
    }
    
    public void InsertBezierPoint(int id, Vector3 point)
    {
      bezierControlPoints.Insert(id, point);
    }
    
    private Vector3 DeCasteljau(int iFirst, double t)
    {
      // Using the de Casteljau algorithm.  See "Curves & Surfaces for Computer
      // Aided Design" for the theory
      double s = 1.0f - t;
      
      // Level 1
      Vector3 Q0 = s * GetBezierPoint(iFirst) + t * GetBezierPoint(iFirst + 1);
      Vector3 Q1 = s * GetBezierPoint(iFirst + 1) + t * GetBezierPoint(iFirst + 2);
      Vector3 Q2 = s * GetBezierPoint(iFirst + 2) + t * GetBezierPoint(iFirst + 3);
      
      // Level 2
      Q0 = s * Q0 + t * Q1;
      Q1 = s * Q1 + t * Q2;
      
      // Level 3
      return s * Q0 + t * Q1;
    }
    
    /// <summary>
    ///  Flatten a Bezier segment within given resolution
    /// </summary>
    /// <param name="iFirst">Index of Bezier segment's first point</param>
    /// <param name="tolerance">tolerance</param>
    /// <param name="points"></param>
    /// <returns></returns>
    private void FlattenSegment(int iFirst, double tolerance, List<Vector3> points)
    {
      // We use forward differencing.  It is much faster than subdivision
      int i, k;
      int nPoints = 1;
      Vector3[] Q = new Vector3[4];
      
      // The number of points is determined by the "curvedness" of this segment,
      // which is a heuristic: it's the maximum of the 2 medians of the triangles
      // formed by consecutive Bezier points.  Why median? because it is cheaper
      // to compute than height.
      double rCurv = 0;
      
      for (i = checked(iFirst + 1); i <= checked(iFirst + 2); i++) {
        // Get the longer median
        Q[0] = (GetBezierPoint(i - 1) + GetBezierPoint(i + 1)) * 0.5f - GetBezierPoint(i);
        
        double r = Q [0].getLenght ();// Length;
        
        if (r > rCurv)
          rCurv = r;
      }
      
      // Now we look at the ratio between the medain and the error tolerance.
      // the points are collinear then one point - the endpoint - will do.
      // Otherwise, since curvature is roughly inverse proportional
      // to the square of nPoints, we set nPoints to be the square root of this
      // ratio, but not less than 3.
      if (rCurv <= 0.5 * tolerance) { // Flat segment
        Vector3 vector = GetBezierPoint(iFirst + 3);
        points.Add(new Vector3(vector.x, vector.y, 0));
        return;
      }
      
      // Otherwise we'll have at least 3 points
      // Tolerance is assumed to be positive
      nPoints = (int)(Math.Sqrt(rCurv / tolerance)) + 3;
      if (nPoints > 1000)
        nPoints = 1000; // Arbitrary limitation, but...
      
      // Get the first 4 points on the segment in the buffer
      double d = 1.0f / (double) nPoints;
      
      Q[0] = GetBezierPoint(iFirst);
      
      for (i = 1; i <= 3; i++) {
        Q[i] = DeCasteljau(iFirst, i * d);
        points.Add(new Vector3(Q[i].x, Q[i].y, 0));
      }
      
      // Replace points in the buffer with differences of various levels
      for (i = 1; i <= 3; i++)
        for (k = 0; k <= (3 - i); k++)
          Q[k] = Q[k + 1] - Q[k];
      
      // Now generate the rest of the points by forward differencing
      for (i = 4; i <= nPoints; i++) {
        for (k = 1; k <= 3; k++)
          Q[k] += Q[k - 1];
        
        points.Add(new Vector3(Q[3].x, Q[3].y, 0));
      }
    }
    
    /// <summary>
    /// Returns a single bezier control point at index
    /// </summary>
    /// <param name="index">Index</param>
    /// <returns></returns>
    private Vector3 GetBezierPoint(int index)
    {
      return bezierControlPoints[index];
    }
  }

  [Serializable]
  public struct Vector3 {
    
    public double x;
    public double y;
    public double z;
    
    #region Constructors
    public Vector3(double x, double y, double z) {
      this.x = x;
      this.y = y;
      this.z = z;
    }
    
    public Vector3(Vector3 v) {
      this.x = v.x;
      this.y = v.y;
      this.z = v.z;
    }
    #endregion Constructors
    
    #region Setters
    public void set(double x, double y, double z) {
      this.x = x;
      this.y = y;
      this.z = z;
    }
    
    public void set(Vector3 v) {
      this.x = v.x;
      this.y = v.y;
      this.z = v.z;
    }
    #endregion Setters
    
    #region Vector to Vector Operations
    
    public static Vector3 operator+(Vector3 v1) {
      return (new Vector3(+v1.x, +v1.y, +v1.z));
    }
    
    public static Vector3 operator-(Vector3 v1) {
      return (new Vector3(-v1.x, -v1.y, -v1.z));
    }
    
    public static Vector3 operator+(Vector3 v1, Vector3 v2) {
      return (new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z));
    }
    
    public static Vector3 operator-(Vector3 v1, Vector3 v2) {
      return (new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z));
    }
    
    //DOT product
    public static double operator*(Vector3 v1, Vector3 v2) {
      return (v1.x * v2.x + v1.y * v2.y + v1.z * v2.z);
    }
    
    //CROSS product
    public static Vector3 operator^(Vector3 v1, Vector3 v2) {
      return (new Vector3(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x));
    }
    
    #endregion
    
    #region Vector to Scalar Operations
    public static Vector3 operator+(Vector3 v1, double s2) {
      return (new Vector3(v1.x + s2, v1.y + s2, v1.z + s2));
    }
    
    public static Vector3 operator-(Vector3 v1, double s2) {
      return (new Vector3(v1.x - s2, v1.y - s2, v1.z - s2));
    }
    
    public static Vector3 operator*(Vector3 v1, double s2) {
      return (new Vector3(v1.x * s2, v1.y * s2, v1.z * s2));
    }
    
    public static Vector3 operator/(Vector3 v1, double s2) {
      double s = 1.0 / s2;
      return (new Vector3(v1.x * s, v1.y * s, v1.z * s));
    }
    
    public static Vector3 operator+(double s1, Vector3 v2) {
      return v2 + s1;
    }
    
    public static Vector3 operator-(double s1, Vector3 v2) {
      return v2 - s1;
    }
    
    public static Vector3 operator*(double s1, Vector3 v2) {
      return v2 * s1;
    }
    
    public static Vector3 operator/(double s1, Vector3 v2) {
      return v2 / s1;
    }
    
    #endregion
    
    #region Vector methods
    
    public double getLenght() {
      return System.Math.Sqrt(x*x + y*y + z*z);
    }
    
    public double getLenghtSqr() {
      return (x*x + y*y + z*z);
    }
    
    public void normalize() {
      double len = getLenght();
      if (len > 1e-12f) {
        x /= len;
        y /= len;
        z /= len;
      }
    }
    
    public Vector3 getNormalized() {
      Vector3 vec = new Vector3(this.x, this.y, this.z);
      vec.normalize();
      return vec;
    }
    
    #endregion
    
    public double this[int index]
    {
      get {
        switch (index) {
          case 0: { return x; }
          case 1: { return y; }
          case 2: { return z; }
          default: throw new ArgumentException("try to access index outside [0.2]", "index");
        }
      }
      set {
        switch (index) {
          case 0: {x = value; break;}
          case 1: {y = value; break;}
          case 2: {z = value; break;}
          default: throw new ArgumentException("try to access index outside [0.2]", "index");
        }
      }
    }
  }
}

