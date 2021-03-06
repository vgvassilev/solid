
/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using MonoDevelop.Components.Docking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using SolidOpt.Services;
using DataMorphose;
using DataMorphose.Model;


public partial class MainWindow: Gtk.Window, IDataMorphose
{ 
  /// <summary>
  /// The collection of loaded plugins.
  /// </summary>
  /// 
  private PluginServiceContainer plugins = new PluginServiceContainer();
  
  private DataModel model = new DataModel(null);
  
  /// <summary>
  /// The application data dir located in the OS specific configuration dir.
  /// </summary>
  /// 
  private string applicationDataDir = "";
  
  /// <summary>
  /// The dir holding the user created plugins located in the OS specific configuration dir.
  /// </summary>
  /// 
  private string pluginsDir = "";
  
  /// <summary>
  /// The file containing the locations of plugins that have to be loaded.
  /// </summary>
  /// 
  private string pluginsFile = "Plugins.env";
  
  private string PluginsFilePath = null;
  
  /// <summary>
  /// The layout file containing the dock items state and placement.
  /// </summary>
  /// 
  private string layoutFile = "";
  
  /// <summary>
  /// The dock frame that contains the dock items.
  /// </summary>
  /// 
  private DockFrame dockFrame = new DockFrame();
  public DockFrame DockFrame {
    get { return dockFrame; }
    set { dockFrame = value; }
  }

  /// <summary>
  /// The main application window.
  /// </summary>
  /// 
  public MainWindow(): base(Gtk.WindowType.Toplevel)
  {
    // Register the MainWindow as service so that it can be passed to the rest of the plugins
    // so that they could attach themselves in the menus.
    plugins.AddService((IDataMorphose)this);
    
    string confDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    applicationDataDir = System.IO.Path.Combine(confDir, 
                                                Assembly.GetExecutingAssembly().GetName().Name);
    pluginsDir = System.IO.Path.Combine(applicationDataDir, "plugins");
    
    if (!Directory.Exists(applicationDataDir))
      Directory.CreateDirectory(applicationDataDir);
    
    if (!Directory.Exists(pluginsDir))
      Directory.CreateDirectory(pluginsDir);
    
    PluginsFilePath = System.IO.Path.Combine(pluginsDir, pluginsFile);
    if (!File.Exists(PluginsFilePath)) {
      File.Create(PluginsFilePath).Close();
    }
    
    layoutFile = System.IO.Path.Combine(applicationDataDir, "config.layout");

    Build();

    OnRealized(this, new EventArgs());
    dockFrame.CurrentLayout = "BasicLayout";
    hbox1.Add(dockFrame);

    this.ShowAll();
  }

  /// <summary>
  /// Raises the delete event when the user attempts to close the application
  /// using the close button from the title bar.
  /// </summary>
  /// <param name='sender'>
  /// The application window (MainWindow).
  /// </param>
  /// <param name='args'>
  /// Arguments.
  /// </param>
  /// 
  protected void OnDeleteEvent(object sender, Gtk.DeleteEventArgs args)
  {
    Gtk.Application.Quit();
    args.RetVal = true;
  }
  
  /// <summary>
  /// Raises the realized event. The plugins are loaded when the MainWindow is realized.
  /// </summary>
  /// <param name='sender'>
  /// The application window (MainWindow).
  /// </param>
  /// <param name='args'>
  /// Arguments.
  /// </param>
  /// 
  protected void OnRealized(object sender, System.EventArgs args)
  {
    LoadRegisteredPlugins();
    LoadLayout();
  }
  
  /// <summary>
  /// Raises the exit action activated event when the user attempts to close the application using
  /// the main menu 'Exit' menu item. Saves the current dock layout and sends the shut down event.
  /// </summary>
  /// <param name='sender'>
  /// The application window (MainWindow).
  /// </param>
  /// <param name='args'>
  /// Arguments.
  /// </param>
  /// 
  protected void OnExitActionActivated(object sender, System.EventArgs args)
  {
    SaveLayout(); // FIXME: This produces a bogus layout file
    if (ShutDownEvent != null)
      ShutDownEvent(this, args);
    
    Gtk.Application.Quit();
  }
  
  // Handling the application startup layout.
  // If layout file is found the dock items would be restored as they were before the shut down.
  private void LoadLayout() {
    if (File.Exists(layoutFile)) {
      dockFrame.LoadLayouts(System.IO.Path.Combine(applicationDataDir, "config.layout"));
    }
    else {
      dockFrame.CreateLayout("BasicLayout", /*copyCurrent*/true);
    }
  }
  
  /// <summary>
  /// Saves the current window components layout and their last changed state.
  /// </summary>
  /// 
  private void SaveLayout()
  {
    // Save all loaded plugins
    plugins.SavePluginsToFile(pluginsFile);
    
    File.Delete(layoutFile);
    File.Create(layoutFile).Close();
    
    dockFrame.SaveLayouts(layoutFile);
  }
  
  protected void OnRemoveActionActivated (object sender, EventArgs e)
  {
    Gtk.ResponseType responseType = Gtk.ResponseType.None;
    LoadedPlugins loadedPlugins = new LoadedPlugins(plugins);
    try {
      responseType = (Gtk.ResponseType) loadedPlugins.Run();
      
      if (responseType == Gtk.ResponseType.Ok) {
        Gtk.TreeModel model;
        Gtk.TreeIter iter;
        Gtk.TreeSelection selection = loadedPlugins.treeView.Selection;
        
        selection.GetSelected(out model, out iter);
        
        string pluginToDelete = (model.GetValue(iter, 0).ToString());
        removePlugin(pluginToDelete);
        //plugins.RemovePlugin(pluginToDelete);
        //DockItem item = DockFrame.GetItem(pluginToDelete);
        //item.Visible = false;
        //dockFrame.RemoveItem(item);
        SaveLayout();
      }
    }
    finally {
      loadedPlugins.Hide();
      loadedPlugins.Dispose();
    }
  }
  
  private void removePlugin(string pluginToDelete)
  {
    foreach (IPlugin plugin in plugins.GetServices<IPlugin>()) {
      if (System.IO.Path.GetFileName(plugin.ToString()) == pluginToDelete) {
        plugin.UnInit(null);
      }
    }
    //plugins.RemovePlugin(pluginToDelete);
  }
  
  /// <summary>
  /// Loads the plugins from the Plugins.env.
  /// </summary>
  /// 
  private void LoadRegisteredPlugins() {
    plugins.AddPluginsFromFile(pluginsFile);
    plugins.LoadPlugins();
    
    foreach (IPlugin p in plugins.GetServices<IPlugin>()) {
      p.Init(this);
    }
  }
  
  #region IDataMorphose implementation
  event EventHandler<EventArgs> ShutDownEvent = null;
  
  /// <summary>
  /// Sent to the subscribed plugins notifying them the main application is terminating.
  /// </summary>
  /// 
  event EventHandler<EventArgs> IDataMorphose.OnShutDown {
    add {
      if (ShutDownEvent == null)
        ShutDownEvent += value;
      else
        ShutDownEvent = new EventHandler<EventArgs>(value);
    } remove {
      if (ShutDownEvent != null)
        ShutDownEvent -= value;
    }
  }
  
  /// <summary>
  /// Gets the OS specific configuration directory.
  /// </summary>
  /// <returns>
  /// The OS specific configuration directory.
  /// </returns>
  /// 
  string IDataMorphose.GetConfigurationDirectory()
  {
    return this.applicationDataDir;
  }
  
  /// <summary>
  /// Gets the main application main menu.
  /// </summary>
  /// <returns>
  /// The main application main menu.
  /// </returns>
  /// 
  Gtk.MenuBar IDataMorphose.GetMainMenu()
  {
    return this.MainMenuBar;
  }
  
  /// <summary>
  /// Gets the main window (MainWindow).
  /// </summary>
  /// <returns>
  /// The main window (MainWindow).
  /// </returns>
  /// 
  MainWindow IDataMorphose.GetMainWindow()
  {
    return this;
  }
  
  /// <summary>
  /// Gets the collection of currently loaded plugins in the main application.
  /// </summary>
  /// <returns>
  /// The collection of plugins.
  /// </returns>
  /// 
  PluginServiceContainer IDataMorphose.GetPlugins() {
    return this.plugins;
  }
  
  DataModel IDataMorphose.GetModel()
  {
    return model;
  }
  
  #endregion
  
  protected void OnAddActionActivated (object sender, EventArgs e)
  {
    var fc = new Gtk.FileChooserDialog("Choose the file to open", null, 
                                       Gtk.FileChooserAction.Open, "Cancel", 
                                       Gtk.ResponseType.Cancel, "Open", Gtk.ResponseType.Accept);
    try {
      fc.SelectMultiple = true;
      fc.SetCurrentFolder(Environment.CurrentDirectory);
      if (fc.Run() == (int)Gtk.ResponseType.Accept) {
        for (int i = 0; i < fc.Filenames.Length; i++) {
          plugins.AddPlugin(fc.Filenames[i]);
        }
        SaveLayout();
        plugins.LoadPlugins();
      }
    } finally {
      fc.Destroy();
    }
  }
}
