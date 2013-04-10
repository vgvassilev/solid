using Mono.Cecil;
using MonoDevelop.Components.Docking;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SolidReflector.Plugins.AssemblyBrowser
{
  public class AssemblyBrowser : IPlugin, IAssemblyBrowser
  {
    private ISolidReflector reflector = null;
    private DockItem dockItem;
    private Gtk.TreeView assemblyTree = new Gtk.TreeView();

    public AssemblyBrowser() { }

    #region IAssemblyBrowser implementation
    event EventHandler<SelectionEventArgs> SelectionChangedEvent = null;
    event EventHandler<SelectionEventArgs> IAssemblyBrowser.SelectionChanged {
      add {
        if (SelectionChangedEvent != null)
          SelectionChangedEvent += value;
        else
          SelectionChangedEvent = new EventHandler<SelectionEventArgs>(value);
      }      
      remove {
        if (SelectionChangedEvent != null)
          SelectionChangedEvent -= value;
      }
    }

    AssemblyBrowser IAssemblyBrowser.GetAssemblyBrowser()
    {
      return this;
    }

    #endregion

    #region IPlugin implementation
    void IPlugin.Init(ISolidReflector reflector)
    {
      this.reflector = reflector;

      var MainWindow = reflector.GetMainWindow();
      Gtk.MenuBar mainMenuBar = reflector.GetMainMenu();
      reflector.OnShutDown += HandleOnShutDown;

      Gtk.MenuItem fileMenu = null;
      // Find the File menu if present
      foreach(Gtk.Widget w in mainMenuBar.Children)
        if (w.Name == "FileAction")
          fileMenu = w as Gtk.MenuItem;

      // If not present - create it
      if (fileMenu == null) {
        Gtk.Menu menu = new Gtk.Menu();
        fileMenu = new Gtk.MenuItem("File");
        fileMenu.Submenu = menu;
        mainMenuBar.Append(fileMenu);
      }

      Gtk.MenuItem open = new Gtk.MenuItem("Open");
      open.Activated += OnActivated;
      (fileMenu.Submenu as Gtk.Menu).Prepend(open);

      dockItem = MainWindow.DockFrame.AddItem("AssemblyBrowser");
      dockItem.DrawFrame = true;
      dockItem.Label = "Assembly";
      dockItem.Content = assemblyTree;

      LoadEnvironment();

      assemblyTree.RowActivated += HandleRowActivated;
      assemblyTree.DeleteEvent += HandleDeleteEvent;
    }

    void HandleOnShutDown (object sender, EventArgs e)
    {
      SaveLoadedAssemblies();
    }

    private void SaveLoadedAssemblies() {
      List<string> filesLoaded = null;
      GetLoadedFiles(out filesLoaded);

      if (!File.Exists(System.IO.Path.Combine(reflector.GetConfigurationDirectory(),
                                              "Assemblies.env"))) {
        File.Create(System.IO.Path.Combine(reflector.GetConfigurationDirectory(),
                                           "Assemblies.env")).Dispose();
      }
      saveLoadedAssembliesData(System.IO.Path.Combine(reflector.GetConfigurationDirectory(), 
                                                      "Assemblies.env"), filesLoaded);
    }

    private void saveLoadedAssembliesData(string curEnv, List<string> items) {
      FileStream file = new FileStream(curEnv, FileMode.OpenOrCreate, FileAccess.ReadWrite);
      using (StreamWriter writer = new StreamWriter(file)) {
        writer.Write("");
        writer.Flush();
        foreach (string line in items)
          writer.WriteLine(line);
        writer.Flush();
      }
    }

    void OnActivated(object sender, EventArgs args)
    {
      var fc = new Gtk.FileChooserDialog("Choose the file to open", null, 
                                         Gtk.FileChooserAction.Open, "Cancel", 
                                         Gtk.ResponseType.Cancel, "Open", Gtk.ResponseType.Accept);
      try {
        fc.SelectMultiple = true;
        fc.SetCurrentFolder(Environment.CurrentDirectory);
        if (fc.Run() == (int)Gtk.ResponseType.Accept) {
          // Get the currently loaded files in the tree view
          List<string> filesLoaded;
          GetLoadedFiles(out filesLoaded);

          List<string> filesToLoad = new List<string>();
          filesToLoad.AddRange(fc.Filenames);
          for (uint i = 0; i < fc.Filenames.Length; i++)
            if (filesLoaded.Contains(fc.Filenames[i])) {
              filesToLoad.Remove(fc.Filenames[i]);
              ShowMessageGtk(String.Format("File {0} already loaded.", fc.Filenames[i]));
            }
          LoadFilesInTreeView(filesToLoad.ToArray());
        }
      } finally {
        fc.Destroy();
      }
    }

    /// <summary>
    /// Walks up the tree view's model and collects the loaded files.
    /// </summary>
    /// <param name='filesLoaded'>[out]
    /// Storage where the loaded file names will be added.
    /// </param>
    private void GetLoadedFiles(out List<string> filesLoaded) {
      filesLoaded = new List<string>();
      if (assemblyTree.Columns.Length == 0)
        return;

      Gtk.TreeIter iter;
      assemblyTree.Model.GetIterFirst(out iter);
      AssemblyDefinition aDef = null;
      do {
        aDef = assemblyTree.Model.GetValue(iter, 0) as AssemblyDefinition;
        Debug.Assert(aDef != null && aDef.MainModule != null,
                     "We must have only assembly definitions here");
        filesLoaded.Add(aDef.MainModule.FullyQualifiedName);
      }
      while (assemblyTree.Model.IterNext(ref iter));
    }

    void HandleDeleteEvent (object o, Gtk.DeleteEventArgs args)
    {
      ShowMessageGtk("asd");
      //SaveEnvironment();
      //Gtk.Application.Quit();
      //a.RetVal = true;
    }

    void HandleRowActivated (object o, Gtk.RowActivatedArgs args)
    {
      //SelectionEventArgs selectionArgs = new SelectionEventArgs();
      //SelectionChangedEvent(o, selectionArgs);
      Gtk.TreeIter iter;
      assemblyTree.Model.GetIter(out iter, args.Path);

      object currentObj = (object) assemblyTree.Model.GetValue(iter, 0);
      SelectionEventArgs selectionArgs;
      AssemblyDefinition curAssembly = null;
      ModuleDefinition curModule = null;
      MemberReference curMember = null;

      switch(args.Path.Depth) {
        case 1:
          curAssembly = currentObj as AssemblyDefinition;
          Debug.Assert(curAssembly != null, "Assembly cannot be null.");
          AttachSubTree(assemblyTree.Model, iter, curAssembly.Modules.ToArray());
          selectionArgs = new SelectionEventArgs(null, null, curAssembly);
          SelectionChangedEvent(o, selectionArgs);
          break;
        case 2:
          curModule = currentObj as ModuleDefinition;
          Debug.Assert(curModule != null, "CurModule is null!?");
          AttachSubTree(assemblyTree.Model, iter, curModule.Types.ToArray());
          selectionArgs = new SelectionEventArgs(null, curModule, curAssembly);
          SelectionChangedEvent(o, selectionArgs);
          break;
        case 3:
          TypeDefinition curType = currentObj as TypeDefinition;
          Debug.Assert(curType != null, "CurType is null!?");
          List<IMemberDefinition> members = new List<IMemberDefinition>();
          members.AddRange(curType.Fields.ToArray());
          members.AddRange(curType.Methods.ToArray());
          members.AddRange(curType.Events.ToArray());
          AttachSubTree(assemblyTree.Model, iter, members.ToArray());
          selectionArgs = new SelectionEventArgs(curType, curModule, curAssembly);
          SelectionChangedEvent(o, selectionArgs);
          break;
        case 4:
          curMember = currentObj as MemberReference;
          Debug.Assert(curMember != null, "MemberDef is null!?");
          selectionArgs = new SelectionEventArgs(curMember, curModule, curAssembly);
          SelectionChangedEvent(o, selectionArgs);
          //DumpMember(memberDef);
          break;
      }
      assemblyTree.ShowAll();
      //ShowAll();
    }

    /// <summary>
    /// Attaches a submodel to the tree view's model.
    /// </summary>
    /// <param name='model'>
    /// The root model to be attached to.
    /// </param>
    /// <param name='parent'>[ref]
    /// The pointer where the elements to be attached to.
    /// </param>
    /// <param name='elements'>
    /// Elements.
    /// </param>
    protected void AttachSubTree(Gtk.TreeModel model, Gtk.TreeIter parent, object[] elements)
    {
      Gtk.TreeStore store = model as Gtk.TreeStore;
      Debug.Assert(store != null, "TreeModel shouldn't be flat");

      // remove the values if they were added before.
      Gtk.TreePath path = store.GetPath(parent);
      path.Down();
      Gtk.TreeIter iter;
      while (store.GetIter(out iter, path))
        store.Remove(ref iter);

      // Add the elements to the tree view.
      for (uint i = 0; i < elements.Length; ++i) {
        store.AppendValues(parent, elements[i]);
      }
    }

    private void LoadEnvironment() {
      string curEnv = System.IO.Path.Combine(reflector.GetConfigurationDirectory(),
                                             "Assemblies.env");
      if (File.Exists(curEnv)) {
        LoadFilesInTreeView(File.ReadAllLines(curEnv));
      }
    }

    private void LoadFilesInTreeView(string [] files) {
      Gtk.TreeViewColumn col = new Gtk.TreeViewColumn();
      Gtk.CellRendererText colAssemblyCell = new Gtk.CellRendererText();

      col.PackStart(colAssemblyCell, true);
      col.AddAttribute(colAssemblyCell, "text", 0);

      if (assemblyTree.GetColumn(0) != null)
        assemblyTree.Columns[0] = col;
      else
        assemblyTree.AppendColumn(col);

      Gtk.TreeStore store = assemblyTree.Model as Gtk.TreeStore;
      if (store == null)
        store = new Gtk.TreeStore(typeof(object));

      foreach (string file in files) {
        if (File.Exists(file))
          store.AppendValues(AssemblyDefinition.ReadAssembly(file));
        else
          ShowMessageGtk(String.Format("File {0} doesn't exits.", file));
      }

      // Add functions managinig the visualization of those assembly definitions
      col.SetCellDataFunc(colAssemblyCell, new Gtk.TreeCellDataFunc(RenderAssemblyDefinition));

      assemblyTree.Model = store;
      assemblyTree.ShowAll();
    }
    #endregion

    private void RenderAssemblyDefinition(Gtk.TreeViewColumn column, Gtk.CellRenderer cell,
                                          Gtk.TreeModel model, Gtk.TreeIter iter) {
      object curObject = model.GetValue(iter, 0);
      switch (model.GetPath(iter).Depth) {
      // Assemblies
      case 1:
        AssemblyDefinition aDef = curObject as AssemblyDefinition;
        Debug.Assert(aDef != null, "Must have assembly.");
        (cell as Gtk.CellRendererText).Text =  aDef.Name.Name;
        break;
      // Modules
      case 2:
        ModuleDefinition modDef = curObject as ModuleDefinition;
        Debug.Assert(modDef != null, "Must have module.");
        (cell as Gtk.CellRendererText).Text =  modDef.Name;
        break;
      // Types
      case 3:
        TypeDefinition tDef = curObject as TypeDefinition;
        Debug.Assert(tDef != null, "Must have type (definition).");
        (cell as Gtk.CellRendererText).Text =  tDef.Name;
        break;
      // Methods
      case 4:
        IMemberDefinition memberDef = curObject as IMemberDefinition;
        Debug.Assert(memberDef != null, "Must have member.");
        (cell as Gtk.CellRendererText).Text =  memberDef.Name;
        break;
      }
    }

    /// <summary>
    /// Shows message box using GTK.
    /// </summary>
    /// <param name='msg'>
    /// The message.
    /// </param>
    private void ShowMessageGtk(string msg) {
      var msgBox = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Info,
                                         Gtk.ButtonsType.Ok, msg);
      msgBox.Run();
      msgBox.Destroy();
    }
  }
}