/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using Gtk;
using System;

using SolidIDE;
using SolidOpt.Services;

using SolidV.Ide.Dock;

namespace TestPlugin
{
  public class TestPlugin : IPlugin
  {
    private ISolidIDE mainApp;
    private MainWindow mainWindow;
    private DockItem dockItem;
    private Gtk.Notebook noteBook;

    #region IPlugin implementation

    void IPlugin.Init(object context)
    {
      mainApp = context as ISolidIDE;
      mainWindow = mainApp.GetMainWindow();

      noteBook = new Gtk.Notebook();
      noteBook.AppendPage(new TextView(), new Gtk.Label("TestPlugin Visualizer"));
      noteBook.AppendPage(new DrawingArea(), new Gtk.Label("TestPlugin Visualizer"));
      noteBook.ShowAll();

      dockItem = mainWindow.DockFrame.AddItem("TestPlugin Visualizer");
      dockItem.Behavior = DockItemBehavior.Normal;
      dockItem.Expand = true;
      dockItem.DrawFrame = true;
      dockItem.Label = "TestPlugin Visualizer";
      dockItem.Content = noteBook;
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

    #endregion
  }
}
