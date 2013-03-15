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
	public class Viewer : IViewer
	{
		private View parent;
		public View Parent {
			get { return parent; }
			set { parent = value; }
		}

		public Viewer()
		{
		}

		public Viewer(View parent) {
			this.Parent = parent;
		}

		public virtual void DrawItem(Context context, object item)
		{
		}
	}
}

