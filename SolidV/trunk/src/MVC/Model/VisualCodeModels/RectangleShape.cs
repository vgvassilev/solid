using System;

namespace SolidV.MVC
{
	public class RectangleShape : Shape
	{
		public RectangleShape(RectangleShape rect) : base(rect)
		{
		}

		public RectangleShape() : base() {}
	}
}

