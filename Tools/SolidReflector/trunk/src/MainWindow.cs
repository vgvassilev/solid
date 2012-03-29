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
		System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
		ofd.Multiselect = true;
		ofd.CheckFileExists = true;
		ofd.InitialDirectory = Environment.CurrentDirectory;
		if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
			fileNames = ofd.FileNames;
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
