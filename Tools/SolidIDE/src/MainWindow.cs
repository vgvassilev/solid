/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using SolidV.Ide.Dock;
using SolidOpt.Services;
//using SolidIDE;

namespace SolidIDE
{
  public partial class MainWindow: Gtk.Window, ISolidIDE
  {
    /// <summary>
    /// The collection of loaded plugins.
    /// </summary>
    /// 
    private PluginServiceContainer plugins = new PluginServiceContainer();
    public PluginServiceContainer Plugins {
      get { return plugins; }
      set { plugins = value; }
    }

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
    /// Flag for disabling application exit
    /// </summary>
    /// 
    private bool isQuitDisabled = false;

    /// <summary>
    /// The main application window.
    /// </summary>
    /// 
    public MainWindow(): base(Gtk.WindowType.Toplevel) {
      // Register the MainWindow as service so that it can be passed to the rest of the plugins
      // so that they could attach themselves in the menus.
      plugins.AddService((ISolidIDE)this);

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

      // That's a hack because of the designer. If one needs to attach an event the designer attaches
      // it in the end of the file after the call to Initialize. Works for the most of the events
      // but not for events like Realize which happen in the initialization. This function is used
      // to attach the event handlers before the initialization part.
      PreBuild();
      Build();

      hbox1.Add(dockFrame);

      dockFrame.HeightRequest = hbox1.Allocation.Height;
      dockFrame.WidthRequest = hbox1.Allocation.Width;
      dockFrame.CurrentLayout = "BasicLayout";
      dockFrame.HandlePadding = 0;
      dockFrame.HandleSize = 4;
      dockFrame.Spacing = 0;

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
    protected void OnDeleteEvent(object sender, Gtk.DeleteEventArgs args) {
      args.RetVal = !isQuitDisabled;

      if (!isQuitDisabled)
        Gtk.Application.Quit();

      isQuitDisabled = false;
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
    protected void OnRealized(object sender, System.EventArgs args) {
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
    protected void OnExitActionActivated(object sender, System.EventArgs args) {
      SaveLayout();
      if (ShutDownEvent != null)
        ShutDownEvent(this, args);

      if (!isQuitDisabled)
        Gtk.Application.Quit();

      isQuitDisabled = false;
    }

    // Handling the application startup layout.
    // If layout file is found the dock items would be restored as they were before the shut down.
    private void LoadLayout() {
      if (File.Exists(layoutFile)) {
        dockFrame.LoadLayouts(System.IO.Path.Combine(applicationDataDir, "config.layout"));
      }
      else {
        dockFrame.CreateLayout("BasicLayout", true);
      }
    }

    /// <summary>
    /// Saves the current window components layout and their last changed state.
    /// </summary>
    /// 
    private void SaveLayout() {
      // Save all loaded plugins
      plugins.SavePluginsToFile(pluginsFile);

      File.Delete(layoutFile);
      File.Create(layoutFile).Close();

      dockFrame.SaveLayouts(layoutFile);
    }

    protected void OnRemoveActionActivated(object sender, EventArgs e) {
      Gtk.ResponseType responseType = Gtk.ResponseType.None;
      LoadedPlugins loadedPlugins = new LoadedPlugins(plugins);
      try {
        responseType = (Gtk.ResponseType) loadedPlugins.Run();

        if (responseType == Gtk.ResponseType.Ok) {
          //Gtk.TreeModel treeModel;
          Gtk.TreeIter iter;
          Gtk.TreePath[] selectedRows = null;
          selectedRows = loadedPlugins.treeView.Selection.GetSelectedRows();

          for (int i = 0; i < selectedRows.Length; i++) {
            loadedPlugins.lsLoadedPlugins.GetIter(out iter, selectedRows[i]);
            string pluginPath = (string)loadedPlugins.lsLoadedPlugins.GetValue(iter, 0);
            PluginInfo pluginInfo = plugins.Plugins.Find(x => x.codeBase == pluginPath);
            List<IService> services = plugins.GetServices(pluginInfo);

            foreach (IService service in services) {
              plugins.RemovePlugin(pluginInfo.codeBase);
              (service as IPlugin).UnInit(null);
            }
          }
        }
      }
      finally {
        loadedPlugins.Hide();
      }
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

    protected virtual void PreBuild() {
      this.Realized += new global::System.EventHandler(this.OnRealized);
    }

    /// <summary>
    /// Loads a plugin using the Add command from the Plugins menu.
    /// </summary>
    /// <param name='sender'>
    /// Sender.
    /// </param>
    /// <param name='e'>
    /// Event args.
    /// </param>
    protected void OnAddActionActivated(object sender, EventArgs e) {
      var fc = new Gtk.FileChooserDialog("Choose the file to open", null, 
                                           Gtk.FileChooserAction.Open, "Cancel", 
                                           Gtk.ResponseType.Cancel, "Open", Gtk.ResponseType.Accept);
      try {
        fc.SelectMultiple = true;
        fc.SetCurrentFolder(Environment.CurrentDirectory);
        if (fc.Run() == (int)Gtk.ResponseType.Accept) {
          PluginServiceContainer pluginsToAdd = new PluginServiceContainer();
          for (int i = 0; i < fc.Filenames.Length; i++) {
            if (!plugins.Plugins.Exists(x => x.codeBase == fc.Filenames[i])) {
              plugins.AddPlugin(fc.Filenames[i]);
              pluginsToAdd.AddPlugin(fc.Filenames[i]);
            }
          }

          pluginsToAdd.LoadPlugins();
          foreach (IPlugin p in pluginsToAdd.GetServices<IPlugin>()) {
            p.Init(this);
          }
        }
      } finally {
        fc.Destroy();
      }
    }

    #region ISolidIDE implementation

    event EventHandler<EventArgs> ShutDownEvent = null;

    /// <summary>
    /// Sent to the subscribed plugins notifying them the main application is terminating.
    /// </summary>
    /// 
    event EventHandler<EventArgs> ISolidIDE.OnShutDown {
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
    string ISolidIDE.GetConfigurationDirectory() {
      return this.applicationDataDir;
    }

    /// <summary>
    /// Gets the main application main menu.
    /// </summary>
    /// <returns>
    /// The main application main menu.
    /// </returns>
    /// 
    Gtk.MenuBar ISolidIDE.GetMainMenu() {
      return this.MainMenuBar;
    }

    Gtk.Toolbar ISolidIDE.GetToolbar() {
      return this.Toolbar;
    }

    /// <summary>
    /// Gets the main window (MainWindow).
    /// </summary>
    /// <returns>
    /// The main window (MainWindow).
    /// </returns>
    /// 
    MainWindow ISolidIDE.GetMainWindow() {
      return this;
    }

    /// <summary>
    /// Gets the collection of currently loaded plugins in the main application.
    /// </summary>
    /// <returns>
    /// The collection of plugins.
    /// </returns>
    /// 
    PluginServiceContainer ISolidIDE.GetPlugins() {
      return this.plugins;
    }

    ///
    MenuItemType ISolidIDE.GetMenuItem<MenuItemType>(params string[] names) {
      Gtk.MenuShell menu = (this as ISolidIDE).GetMainMenu();
      Gtk.MenuItem item = null;
      for (int i=0; i<names.Length; i++) {
        item = null;
        foreach (Gtk.Widget w in menu.Children) {
          if (w.Name == names[i] + "Action") {
            item = w as Gtk.ImageMenuItem;
            if (i < names.Length - 1) {
              if (item.Submenu == null)
                item.Submenu = new Gtk.Menu();
              menu = item.Submenu as Gtk.MenuShell;
            }
            break;
          }
        }
        if (item == null) {
          item = new MenuItemType();
          Gtk.AccelLabel accelLabel = new Gtk.AccelLabel("");
          accelLabel.TextWithMnemonic = names[i];
          accelLabel.SetAlignment(0f, 0.5f);
          item.Add(accelLabel);
          accelLabel.AccelWidget = item;
          item.Name = names[i] + "Action";
          menu.Append(item);
          if (i<names.Length-1) item.Submenu = new Gtk.Menu();
          menu = item.Submenu as Gtk.MenuShell;
        }
      }
      return item as MenuItemType;
    }

    ToolItemType ISolidIDE.GetToolbarItem<ToolItemType>(params string[] toolbarNames) {
      //TODO: Find for existing toolbar item
      Gtk.ToolItem item = new ToolItemType();
      item.Name = toolbarNames[0];
      this.Toolbar.Add(item);
      return item as ToolItemType;
    }

    Gtk.ToolItem ISolidIDE.GetToolbarItem(Gtk.ToolItem newItem, params string[] toolbarNames) {
      //TODO: Find for existing toolbar item
      this.Toolbar.Add(newItem);
      return newItem;
    }

    void ISolidIDE.DisableQuit(bool disable) {
      this.isQuitDisabled = disable;
    }

    IServiceContainer ISolidIDE.GetServiceContainer() {
      return this.Plugins;
    }

    #endregion
  }
}
