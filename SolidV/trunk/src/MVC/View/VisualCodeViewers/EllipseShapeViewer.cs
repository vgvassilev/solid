// /*
//  * $Id:
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
//
using System;

using Cairo;

namespace SolidV.MVC
{
	public class EllipseShapeViewer : ShapeViewer
	{
		public EllipseShapeViewer()
		{
		}

		public EllipseShapeViewer(View parent) : base(parent) {}
		
		public override void DrawItem(Context context, object item)
		{
			Shape shape = (Shape)item;
			context.Arc((shape.Location.X + shape.Width) / 2, (shape.Location.Y + shape.Height) / 2, shape.Height, 0, 2 * Math.PI);
			context.Color = shape.Style.FillColor;
      context.Fill();
		}
	}
}

