/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Gtk;

namespace DataMorphose
{
	public class Program
	{
		public static void Main(string[] args) {
      Application.Init();
			MainWindow form = new MainWindow();
			form.Show();
			Application.Run();
		}
	}
}

