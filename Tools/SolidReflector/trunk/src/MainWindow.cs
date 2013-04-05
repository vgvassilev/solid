/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;
using System.IO;

using MonoDevelop.Components.Docking;

using SolidOpt.Services;
using SolidReflector.Plugins;


public partial class MainWindow: Gtk.Window, ISolidReflector
{
  DockFrame dockFrame = new DockFrame();
  public DockFrame DockFrame {
    get { return dockFrame; }
  }

  private PluginServiceContainer plugins = new PluginServiceContainer();

  public MainWindow(): base(Gtk.WindowType.Toplevel)
  {
     // Register the MainWindow as service so that it can be passed to the rest of the plugins
     // so that they could attach themselves in the menus.
     plugins.AddService((ISolidReflector)this);

     // That's a hack because of the designer. If one needs to attach an event the designer attaches
     // it in the end of the file after the call to Initialize. Works for the most of the events
     // but not for events like Realize which happen in the initialization. This function is used
     // to attach the event handlers before the initialization part.
     PreBuild();
     Build();

     //SetSizeRequest(800, 600);

     vbox1.Add(dockFrame);
         
     if (File.Exists("config.layout"))
       dockFrame.LoadLayouts("config.layout");
     else
       dockFrame.CreateLayout("test", true);
  
     dockFrame.HeightRequest = vbox1.Allocation.Height;
     dockFrame.CurrentLayout = "test";
     dockFrame.HandlePadding = 0;
     dockFrame.HandleSize = 2;

     this.ShowAll();
   }

   protected void OnDeleteEvent(object sender, Gtk.DeleteEventArgs a)
   {
      //SaveEnvironment();
      Gtk.Application.Quit();
      a.RetVal = true;
   }

   protected void OnExitActionActivated(object sender, System.EventArgs e)
   {
      //SaveEnvironment();
      ShutDownEvent(this, new EventArgs());
      Gtk.Application.Quit();
   }

  protected void OnRealized(object sender, System.EventArgs e)
  {
    LoadRegisteredPlugins();
  }

  private void SaveEnvironment() {
    List<string> filesLoaded = null;
    //GetLoadedFiles(out filesLoaded);

    saveEnvironmentData(System.IO.Path.Combine(Environment.CurrentDirectory, "Current.env"),
                        filesLoaded);
    saveEnvironmentData(System.IO.Path.Combine(Environment.CurrentDirectory, "Plugin.env"),
                        plugins.plugins.ConvertAll<string>(x => x.ToString()));
  }

  private void saveEnvironmentData(string curEnv, List<string> items) {
    FileStream file = new FileStream(curEnv, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    using (StreamWriter writer = new StreamWriter(file)) {
      writer.Write("");
      writer.Flush();
      foreach (string line in items)
        writer.WriteLine(line);
      writer.Flush();
    }
  }

  private void LoadRegisteredPlugins() {
    string registeredPlugins =
        System.IO.Path.Combine(Environment.CurrentDirectory, "Plugins.env");
    if (File.Exists(registeredPlugins)) {
      foreach (string s in File.ReadAllLines(registeredPlugins))
        if (File.Exists(s))
          plugins.AddPlugin(s);
    }

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

  Gtk.MenuBar ISolidReflector.GetMainMenu()
  {
    return this.MainMenuBar;
  }

  MainWindow SolidReflector.Plugins.ISolidReflector.GetMainWindow()
  {
    return this;
  }

  PluginServiceContainer SolidReflector.Plugins.ISolidReflector.GetPlugins() {
    return this.plugins;
  }
  #endregion
}