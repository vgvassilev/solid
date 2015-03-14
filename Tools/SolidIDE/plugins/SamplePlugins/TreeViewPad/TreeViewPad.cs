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

namespace TreeViewPad
{
  public class TreeViewPad : IPlugin, IPad
  {
    private ISolidIDE mainApp;
    private MainWindow mainWindow;
    private Gtk.TreeView treeView;
    private Gtk.Notebook noteBook;
    private string currentPath;

    #region IPlugin implementation

    void IPlugin.Init(object context) {
      mainApp = context as ISolidIDE;
      mainWindow = mainApp.GetMainWindow();
      DockFrame frame = mainWindow.DockFrame;

      // Tree View
      treeView = new Gtk.TreeView();
      Gtk.TreeViewColumn col = new Gtk.TreeViewColumn();
      Gtk.CellRendererText colAssemblyCell = new Gtk.CellRendererText();
      col.PackStart(colAssemblyCell, true);
      col.AddAttribute(colAssemblyCell, "text", 0);
      if (treeView.GetColumn(0) != null)
        treeView.Columns[0] = col;
      else
        treeView.AppendColumn(col);
      treeView.Model = new Gtk.TreeStore(typeof(string));
      treeView.Model = homeSubFolders(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
      treeView.RowActivated += HandleRowActivated;

      Gtk.ScrolledWindow treeViewScrollWindow = new Gtk.ScrolledWindow();
      Gtk.Viewport treeViewViewport = new Gtk.Viewport();
      treeViewScrollWindow.Add(treeViewViewport);
      treeViewViewport.Add(treeView);
      treeViewScrollWindow.ShowAll();

      DockItem treeViewDock = frame.AddItem("TreeViewDock");
      treeViewDock.Behavior = DockItemBehavior.Normal;
      treeViewDock.Expand = true;
      treeViewDock.DrawFrame = true;
      treeViewDock.Label = "Files";
      treeViewDock.Content = treeViewScrollWindow;
      treeViewDock.DefaultVisible = true;
      treeViewDock.Visible = true;

      // Text Editor Notebook
      noteBook = new Gtk.Notebook();

      Gtk.ScrolledWindow textEditorScrollWindow = new Gtk.ScrolledWindow();
      Gtk.Viewport textEditorViewport = new Gtk.Viewport();
      textEditorScrollWindow.Add(textEditorViewport);
      textEditorViewport.Add(noteBook);
      textEditorScrollWindow.ShowAll();

      DockItem textEditorDock = frame.AddItem("TextEditorDock");
      textEditorDock.Behavior = DockItemBehavior.Normal;
      textEditorDock.Expand = true;
      textEditorDock.DrawFrame = true;
      textEditorDock.Label = "Text Editor";
      textEditorDock.Content = textEditorScrollWindow;
      textEditorDock.DefaultVisible = true;
      textEditorDock.Visible = true;

      // Menu
      var fileMenuItem = mainApp.GetMenuItem<Gtk.ImageMenuItem>("File");
      var saveMenuItem = mainApp.GetMenuItem<Gtk.ImageMenuItem>("File", "Save");
      saveMenuItem.Activated += HandleSaveActivated;
      var closeMenuItem = mainApp.GetMenuItem<Gtk.ImageMenuItem>("File", "Close");
      closeMenuItem.Activated += HandleCloseActivated;
      var exitMenuItem = mainApp.GetMenuItem<Gtk.ImageMenuItem>("File", "Exit");
      exitMenuItem.Activated += HandleExitActivated;

      /*
      mainApp.GetMenuItem<Gtk.SeparatorMenuItem>("View", "");
      mainApp.GetMenuItem<Gtk.CheckMenuItem>("View", "CheckTest1");
      mainApp.GetMenuItem<Gtk.CheckMenuItem>("View", "CheckTest2");
      //mainApp.GetMenuItem<Gtk.RadioMenuItem>("View", "RadioTest1");
      //mainApp.GetMenuItem<Gtk.RadioMenuItem>("View", "RadioTest2");
      //mainApp.GetMenuItem<Gtk.RadioMenuItem>("View", "RadioTest3");
      */
    }

    void IPlugin.UnInit(object context) {
      throw new NotImplementedException();
    }

    #endregion

    #region IPad implementation

    void IPad.Init(DockFrame frame) {
      throw new NotImplementedException();
    }

    #endregion

    void HandleSaveActivated(object sender, EventArgs e)
    {
      if (currentPath != null)
        using (StreamWriter file = new System.IO.StreamWriter(currentPath)) {
          //file.Write(textView.Buffer.Text);
        }
    }

    void HandleCloseActivated(object sender, EventArgs e)
    {
      noteBook.RemovePage(noteBook.Page);
    }

    void HandleExitActivated(object sender, EventArgs e) {
      Gtk.MessageDialog md = new Gtk.MessageDialog(mainWindow,
        Gtk.DialogFlags.DestroyWithParent | Gtk.DialogFlags.Modal,
        Gtk.MessageType.Question,
        Gtk.ButtonsType.OkCancel,
        "Are you sure to quit?");
      md.Response += delegate (object o, Gtk.ResponseArgs response) {
        if (response.ResponseId == Gtk.ResponseType.Ok) {
          mainApp.DisableQuit(true);
        }
      };
      md.Run();
      md.Destroy();
    }

    void HandleRowActivated(object o, Gtk.RowActivatedArgs args)
    {
      Gtk.TreeIter iter;
      treeView.Model.GetIter(out iter, args.Path);
      currentPath = Path.GetFullPath((string) treeView.Model.GetValue(iter, 0));

      FileAttributes attr = File.GetAttributes(currentPath);

      if ((attr & FileAttributes.Directory) != FileAttributes.Directory) {
        Gtk.TextView textView = new Gtk.TextView();
        textView.Buffer.Text = File.ReadAllText(currentPath);
        noteBook.AppendPage(textView, new Gtk.Label(currentPath));
        noteBook.ShowAll();
        return;
      }

      DirectoryInfo rootDirInfo = new DirectoryInfo(currentPath);
      attachSubTree(treeView.Model, iter, rootDirInfo.GetDirectories(), rootDirInfo.GetFiles());
    }

    protected void attachSubTree(Gtk.TreeModel model, Gtk.TreeIter parent, params object[] elements)
    {
      Gtk.TreeStore store = model as Gtk.TreeStore;

      // remove the values if they were added before.
      Gtk.TreePath path = store.GetPath(parent);
      path.Down();
      Gtk.TreeIter iter;
      while (store.GetIter(out iter, path))
        store.Remove(ref iter);

      // Add the elements to the tree view.
      DirectoryInfo[] di = elements[0] as DirectoryInfo[];
      FileInfo[] fi = elements[1] as FileInfo[];

      for (uint i = 0; i < di.Length; ++i) {
        store.AppendValues(parent, di[i].ToString());
      }

      for (uint i = 0; i < fi.Length; ++i) {
        store.AppendValues(parent, fi[i].ToString());
      }
    }

    private Gtk.TreeStore homeSubFolders(string dir) {
      Gtk.TreeStore store = treeView.Model as Gtk.TreeStore;

      if (store == null)
        store = new Gtk.TreeStore(typeof(string));

      DirectoryInfo rootDirInfo = new DirectoryInfo(dir);
      DirectoryInfo[] subDirInfo = rootDirInfo.GetDirectories();

      //store.AppendValues(subDirInfo);
      foreach (DirectoryInfo di in subDirInfo) {
        if ((di.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
          store.AppendValues(di.FullName);
      }
      return store;
    }

    private void RenderAssemblyDefinition(Gtk.TreeViewColumn column, Gtk.CellRenderer cell,
                                          Gtk.TreeModel model, Gtk.TreeIter iter) {
    }
  }
}
