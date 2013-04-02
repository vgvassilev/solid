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
    MainWindow GetMainWindow();
    Gtk.MenuBar GetMainMenu();
    PluginServiceContainer GetPlugins();
  }
}