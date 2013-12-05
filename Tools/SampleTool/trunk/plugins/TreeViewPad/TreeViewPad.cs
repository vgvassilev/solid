/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;

using SampleTool;
using SolidOpt.Services;
using SolidV.Ide.Dock;

namespace TreeViewPad
{
  public class TreeViewPad : IPlugin, IPad
  {
    private Gtk.TextView textView = new Gtk.TextView();
    private Gtk.TreeView treeView = new Gtk.TreeView();
    private Gtk.Notebook nb = new Gtk.Notebook();
    private Gtk.TreeIter iter;

    #region IPlugin implementation

    void IPlugin.Init(object context) {
      ISampleTool SampleTool = context as ISampleTool;
      DockFrame frame = SampleTool.GetMainWindow().DockFrame;

      Gtk.ScrolledWindow treeViewScrollWindow = new Gtk.ScrolledWindow();
      Gtk.Viewport treeViewViewport = new Gtk.Viewport();
      treeViewScrollWindow.Add(treeViewViewport);
      treeViewViewport.Add(treeView);
      treeViewScrollWindow.ShowAll();

      Gtk.ScrolledWindow textEditorScrollWindow = new Gtk.ScrolledWindow();
      Gtk.Viewport textEditorViewport = new Gtk.Viewport();
      textEditorScrollWindow.Add(textEditorViewport);
      textEditorViewport.Add(nb);
      textEditorScrollWindow.ShowAll();

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

      DockItem treeViewDock = frame.AddItem("TreeViewDock");
      treeViewDock.Behavior = DockItemBehavior.Normal;
      treeViewDock.Expand = true;
      treeViewDock.DrawFrame = true;
      treeViewDock.Label = "Files";
      treeViewDock.Content = treeViewScrollWindow;
      treeViewDock.DefaultVisible = true;
      treeViewDock.Visible = true;

      DockItem textEditorDock = frame.AddItem("TextEditorDock");
      textEditorDock.Behavior = DockItemBehavior.Normal;
      textEditorDock.Expand = true;
      textEditorDock.DrawFrame = true;
      textEditorDock.Label = "Text Editor";
      textEditorDock.Content = textEditorScrollWindow;
      textEditorDock.DefaultVisible = true;
      textEditorDock.Visible = true;
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

    void HandleRowActivated (object o, Gtk.RowActivatedArgs args)
    {
      treeView.Model.GetIter(out iter, args.Path);
      string currentDir = Path.GetFullPath((string) treeView.Model.GetValue(iter, 0));

      FileAttributes attr = File.GetAttributes(currentDir);

      if ((attr & FileAttributes.Directory) != FileAttributes.Directory) {
        textView = new Gtk.TextView();
        textView.Buffer.Text = File.ReadAllText(currentDir);
        nb.AppendPage(textView, new Gtk.Label(currentDir));
        nb.ShowAll();
        return;
      }

      DirectoryInfo rootDirInfo = new DirectoryInfo(currentDir);
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

