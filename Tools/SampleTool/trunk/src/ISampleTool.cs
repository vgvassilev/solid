using System;

using SolidOpt.Services;

namespace SampleTool
{
  /// <summary>
  /// Exposes methods and events an application has to implement in order to be compatible with
  /// the SolidReflector/DataMorphose plugins
  /// </summary>
  /// 
  public interface ISampleTool : IService
  {
    /// <summary>
    /// Occurs when the main application (SolidReflector, DataMorphose) is requested to terminate.
    /// The event is sent to the subscribed plugins allowing them to do some work before the
    /// application is terminated.
    /// </summary>
    event EventHandler<EventArgs> OnShutDown;

    /// <summary>
    /// Gets the current user configuration directory.
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