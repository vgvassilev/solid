/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using Gtk;
using System;
using SolidOpt.Services;

namespace SampleTool
{
  public partial class LoadedPlugins : Gtk.Dialog
  {
    public TreeView treeView;
    public LoadedPlugins(PluginServiceContainer plugins) {
      this.Build();

      this.treeView = treeview1;
      Gtk.TreeViewColumn column = new Gtk.TreeViewColumn();
      column.Title = "Loaded plugins";
      treeview1.AppendColumn(column);

      Gtk.ListStore loadedPlugins = new Gtk.ListStore(typeof(string));
      Gtk.CellRendererText cell = new Gtk.CellRendererText();

      column.PackStart(cell, true);
      column.AddAttribute(cell, "text", 0);

      for (int i = 0; i < plugins.Plugins.Count; i++) {
        loadedPlugins.AppendValues(System.IO.Path.GetFileName(plugins.Plugins[i].codeBase));
      }

      treeview1.Model = loadedPlugins;
    }
  }
}

