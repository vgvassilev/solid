/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using Gtk;
using System;
using SolidOpt.Services;

namespace SolidIDE
{
  public partial class LoadedPlugins : Gtk.Dialog
  {
    public TreeView treeView;
    public Gtk.ListStore lsLoadedPlugins;
    public LoadedPlugins(PluginServiceContainer plugins) {
      this.Build();

      this.treeView = treeview1;
      treeView.Selection.Mode = SelectionMode.Multiple;

      Gtk.TreeViewColumn column = new Gtk.TreeViewColumn();
      column.Title = "Loaded plugins";
      treeview1.AppendColumn(column);

      lsLoadedPlugins = new Gtk.ListStore(typeof(string));
      Gtk.CellRendererText cell = new Gtk.CellRendererText();

      column.PackStart(cell, true);
      column.AddAttribute(cell, "text", 0);

      for (int i = 0; i < plugins.Plugins.Count; i++) {
        lsLoadedPlugins.AppendValues(plugins.Plugins[i].codeBase);
      }

      treeview1.Model = lsLoadedPlugins;
    }
  }
}

