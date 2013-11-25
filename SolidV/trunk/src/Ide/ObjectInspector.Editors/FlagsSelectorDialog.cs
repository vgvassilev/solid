/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidV.Gtk.InspectorGrid.InspectorEditors
{
  using Gtk = global::Gtk;
	public class FlagsSelectorDialog: IDisposable
	{
		Gtk.TreeView treeView;
		Gtk.Dialog dialog;
		Gtk.ListStore store;
		Gtk.Window parent;
		ulong flags;
		
		public FlagsSelectorDialog(Gtk.Window parent, Type enumDesc, ulong flags, string title)
		{
			this.flags = flags;
			this.parent = parent;

			Gtk.ScrolledWindow sc = new Gtk.ScrolledWindow();
			sc.HscrollbarPolicy = Gtk.PolicyType.Automatic;
			sc.VscrollbarPolicy = Gtk.PolicyType.Automatic;
			sc.ShadowType = Gtk.ShadowType.In;
			sc.BorderWidth = 6;
			
			treeView = new Gtk.TreeView();
			sc.Add(treeView);
			
			dialog = new Gtk.Dialog();
			dialog.VBox.Add(sc);
			dialog.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
			dialog.AddButton(Gtk.Stock.Ok, Gtk.ResponseType.Ok);
			
			store = new Gtk.ListStore(typeof(bool), typeof(string), typeof(ulong));
			treeView.Model = store;
			treeView.HeadersVisible = false;
			
			Gtk.TreeViewColumn col = new Gtk.TreeViewColumn();
			
			Gtk.CellRendererToggle tog = new Gtk.CellRendererToggle();
			tog.Toggled += new Gtk.ToggledHandler(OnToggled);
			col.PackStart(tog, false);
			col.AddAttribute(tog, "active", 0);
			
			Gtk.CellRendererText crt = new Gtk.CellRendererText();
			col.PackStart(crt, true);
			col.AddAttribute(crt, "text", 1);
			
			treeView.AppendColumn(col);
			
			foreach(object value in System.Enum.GetValues(enumDesc)) {
				ulong val = Convert.ToUInt64(value);
				store.AppendValues(((flags & val) != 0), value.ToString(), val);
			}
		}
		
		public int Run()
		{
			dialog.DefaultWidth = 500;
			dialog.DefaultHeight = 400;
			dialog.ShowAll();
			return GtkExtensions.RunCustomDialog(dialog, parent);
		}
		
		public void Dispose()
		{
			dialog.Destroy();
		}
		
		void OnToggled(object s, Gtk.ToggledArgs args)
		{
			Gtk.TreeIter iter;
			if(!store.GetIterFromString(out iter, args.Path))
				return;
			
			bool oldValue =(bool) store.GetValue(iter, 0);
			ulong flag =(ulong) store.GetValue(iter, 2);
			store.SetValue(iter, 0, !oldValue);
			
			if(oldValue)
				flags &= ~flag;
			else
				flags |= flag;
		}

		public ulong Value {
			get { return flags; }
		}
	}
}
