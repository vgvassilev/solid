/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using MonoDevelop.Components.Docking;
using System;
using System.IO;
using System.Reflection;

using SolidOpt.Services;
using SolidReflector.Plugins;


public partial class MainWindow: Gtk.Window, ISolidReflector
{
  /// <summary>
  /// The collection of loaded plugins.
  /// </summary>
  /// 
  private PluginServiceContainer plugins = new PluginServiceContainer();

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
  }

  /// <summary>
  /// The main application window.
  /// </summary>
  /// 
  public MainWindow(): base(Gtk.WindowType.Toplevel)
  {
    // Register the MainWindow as service so that it can be passed to the rest of the plugins
    // so that they could attach themselves in the menus.
    plugins.AddService((ISolidReflector)this);

    string confDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    applicationDataDir = System.IO.Path.Combine(confDir, 
                                                Assembly.GetExecutingAssembly().GetName().Name);
    pluginsDir = System.IO.Path.Combine(applicationDataDir, "plugins");

    if (!Directory.Exists(applicationDataDir))
      Directory.CreateDirectory(applicationDataDir);

    if (!Directory.Exists(pluginsDir))
      Directory.CreateDirectory(pluginsDir);

    // That's a hack because of the designer. If one needs to attach an event the designer attaches
    // it in the end of the file after the call to Initialize. Works for the most of the events
    // but not for events like Realize which happen in the initialization. This function is used
    // to attach the event handlers before the initialization part.
    PreBuild();
    Build();

    hbox1.Add(dockFrame);

    // Handling the application startup layout.
    // If layout file is found the dock items would be restored as they were before the shut down.
    layoutFile = System.IO.Path.Combine(applicationDataDir, "config.layout");
    if (File.Exists(layoutFile))
      dockFrame.LoadLayouts(System.IO.Path.Combine(applicationDataDir, "config.layout"));
    else {
      dockFrame.CreateLayout("BasicLayout", true);
    }

    dockFrame.HeightRequest = hbox1.Allocation.Height;
    dockFrame.WidthRequest = hbox1.Allocation.Width;
    dockFrame.CurrentLayout = "BasicLayout";
    dockFrame.HandlePadding = 0;
    dockFrame.HandleSize = 4;

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
    SaveLayout();
    ShutDownEvent(this, new EventArgs());

    Gtk.Application.Quit();
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
  }

  /// <summary>
  /// Saves the current window components layout and their last changed state.
  /// </summary>
  /// 
  private void SaveLayout()
  {
    // Save all loaded plugins
    plugins.SavePluginsToFile(pluginsFile);

    if (!File.Exists(layoutFile))
      File.Create(layoutFile).Close();

    dockFrame.SaveLayouts(layoutFile);
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
    this.Realized += new global::System.EventHandler (this.OnRealized);
  }

  #region ISolidReflector implementation
  event EventHandler<EventArgs> ShutDownEvent = null;

  /// <summary>
  /// Sent to the subscribed plugins notifying them the main application is terminating.
  /// </summary>
  /// 
  event EventHandler<EventArgs> ISolidReflector.OnShutDown {
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
  string ISolidReflector.GetConfigurationDirectory()
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
  Gtk.MenuBar ISolidReflector.GetMainMenu()
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
  MainWindow SolidReflector.Plugins.ISolidReflector.GetMainWindow()
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
  PluginServiceContainer SolidReflector.Plugins.ISolidReflector.GetPlugins() {
    return this.plugins;
  }
  #endregion
}