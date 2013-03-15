using System;
using Cairo;

namespace SolidV.MVC
{
	public abstract class Shape
	{
		#region Fields
		
		private Matrix matrix = new Matrix();
		public Matrix Matrix {
			get { return matrix; }
			set { matrix = value; }
		}
		
		private string entityName = "Shape";
		public string EntityName {
			get { return entityName; }
			set { entityName = value; }
		}
		
		private Color fillColor;		
		public virtual Color FillColor {
			get { return fillColor; }
			set { fillColor = value; }
		}
		
		private Color borderColor;
		public virtual Color BorderColor {
			get { return borderColor; }
			set { borderColor = value; }
		}
		
		private int borderWidth;
		public int BorderWidth {
			get { return borderWidth; }
			set { borderWidth = value; }
		}
		public double Width {
			get { return Rectangle.Width; }
			set { 
//				rectangle.Width = value;
			}
		}
		public double Height {
			get { return Rectangle.Height; }
			set { 
//				rectangle.Height = value;
			}
		}

		public PointD Location {
			get { return new PointD(Rectangle.X, Rectangle.Y); }
			set { 
//				rectangle.Location = value;
			}
		}
		
		private Rectangle rectangle;		
		public virtual Rectangle Rectangle {
			get { return rectangle; }
			set { rectangle = value; }
		}
		#endregion
		
		
		#region Constructor
		public Shape()
		{
		}
		public Shape(Rectangle rect)
		{
			rectangle = rect;
		}
		public Shape(Shape shape)
		{
			this.borderColor = shape.borderColor;
			this.borderWidth = shape.borderWidth;
			this.entityName = shape.entityName;
			this.fillColor = shape.fillColor;
			this.Height = shape.Height;
			this.Location = shape.Location;
			this.matrix = (Matrix)shape.matrix.Clone();
			this.rectangle = shape.rectangle;
			this.Width = shape.Width;
		}
		#endregion
		
		#region Methods
		public virtual void AddItem(Shape Item)
		{	
		}
		public virtual void RemoveItem(Shape Item)
		{	
		}
		public virtual void RemoveItem(int Index)
		{	
		}
		#endregion
	}
}

