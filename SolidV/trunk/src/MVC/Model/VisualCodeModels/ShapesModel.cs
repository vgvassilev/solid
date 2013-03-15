// /*
//  * $Id:
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
//
using System;
using System.Collections.Generic;

namespace SolidV.MVC
{
	public class ShapesModel : Model
	{
		private List<Shape> shapes = new List<Shape>();
		public List<Shape> Shapes {
			get { return shapes; }
			set { shapes = value; }
		}

		public ShapesModel()
		{
		}
	}
}

