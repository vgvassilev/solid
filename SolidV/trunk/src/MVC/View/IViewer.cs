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
	public interface IViewer
	{
		View Parent {
			get ;
			set ;
		}

		void DrawItem(Context context, object item);
	}
}
