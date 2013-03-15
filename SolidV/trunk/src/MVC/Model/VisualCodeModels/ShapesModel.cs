using System;
using System.Collections.Generic;

namespace SolidV.MVC
{
	public class ShapesModel : Model
	{
		private List<object> shapes = new List<object>();
		public List<object> Shapes {
			get { return shapes; }
			set { shapes = value; }
		}

		public ShapesModel ()
		{
		}
	}
}

