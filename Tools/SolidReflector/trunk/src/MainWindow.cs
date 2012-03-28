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

	protected void OnOpenActionActivated (object sender, System.EventArgs e)
	{
		System.Windows.Forms.MessageBox.Show("It works!");
	}

	protected void OnExitActionActivated (object sender, System.EventArgs e)
	{
		Application.Quit();
	}
}
