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

