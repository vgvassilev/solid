using System;

namespace SolidV.MVC
{
	public class Viewer : IViewer
	{
		private View parent;
		public View Parent {
			get { return parent; }
			set { parent = value; }
		}

		public Viewer ()
		{
		}

		public Viewer(View parent) {
			this.Parent = parent;
		}

		public virtual void DrawItem(Cairo.Context context, object item)
		{
		}
	}
}

