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

     vbox1.Add(dockFrame);
          
     if (File.Exists(System.IO.Path.Combine(Environment.CurrentDirectory, "config.layout")))
       dockFrame.LoadLayouts(System.IO.Path.Combine(Environment.CurrentDirectory,"config.layout"));
     else {
       dockFrame.CreateLayout("BasicLayout", true);
     }
  
     dockFrame.HeightRequest = vbox1.Allocation.Height;
     dockFrame.CurrentLayout = "BasicLayout";
     dockFrame.HandlePadding = 0;
     dockFrame.HandleSize = 2;

     this.ShowAll();
   }

   protected void OnDeleteEvent(object sender, Gtk.DeleteEventArgs a)
   {
      Gtk.Application.Quit();
      a.RetVal = true;
   }

   protected void OnExitActionActivated(object sender, System.EventArgs e)
   {
      SaveLayout();

      ShutDownEvent(this, new EventArgs());
      Gtk.Application.Quit();
   }

  protected void OnRealized(object sender, System.EventArgs e)
  {
    LoadRegisteredPlugins();
  }

  /// <summary>
  /// Saves the current window components layout and their last changed state.
  /// </summary>
  private void SaveLayout()
  {
    if (!File.Exists(System.IO.Path.Combine(Environment.CurrentDirectory,"config.layout")))
      File.Create(System.IO.Path.Combine(Environment.CurrentDirectory,"config.layout")).Dispose();

    dockFrame.SaveLayouts(System.IO.Path.Combine(Environment.CurrentDirectory,"config.layout"));
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