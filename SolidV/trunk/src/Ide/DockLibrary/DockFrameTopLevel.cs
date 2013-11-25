using System;
using Gtk;

namespace MonoDevelop.Components.Docking
{
	class DockFrameTopLevel: EventBox
	{
		int x, y;
		
		public int X {
			get { return x; }
			set {
				x = value;
				if (Parent != null)
					Parent.QueueResize ();
			}
		}
		
		public int Y {
			get { return y; }
			set {
				y = value;
				if (Parent != null)
					Parent.QueueResize ();
			}
		}
	}

}
