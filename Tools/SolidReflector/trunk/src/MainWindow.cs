/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{
	public MainWindow(): base(Gtk.WindowType.Toplevel)
	{
		Build();
	}
	
	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnOpenActionActivated(object sender, System.EventArgs e)
  {
    string[] fileNames = {};

    var fc = new FileChooserDialog("Choose the file to open",
      this,
      FileChooserAction.Open,
      "Cancel", ResponseType.Cancel,
      "Open", ResponseType.Accept);
    try {
      fc.SelectMultiple = true;
      fc.SetCurrentFolder(Environment.CurrentDirectory);
      if (fc.Run() == (int)ResponseType.Accept) {
        fileNames = fc.Filenames;
      }
    } finally {
      fc.Destroy();
    }

    TreeViewColumn col = new TreeViewColumn();
    assemblyView.AppendColumn(col);

    ListStore ls = new ListStore(typeof(string));
    foreach (string file in fileNames) {
      ls.AppendValues(file);
    }

    assemblyView.Model = ls;
    assemblyView.ShowAll();
    //System.Windows.Forms.MessageBox.Show("It works!");
  }

	protected void OnExitActionActivated(object sender, System.EventArgs e)
	{
		Application.Quit();
	}
}
