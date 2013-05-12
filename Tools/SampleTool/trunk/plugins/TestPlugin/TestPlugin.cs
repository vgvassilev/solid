// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using Gtk;
using System;

using SampleTool;
using SolidOpt.Services;

using MonoDevelop.Components.Docking;

namespace TestPlugin
{
  public class TestPlugin : IPlugin
  {
    private Gtk.TreeView assemblyTree = new Gtk.TreeView();

    void IPlugin.Init(object context)
    {
      DockItem dockItem = null;
      ISampleTool reflector = context as ISampleTool;
      var MainWindow = reflector.GetMainWindow();

      Gtk.Notebook nb = new Gtk.Notebook();
      nb.AppendPage(new TextView(), new Gtk.Label("TestPlugin Visualizer"));
      nb.AppendPage(new DrawingArea(), new Gtk.Label("TestPlugin Visualizer"));
      nb.ShowAll();

      dockItem = MainWindow.DockFrame.AddItem("TestPlugin Visualizer");

      dockItem.Visible = true;
      dockItem.Behavior = DockItemBehavior.Normal;
      dockItem.Expand = true;
      dockItem.DrawFrame = true;
      dockItem.Label = "TestPlugin Visualizer";
      dockItem.Content = nb;
      dockItem.DefaultVisible = true;
      dockItem.Visible = true;
    }

    void IPlugin.UnInit(object context)
    {
    }
  }
}

