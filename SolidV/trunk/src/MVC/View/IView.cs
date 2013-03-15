using System;

using Cairo;

namespace SolidV.MVC
{
	public interface IView
	{
		void Draw(Context context, Model model);
	}
}

