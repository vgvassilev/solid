/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using Gtk;
using System;

using SampleTool;
using SolidOpt.Services;

using SolidV.Ide.Dock;

namespace TestPlugin
{
  public class TestPlugin : IPlugin
  {
    DockItem dockItem = null;
    MainWindow mainWindow = null;

    void IPlugin.Init(object context)
    {
      ISampleTool reflector = context as ISampleTool;
      mainWindow = reflector.GetMainWindow();

      Gtk.Notebook nb = new Gtk.Notebook();
      nb.AppendPage(new TextView(), new Gtk.Label("TestPlugin Visualizer"));
      nb.AppendPage(new DrawingArea(), new Gtk.Label("TestPlugin Visualizer"));
      nb.ShowAll();

      dockItem = mainWindow.DockFrame.AddItem("TestPlugin Visualizer");
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
      dockItem.Visible = false;
      // BUG: Object not set to an instance of an object exception if only one plugin is loaded
      // and attempted to be UnInit-ed
      mainWindow.DockFrame.RemoveItem(dockItem);
    }
  }
}

