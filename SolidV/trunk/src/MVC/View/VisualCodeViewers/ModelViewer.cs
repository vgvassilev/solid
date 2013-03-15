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
	public class ModelViewer : Viewer
	{
		public ModelViewer()
		{
		}

		public ModelViewer(View parent) : base(parent) {}

		public override void DrawItem(Context context, object item)
		{
			foreach (Model model in ((Model)item).SubModels) {
				Parent.DrawItem(context, model);
			}
		}
	}
}

