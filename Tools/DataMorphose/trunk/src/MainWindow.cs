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


    protected void OnOpenActionActivated (object sender, System.EventArgs e)
    {
      var fc = new Gtk.FileChooserDialog("Choose the file to open",
                                         this, Gtk.FileChooserAction.Open,
                                         "Cancel", Gtk.ResponseType.Cancel,
                                         "Open", Gtk.ResponseType.Accept);
      try {
        fc.SelectMultiple = false;
        var filter = new Gtk.FileFilter();
        filter.Name = "Database description files (*.csvdb)";
        filter.AddPattern("*.csvdb");
        fc.AddFilter(filter);
        fc.SetCurrentFolder(Environment.CurrentDirectory);
        if (fc.Run() == (int)Gtk.ResponseType.Accept) {

        }
      }
      finally {
        fc.Destroy();
      }
    }

    protected void OnQuitActionActivated (object sender, System.EventArgs e)
    {
      Gtk.Application.Quit();
    }
	}
}

