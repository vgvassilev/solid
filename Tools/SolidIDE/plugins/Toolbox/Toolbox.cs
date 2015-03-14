/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;

using SolidIDE;
using SolidOpt.Services;

using SolidV.Ide.Dock;

using SolidIDE.Plugins.Designer;

namespace SolidIDE.Plugins.Toolbox
{
  public class Toolbox : IPlugin, IPad, IToolbox
  {
    // External global objects (form Main program and other plugins)
    private ISolidIDE solidIDE;
    private MainWindow mainWindow;

    // Plugin global objects
    private DockItem toolboxDockItem;
    private Gtk.TreeView treeView;

    #region IPlugin implementation

    void IPlugin.Init(object context) {
      solidIDE = context as ISolidIDE;
      mainWindow = solidIDE.GetMainWindow();

      // Dock with Tree
      Gtk.ScrolledWindow treeViewScrollWindow = new Gtk.ScrolledWindow();
      Gtk.Viewport treeViewViewport = new Gtk.Viewport();
      treeViewScrollWindow.Add(treeViewViewport);
      treeView = new Gtk.TreeView();
      treeViewViewport.Add(treeView);
      treeViewScrollWindow.ShowAll();
      Gtk.TreeViewColumn col = new Gtk.TreeViewColumn();
      Gtk.CellRendererText colAssemblyCell = new Gtk.CellRendererText();
      col.PackStart(colAssemblyCell, true);
      col.AddAttribute(colAssemblyCell, "text", 0);
      treeView.AppendColumn(col);
      treeView.Model = new Gtk.TreeStore(typeof(string), typeof(object));
      treeView.RowActivated += HandleRowActivated;

      toolboxDockItem = mainWindow.DockFrame.AddItem("Toolbox");
      toolboxDockItem.Behavior = DockItemBehavior.Normal;
      toolboxDockItem.Expand = true;
      toolboxDockItem.DrawFrame = true;
      toolboxDockItem.Label = "Toolbox";
      toolboxDockItem.Content = treeViewScrollWindow;
      toolboxDockItem.DefaultVisible = true;
      toolboxDockItem.Visible = true;

      UpdateToolbox();

      // Menu
      var viewMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("View");
      solidIDE.GetMenuItem<Gtk.TearoffMenuItem>("View", "TearoffView");
      var toolboxMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("View", "Toolbox");
      toolboxMenuItem.Activated += HandleShowToolboxActivated;
    }

    void IPlugin.UnInit(object context) {
      // throw new NotImplementedException();
    }

    #endregion

    #region IPad implementation

    void IPad.Init(DockFrame frame) {
      // throw new NotImplementedException();
    }

    #endregion

    #region IToolbox implementation


    #endregion

    protected void UpdateToolbox() {
      Gtk.TreeStore store = treeView.Model as Gtk.TreeStore;
      if (store == null) store = new Gtk.TreeStore(typeof(string), typeof(object));
      store.AppendValues("Test1", new object());
      store.AppendValues("Test2", new object());
      store.AppendValues("Test3", null);
      store.AppendValues("Test4", "Test4!");
    }

    void HandleShowToolboxActivated(object sender, EventArgs e) {
      toolboxDockItem.Visible = true;
    }

    void HandleRowActivated(object o, Gtk.RowActivatedArgs args) {
      Gtk.TreeIter iter;
      treeView.Model.GetIter(out iter, args.Path);
      string currentName = (string)treeView.Model.GetValue(iter, 0);
      object currentInfoObject = treeView.Model.GetValue(iter, 1);

      IServiceContainer plugins = solidIDE.GetServiceContainer();
      IDesigner designer = plugins.GetService<IDesigner>();
      Gtk.Notebook noteBook = designer.GetNotebook();

      noteBook.AppendPage(new Gtk.DrawingArea(), new Gtk.Label("Sheet "+currentName+" ["+currentInfoObject+"]"));
      noteBook.ShowAll();
    }

  }
}
