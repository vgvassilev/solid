using System;

using Cairo;

namespace SolidV.MVC
{
	public class ShapeModelViewer : Viewer
	{
		public ShapeModelViewer ()
		{
		}

		public ShapeModelViewer(View parent) : base(parent) {}

		public override void DrawItem(Context context, object item)
		{
			foreach (Shape shape in ((ShapesModel)item).Shapes) {
				Parent.DrawItem(context, shape);
			}
		}
	}
}

