// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */

using System;
using Gtk;

namespace DataMorphose
{
	public partial class MainWindow : Gtk.Window
	{
		public MainWindow() : base(Gtk.WindowType.Toplevel) {
			this.Build();
		}

    protected void OnDeleteEvent (object sender, DeleteEventArgs a) {
      Application.Quit();
		  a.RetVal = true;
	  }
	}
}

