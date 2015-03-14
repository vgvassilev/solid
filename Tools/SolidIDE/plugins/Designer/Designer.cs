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

namespace SolidIDE.Plugins.Designer
{
  public class DesignerPlugin : IPlugin, IDesigner
  {
    // External global objects (form Main program and other plugins)
    private ISolidIDE solidIDE;
    private MainWindow mainWindow;

    // Plugin global objects
    private DockItem designerDockItem;
    private Gtk.Notebook noteBook;

    #region IPlugin implementation

    void IPlugin.Init(object context) {
      solidIDE = context as ISolidIDE;
      mainWindow = solidIDE.GetMainWindow();

      // Notebook
      noteBook = new Gtk.Notebook();
      noteBook.AppendPage(new DrawingArea(), new Gtk.Label("Sheet"));
      noteBook.ShowAll();

      designerDockItem = mainWindow.DockFrame.AddItem("Designer");
      designerDockItem.Behavior =
        DockItemBehavior.CantAutoHide |
        DockItemBehavior.CantClose |
        DockItemBehavior.NeverFloating;
      designerDockItem.Expand = true;
      designerDockItem.DrawFrame = true;
      designerDockItem.Label = "Designer";
      designerDockItem.Content = noteBook;
      designerDockItem.DefaultVisible = true;
      designerDockItem.Visible = true;

      // Menu
      var fileMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("File");
      var saveMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("File", "Save");
      saveMenuItem.Activated += HandleSaveActivated;
    }

    void IPlugin.UnInit(object context) {
      designerDockItem.Visible = false;
      // BUG: Object not set to an instance of an object exception if only one plugin is loaded
      // and attempted to be UnInit-ed
      mainWindow.DockFrame.RemoveItem(designerDockItem);
    }

    #endregion

    #region IDesigner

      Gtk.Notebook IDesigner.GetNotebook() {
        return this.noteBook;
      }

    #endregion

    void HandleSaveActivated(object sender, EventArgs e) {
      //using (StreamWriter file = new System.IO.StreamWriter(currentPath)) {
        //file.Write(textView.Buffer.Text);
      //}
    }

  }
}
