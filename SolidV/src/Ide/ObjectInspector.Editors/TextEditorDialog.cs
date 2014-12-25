/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.ComponentModel;

namespace SolidV.Gtk.InspectorGrid.InspectorEditors
{
  using Gtk = global::Gtk;
  public class TextEditorDialog: IDisposable
  {

    Gtk.TextView textview;
    Gtk.Dialog dialog;
    
    public TextEditorDialog()
    {
      Gtk.ScrolledWindow sc = new Gtk.ScrolledWindow();
      sc.HscrollbarPolicy = Gtk.PolicyType.Automatic;
      sc.VscrollbarPolicy = Gtk.PolicyType.Automatic;
      sc.ShadowType = Gtk.ShadowType.In;
      sc.BorderWidth = 6;
      
      textview = new Gtk.TextView();
      sc.Add(textview);
      
      dialog = new Gtk.Dialog();
      dialog.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
      dialog.AddButton(Gtk.Stock.Ok, Gtk.ResponseType.Ok);
      dialog.VBox.Add(sc);
    }

    public Gtk.Window TransientFor {
      set { dialog.TransientFor = value; }
    }
    
    public string Text {
      get { return textview.Buffer.Text; }
      set { textview.Buffer.Text = value; }
    }
    
    public int Run()
    {
      dialog.DefaultWidth = 500;
      dialog.DefaultHeight = 400;
      dialog.ShowAll();
      return GtkExtensions.RunCustomDialog(dialog, dialog.TransientFor);
    }
    
    public void Dispose()
    {
      dialog.Destroy();
    }
  }
}
