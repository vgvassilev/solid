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
  /// <summary>
  /// A base class for all shapes.
  /// </summary>
  [Serializable]
  public abstract class Shape
  {
    private Shape parent;
    public Shape Parent {
      get { return parent; }
      set { parent = value; }
    }

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
      set { Rectangle = new Rectangle(Rectangle.X, Rectangle.Y, value, Height); }
    }

    public double Height {
      get { return Rectangle.Height; }
      set { Rectangle = new Rectangle(Rectangle.X, Rectangle.Y, Width, value); }
    }

    /// <summary>
    /// Gets or sets the location, (x,y) or the top,left of the bounding rectangle.
    /// </summary>
    /// <value>The top,left location.</value>
    /// 
    public PointD Location {
      get { return new PointD(Rectangle.X, Rectangle.Y); }
      set { rectangle = new Rectangle(value.X, value.Y, Width, Height); }
    }

    /// <summary>
    /// Gets or sets the center of the bounding rectangle.
    /// </summary>
    /// <value>The center of the bounding rectangle.</value>
    /// 
    public PointD Center {
      get { return new PointD(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2); }
      set { Rectangle = new Rectangle(value.X - Width / 2, value.Y - Height / 2, Width, Height); }
    }

    /// <summary>
    /// A bounding rectangle.
    /// </summary>
    /// 
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

    private object describedEntity;
    public object DescribedEntity {
      get { return describedEntity; }
      set { describedEntity = value;}
    }

    public Shape this[int i]
    {
      get { return Items[i]; }
      set { Items[i] = value; }
    }
    
    public Shape(Rectangle rect)
    {
      style = Style.DefaultStyle;
      rectangle = rect;
      items = new List<Shape>();
    }
  }
}
