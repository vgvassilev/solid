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
	public class RectangleShapeViewer : ShapeViewer
	{
		public RectangleShapeViewer()
		{
		}

		public RectangleShapeViewer(View parent) : base(parent) {}
		
		public override void DrawItem(Context context, object item)
		{
			base.DrawItem(context, (Shape)item);
		}
		
	}
}

