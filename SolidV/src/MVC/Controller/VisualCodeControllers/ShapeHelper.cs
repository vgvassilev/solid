/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Cairo;

namespace SolidV.MVC
{
  /// <summary>
  /// Helper methods for shape model. Use only in controllers and viewers.
  /// </summary>
  public static partial class ShapeHelper
  {
    /// <summary>
    /// Transforms the point to global. Use only in controllers and viewers.
    /// </summary>
    /// <returns>Converted point to global coordinate system (screen).</returns>
    /// <param name="shape">Shape.</param>
    /// <param name="localPoint">Point in shape's local coordinates.</param>
    /// <param name="context">Context with assigned shape matrix.</param>
    public static PointD TransformPointToGlobal(this Shape shape, PointD localPoint, Context context)
    {
      context.Save();
      ApplyAllParenTransforms(shape, context);
      double x = localPoint.X;
      double y = localPoint.Y;
      context.UserToDevice(ref x, ref y);
      context.Restore();
      return new PointD(x,y);
    }
    
    public static PointD TransformPointToLocal(this Shape shape, PointD globalPoint, Context context)
    {
      context.Save();
      ApplyAllParenTransforms(shape, context);
      double x = globalPoint.X;
      double y = globalPoint.Y;
      context.DeviceToUser(ref x, ref y);
      context.Restore();
      return new PointD(x,y);
    }

    public static Distance DistanceToGlobal(this Shape shape, Distance distance, Context context)
    {
      double dx = distance.Dx;
      double dy = distance.Dy;
      context.UserToDeviceDistance(ref dx, ref dy);
      return new Distance(dx,dy);
    }
    
    public static Distance DistanceToLocal(this Shape shape, Distance distance, Context context)
    {
      double dx = distance.Dx;
      double dy = distance.Dy;
      context.DeviceToUserDistance(ref dx, ref dy);
      return new Distance(dx,dy);
    }

    public static bool IsPointInShape(this Shape shape, PointD globalPoint, Context context,
                                      IView<Context, Model> view) {
      context.Save();
      PointD point = shape.TransformPointToLocal(globalPoint, context);
      ViewMode oldViewMode = view.Mode;
      view.Mode = ViewMode.Select;
      view.DrawItem(context, shape);
      context.LineWidth = 5;
      bool result = context.InFill(point.X, point.Y) || context.InStroke(point.X, point.Y);
      context.NewPath();
      context.Restore();
      view.Mode = oldViewMode;
      return result;
    }
    
    private static void ApplyAllParenTransforms(Shape shape, Context context) {
      if (shape == null) {
        context.IdentityMatrix();
        return;
      }
      ApplyAllParenTransforms(shape.Parent, context);
      context.Transform(shape.Matrix);
    }

    /// <summary>
    /// Gets the leftmost top and bottom, and the rightmost top and bottom coordinates of all 
    /// shapes on scene.
    /// </summary>
    /// <returns>The scene rectangle.</returns>
    public static Rectangle CalculateEnclosingRectangle(List<Shape> shapes, Context context) {
      double minX, maxX, minY, maxY;
      minX = double.MaxValue;
      maxX = double.MinValue;
      minY = double.MaxValue;
      maxY = double.MinValue;
      
      double topLeftX, topLeftY, bottomRightX, bottomRightY;
      
      foreach (Shape shape in shapes) {
        PointD pos = shape.TransformPointToGlobal(shape.Location, context);
        
        topLeftX = pos.X;
        topLeftY = pos.Y;
        bottomRightX = pos.X + shape.Width;
        bottomRightY = pos.Y + shape.Height;
        
        if (minX > topLeftX)
          minX = topLeftX;
        if (minY > topLeftY)
          minY = topLeftY;
        if (maxX < bottomRightX)
          maxX = bottomRightX;
        if (maxY < bottomRightY)
          maxY = bottomRightY;
      }
      
      double width = maxX - minX;
      double height = maxY - minY;
      
      return new Rectangle(minX, minY, width, height);
    }

  }
}

