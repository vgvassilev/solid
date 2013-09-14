using Mono.Cecil;
using MonoDevelop.Components.Docking;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using SolidOpt.Services;

namespace SolidReflector.Plugins.AssemblyBrowser
{
  /// <summary>
  /// Assembly browser plugin representing the assembly structure in tree like form.
  /// </summary>
  /// 
  public class AssemblyBrowser : IPlugin, IAssemblyBrowser
  {
    /// <summary>
    /// The main application window.
    /// </summary>
    private MainWindow mainWindow = null;
    /// <summary>
    /// The main application instance.
    /// </summary>
    /// 
    private ISolidReflector reflector = null;

    /// <summary>
    /// The dock item that will be attached to the DockFrame from the main application.
    /// </summary>
    /// 
    private DockItem dockItem;

    /// <summary>
    /// The TreeView component representing the assembly structure.
    /// </summary>
    /// 
    private Gtk.TreeView assemblyTree = new Gtk.TreeView();

    /// <summary>
    /// The AssemblyDefinition data of the currently reviewed assembly.
    /// </summary>
    /// 
    private AssemblyDefinition curAssembly = null;

    /// <summary>
    /// The ModuleDefinition data of the currently reviewed assembly.
    /// </summary>
    /// 
    private ModuleDefinition curModule = null;

    /// <summary>
    /// The MemberReference data of the currently reviewed assembly.
    /// </summary>
    /// 
    private MemberReference curMember = null;
    private Gtk.TreeIter iter;
    private TypeDefinition curType = null;

    public AssemblyBrowser() { }

    #region IAssemblyBrowser implementation
    event EventHandler<SelectionEventArgs> SelectionChangedEvent = null;

    /// <summary>
    /// Sent to the subscribed plugins notifying them the selection in the TreeView is changed.
    /// </summary>
    /// 
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

    /// <summary>
    /// Gets the AssemblyBrowser plugin.
    /// </summary>
    /// <returns>
    /// The AssemblyBrowser plugin.
    /// </returns>
    AssemblyBrowser IAssemblyBrowser.GetAssemblyBrowser()
    {
      return this;
    }

    #endregion

    #region IPlugin implementation
    void IPlugin.Init(object context)
    {
      reflector = context as ISolidReflector;

      mainWindow = reflector.GetMainWindow();
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

      // Setting up the Open menu item in File
      Gtk.MenuItem open = new Gtk.MenuItem("Open");
      open.Activated += OnActivated;
      (fileMenu.Submenu as Gtk.Menu).Prepend(open);

      // Attaching the current dockItem in the DockFrame
      dockItem = mainWindow.DockFrame.AddItem("AssemblyBrowser");
      dockItem.DrawFrame = true;
      dockItem.Label = "Assembly";
      dockItem.Content = assemblyTree;

      LoadEnvironment();

      assemblyTree.RowActivated += HandleRowActivated;

      assemblyTree.Realized += delegate(object sender, EventArgs e) {
        LoadSelectedAssembliesTreePaths();
      };
    }

    void IPlugin.UnInit(object context)
    {
      dockItem.Visible = false;
      // BUG: Object not set to an instance of an object exception if only one plugin is loaded
      // and attempted to be UnInit-ed
      mainWindow.DockFrame.RemoveItem(dockItem);
    }

    #endregion

    /// <summary>
    /// Saves the loaded assemblies when the shut down event is received.
    /// </summary>
    /// <param name='sender'>
    /// The MainWindow.
    /// </param>
    /// <param name='e'>
    /// E.
    /// </param>
    /// 
    void HandleOnShutDown (object sender, EventArgs e)
    {
      SaveLoadedAssemblies();
    }

    /// <summary>
    /// Saves the list of loaded assemblies in the Assemblies.env
    /// located in the main app configuration directory.
    /// </summary>
    /// 
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

    /// <summary>
    /// Saves the loaded assemblies file paths in the Assemblies.env
    /// and the currently selected assembly tree path.
    /// </summary>
    /// <param name='curEnv'>
    /// The path to the Assemlies.env.
    /// </param>
    /// <param name='items'>
    /// The list of file paths that have to be written in the curEnv.
    /// </param>
    /// 
    private void saveLoadedAssembliesData(string curEnv, List<string> items) {
      FileStream file = new FileStream(curEnv, FileMode.OpenOrCreate, FileAccess.ReadWrite);
      using (StreamWriter writer = new StreamWriter(file)) {
        writer.Write("");
        writer.Flush();
        foreach (string line in items)
          writer.WriteLine(line);
        writer.Flush();
      }

      SaveSelectedAssembliesTreePaths();
    }

    /// <summary>
    /// Raises the activated event when then Open menu item is invoked.
    /// Adds an assembly to the assemblyTree.
    /// </summary>
    /// <param name='sender'>
    /// The Gtk.MenuItem.
    /// </param>
    /// <param name='args'>
    /// Arguments.
    /// </param>
    /// 
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

    /// <summary>
    /// Displays the proper assembly definition according to the depth of the activated row.
    /// </summary>
    /// <param name='o'>
    /// The Gtk.TreeView assemblyTree.
    /// </param>
    /// <param name='args'>
    /// Arguments.
    /// </param>
    void HandleRowActivated (object o, Gtk.RowActivatedArgs args)
    {
      //SelectionEventArgs selectionArgs = new SelectionEventArgs();
      //SelectionChangedEvent(o, selectionArgs);

      assemblyTree.Model.GetIter(out iter, args.Path);

      object currentObj = (object) assemblyTree.Model.GetValue(iter, 0);
      SelectionEventArgs selectionArgs;

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
          curType = currentObj as TypeDefinition;
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

    /// <summary>
    /// Loads the assemblies from the Assemblies.env.
    /// </summary>
    /// 
    private void LoadEnvironment() {
      string curEnv = System.IO.Path.Combine(reflector.GetConfigurationDirectory(),
                                             "Assemblies.env");
      if (File.Exists(curEnv)) {
        LoadFilesInTreeView(File.ReadAllLines(curEnv));
      }
    }

    /// <summary>
    /// Loads the files in the Assemblies.env into the TreeView.
    /// </summary>
    /// <param name='files'>
    /// Files.
    /// </param>
    /// 
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

    /// <summary>
    /// Saves the TreePaths (expanded rows) of the selected assemblies in a file.
    /// </summary>
    ///
    private void SaveSelectedAssembliesTreePaths() {
      Gtk.TreePath[] paths = assemblyTree.Selection.GetSelectedRows();
      
      FileStream file = new FileStream(Path.Combine(reflector.GetConfigurationDirectory(),
                                       "SolidReflector.session"), FileMode.Create,
                                       FileAccess.ReadWrite);

      using (StreamWriter writer = new StreamWriter(file)) {
        foreach (Gtk.TreePath path in paths)
          writer.Write(path.ToString());
        
        writer.Flush();
      }
    }

    /// <summary>
    /// Restores the TreePaths (expanded rows) from a file
    /// </summary>
    ///
    private void LoadSelectedAssembliesTreePaths() {
      string filepath = Path.Combine(reflector.GetConfigurationDirectory(), "SolidReflector.session");
      string[] lines = null;
      if (File.Exists(filepath)) {
        lines = File.ReadAllLines(filepath);
        foreach (string treePath in lines) {
          expandPath(treePath);
        }
      }
    }

    /// <summary>
    /// Incrementally expands the given TreePath
    /// because the lazy loaded rows do not support regular expanding.
    /// </summary>
    /// <param name="path">Path.</param>
    /// 
    private void expandPath(string path) {
      string[] pathNodes = path.Split(':');
      string currentPath = pathNodes[0];
      for (int i = 1; i < pathNodes.Length; i++) {
        assemblyTree.ActivateRow(new Gtk.TreePath(currentPath), assemblyTree.GetColumn(0));
        assemblyTree.ExpandRow(new Gtk.TreePath(currentPath), true);
        currentPath += ":" + pathNodes[i];
      }
    }

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