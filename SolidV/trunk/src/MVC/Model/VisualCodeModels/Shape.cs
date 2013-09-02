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
        // Cannot be assigned to Cairo.Rectangle
//        rectangle.Width = value;
      }
    }
    public double Height {
      get { return Rectangle.Height; }
      set { 
        // Cannot be assigned to Cairo.Rectangle
//        rectangle.Height = value;
      }
    }

    public PointD Location {
      get { return new PointD(Rectangle.X, Rectangle.Y); }
      set { rectangle = new Rectangle(value.X, value.Y, Width, Height);
        // Cannot be assigned to Cairo.Rectangle
//        rectangle.Location = value;
      }
    }

    public PointD Center {
      get { return new PointD(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2); }
      set { 
        // Cannot be assigned to Cairo.Rectangle
        //        rectangle.Location = value;
      }
    }

    private Rectangle rectangle;    
    public virtual Rectangle Rectangle {
      get { return rectangle; }
      set { rectangle = value; }
    }

	  private List<Shape> items;
	  public List<Shape> Items {
		  get { return items; }
		  set { items = value; }
	  }
    public Shape this[int i]
    {
      get { return Items[i]; }
      set { Items [i] = value; }
    }
    
    public Shape(Rectangle rect)
    {
      style = Style.DefaultStyle;
      rectangle = rect;
      items = new List<Shape>();
    }

    public Shape(Shape shape)
    {
      this.Style = shape.Style;
      this.Height = shape.Height;
      this.Location = shape.Location;
      this.Matrix = new Matrix(shape.Matrix.Xx, shape.Matrix.Yx, shape.Matrix.Xy, shape.Matrix.Yy, shape.Matrix.X0, shape.Matrix.Y0); // (Matrix)shape.matrix.Clone();
      this.Rectangle = shape.rectangle;
      this.Width = shape.Width;
	    this.items = shape.Items; //TODO: Recursive copy? Parameter memberwise/deepcopy?
    }
    
  }
}
