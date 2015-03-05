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
  public class BezierCurvedArrowShapeViewer : ShapeViewer
  {
    public BezierCurvedArrowShapeViewer() {}
    
    protected void RenderArrowPoints(SolidV.MVC.IView<Context, SolidV.MVC.Model> view, Context context,
      double x, double y, double connectorSize, double extra_thickness = 0)
    {
      PointD[] pts = new PointD[] {
        new PointD(x - (connectorSize + 1.0) - extra_thickness, y + (connectorSize / 1.5) + extra_thickness),
        new PointD(x + 1.0f + extra_thickness, y),
        new PointD(x - (connectorSize + 1.0) - extra_thickness, y - (connectorSize / 1.5) - extra_thickness)
      };
      
      context.MoveTo(pts[0]);
      context.LineTo(pts[1]);
      context.LineTo(pts[2]);
      
      if (view.Mode == ViewMode.Render) {
        context.ClosePath();
        context.Fill();
      }
    }
    
    protected void GetArrowLinePoints(Context context, double x1, double y1, double x2, double y2,
      double lineWidth, double connectorSize, double extraThickness = 0)
    {
      double widthX  = (x2 - x1);
      double lengthX = Math.Max(60, Math.Abs(widthX / 2));
      double lengthY = 0;
      
      if (widthX < 120)
        lengthX = 60;
      
      double yB = ((y1 + y2) / 2) + lengthY;
      double yC = y2 + yB;
      double xC = (x1 + x2) / 2;
      double xA = x1 + lengthX;
      double xB = x2 - lengthX;
      
      CubicBezier bezier = new CubicBezier();
      bezier.ControlPoints.Add(new PointD(x1, y1));
      bezier.ControlPoints.Add(new PointD(xA, y1));
      bezier.ControlPoints.Add(new PointD(xB, y2));
      bezier.ControlPoints.Add(new PointD(x2 - connectorSize - extraThickness, y2));
      
      double t = 1;
      double yA = (yB * t) + (yC * (1 - t));
      
      if (widthX <= 12) {
        bezier.ControlPoints.Insert(2, new PointD(xB, yA));
        bezier.ControlPoints.Insert(2, new PointD(xC, yA));
        bezier.ControlPoints.Insert(2, new PointD(xA, yA));
      }
      
      List<PointD> points = bezier.Interpolate(0.25);
      
      PointD[] angles  = new PointD[points.Count - 1];
      double[] lengths = new double[points.Count - 1];
      double totalLength = 0;
      
      points.Add(points[points.Count - 1]);
      for (int i = 0; i < points.Count - 2; i++) {
        PointD pt1 = points[i];
        PointD pt2 = points[i + 1];
        PointD pt3 = points[i + 2];
        PointD delta = pt2.Substract(pt1).Addition(pt3.Substract(pt2));
        double length = delta.Length();
        
        if (length <= 1.0) {
          points.RemoveAt(i);
          i--;
          continue;
        }
        
        lengths[i] = length;
        totalLength += length;
        angles[i] = delta.Division(length);
      }
      
      double startWidth = extraThickness + 0.75;
      double endWidth = extraThickness + (connectorSize / 3.5);
      double currentLength = 0;
      List<PointD> newPoints = new List<PointD>();
      newPoints.Add(points[0]);
      
      for (int i = 0; i < points.Count - 2; i++) {
        PointD angle = angles[i];
        PointD point = points[i + 1];
        double length = lengths[i];
        double width = (((currentLength * (endWidth - startWidth)) / totalLength) + startWidth);
        double angleX = angle.X * width;
        double angleY = angle.Y * width;
        double newLength = currentLength + length;
        PointD pt1 = new PointD(point.X - angleY, point.Y + angleX);
        PointD pt2 = new PointD(point.X + angleY, point.Y - angleX);
        
        if (Math.Abs(newPoints[newPoints.Count - 1].X - pt1.X) > 1.0 ||
            Math.Abs(newPoints[newPoints.Count - 1].Y - pt1.Y) > 1.0)
          newPoints.Add(pt1);
        
        if (Math.Abs(newPoints[0].X - pt2.X) > 1.0 ||
            Math.Abs(newPoints[0].Y - pt2.Y) > 1.0)
          newPoints.Insert(0, pt2);
        
        currentLength = newLength;
      }
      
      context.LineWidth = lineWidth;
      context.MoveTo(newPoints[0]);
      for (int i = 1; i < newPoints.Count; ++i)
        context.LineTo(newPoints[i]);
      context.ClosePath();
    }
    
    public override void DrawItem(SolidV.MVC.IView<Context, SolidV.MVC.Model> view,
      Context context, object shape)
    {
      BezierCurvedArrowShape sh = (BezierCurvedArrowShape)shape;
      
      if (view.Mode == ViewMode.Render)
        context.NewPath();
      
      context.Save();
      
      PointD from = new PointD();
      if (sh.From != null) {
        from = (sh.FromGlue != null) ?
          sh.FromGlue.TransformPointToGlobal(sh.FromGlue.Center, context) : sh.From.Center;
        from = sh.TransformPointToLocal(from, context);
      }
      
      PointD to = new PointD();
      if (sh.To != null) {
        to = (sh.ToGlue != null) ?
          sh.ToGlue.TransformPointToGlobal(sh.ToGlue.Center, context) : sh.To.Center;
        to = sh.TransformPointToLocal(to, context);
      }
      
      GetArrowLinePoints(context, from.X, from.Y, to.X, to.Y, sh.LineWidth, sh.ConnectorSize,
                         view.Mode == ViewMode.Select ? sh.ExtraThickness : sh.LineWidth);
      
      if (view.Mode == ViewMode.Render) 
        context.Fill();
      
      RenderArrowPoints(view, context, to.X, to.Y, sh.ConnectorSize,
                        view.Mode == ViewMode.Select ? sh.ExtraThickness : sh.LineWidth);
      
      context.Restore();
    }
  }
  
  internal class CubicBezier
  {
    // Bezier control points
    private List<PointD> controlPoints = new List<PointD>();
    public List<PointD> ControlPoints {
      get { return controlPoints; }
      set { controlPoints = value; }
    }
    
    public CubicBezier() {}
    
    /// <summary>
    /// Interpolate a Bezier with a given precision
    /// </summary>
    /// <param name="precision">precission</param>
    public List<PointD> Interpolate(double precision)
    {
      List<PointD> points = new List<PointD>();
      
      points.Add(ControlPoints[0]);
      for (int i = 0; i <= ControlPoints.Count - 4; i += 3)
        InterpolateSegment(i, precision, points);
      
      return points;
    }
    
    private PointD DeCasteljau(int i, double t)
    {
      PointD P0 = ControlPoints[i];
      PointD P1 = ControlPoints[i+1];
      PointD P2 = ControlPoints[i+2];
      //
      P0 = P0.Lerp(P1, t);
      P1 = P1.Lerp(P2, t);
      P2 = P2.Lerp(ControlPoints[i+3], t);
      //
      P0 = P0.Lerp(P1, t);
      P1 = P1.Lerp(P2, t);
      //
      return P0.Lerp(P1, t);
    }
    
    /// <summary>
    /// Interpolate a Bezier segment with a given precision
    /// </summary>
    /// <param name="i">Index first point of Bezier segment</param>
    /// <param name="precision">precision</param>
    /// <param name="points">List of points where to add new points for the segment</param>
    private void InterpolateSegment(int i, double precision, List<PointD> points)
    {
      PointD P0, P1, P2, P3;
      
      double maxMedianLengthSqr = Math.Max(
        ControlPoints[i+1].Median(ControlPoints[i], ControlPoints[i+2]).LengthSqr(),
        ControlPoints[i+2].Median(ControlPoints[i+1], ControlPoints[i+3]).LengthSqr()
      );
      
      if (maxMedianLengthSqr <= 0.25 * precision*precision) {
        points.Add(ControlPoints[i+3]);
        return;
      }
      
      double cnt = Math.Min((int)Math.Sqrt(Math.Sqrt(maxMedianLengthSqr)/precision) + 3, 1500);
      
      double d = 1/cnt;
      P0 = ControlPoints[i];
      P1 = DeCasteljau(i, d);
      points.Add(P1);
      P2 = DeCasteljau(i, 2*d);
      points.Add(P2);
      P3 = DeCasteljau(i, 3*d);
      points.Add(P3);
      
      P0 = P1.Substract(P0);
      P1 = P2.Substract(P1);
      P2 = P3.Substract(P2);
      //
      P0 = P1.Substract(P0);
      P1 = P2.Substract(P1);
      //
      P0 = P1.Substract(P0);
      
      for (int j = 4; j <= (int)cnt; j++) {
        P1 = P1.Addition(P0);
        P2 = P2.Addition(P1);
        P3 = P3.Addition(P2);
        points.Add(P3);
      }
    }
  }

}

