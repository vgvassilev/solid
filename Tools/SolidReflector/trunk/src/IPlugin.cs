using System;

using SolidOpt.Services;

namespace SolidReflector.Plugins
{
  public interface IPlugin : IService
  {
    void Init(ISolidReflector reflector);
  }

  public interface ISolidReflector : IService
  {
    event EventHandler<EventArgs> OnShutDown;

    /// <summary>
    /// Gets the current user's configuration directory.
    /// </summary>
    /// <description>
    /// On Unix it is /home/user/.config/SolidReflector
    /// On Mac it is /Users/user/.config/SolidReflector
    /// On Windows it is in AppData
    /// </description>
    /// <returns>
    /// The configuration directory.
    /// </returns>
    ///
    string GetConfigurationDirectory();

    /// <summary>
    /// Gets the application main window.
    /// </summary>
    /// <returns>
    /// The main window.
    /// </returns>
    ///
    //FIXME: Must return Gtk.Window
    MainWindow GetMainWindow();

    /// <summary>
    /// Gets the application main menu.
    /// </summary>
    /// <returns>
    /// The main menu.
    /// </returns>
    /// 
    Gtk.MenuBar GetMainMenu();

    /// <summary>
    /// Gets the currently loaded plugins in the application.
    /// </summary>
    /// <returns>
    /// The plugins.
    /// </returns>
    /// 
    PluginServiceContainer GetPlugins();
  }
}