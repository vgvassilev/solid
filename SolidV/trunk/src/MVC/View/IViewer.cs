using System;

using Cairo;

namespace SolidV.MVC
{
	public interface IViewer
	{
		View Parent {
			get ;
			set ;
		}

		void DrawItem(Context context, object item);
	}
}
