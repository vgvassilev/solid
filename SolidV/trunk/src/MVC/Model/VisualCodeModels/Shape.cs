/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.MVC
{
  public abstract class Shape
  {
    private Matrix matrix = new Matrix();
    public Matrix Matrix {
      get { return matrix; }
      set { matrix = value; }
    }

    private Style style;
    public Style Style {
      get { return style; }
      set { style = value; }
    }
    
    public double Width {
      get { return Rectangle.Width; }
      set { 
//        rectangle.Width = value;
      }
    }
    public double Height {
      get { return Rectangle.Height; }
      set { 
//        rectangle.Height = value;
      }
    }

    public PointD Location {
      get { return new PointD(Rectangle.X, Rectangle.Y); }
      set { 
//        rectangle.Location = value;
      }
    }
    
    private Rectangle rectangle;    
    public virtual Rectangle Rectangle {
      get { return rectangle; }
      set { rectangle = value; }
    }
    
    
    public Shape()
    {
      style = Style.DefaultStyle;
    }

    public Shape(Rectangle rect): this()
    {
      rectangle = rect;
    }

    public Shape(Shape shape)
    {
      this.Style = shape.Style;
      this.Height = shape.Height;
      this.Location = shape.Location;
      this.Matrix = new Matrix(shape.Matrix.Xx, shape.Matrix.Yx, shape.Matrix.Xy, shape.Matrix.Yy, shape.Matrix.X0, shape.Matrix.Y0); // (Matrix)shape.matrix.Clone();
      this.Rectangle = shape.rectangle;
      this.Width = shape.Width;
    }
    

    public virtual void AddItem(Shape Item)
    {  
    }
    
    public virtual void RemoveItem(Shape Item)
    {  
    }
    
    public virtual void RemoveItem(int Index)
    {  
    }

  }
}
