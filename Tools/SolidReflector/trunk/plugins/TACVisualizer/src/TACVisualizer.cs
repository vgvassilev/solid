using System;
using Gtk;

using SolidOpt.Services;
using SolidReflector.Plugins.AssemblyBrowser;
using SolidV.Ide.Dock;

namespace SolidReflector.Plugins.TACVisualizer
{
  public class TACVisualizer : IPlugin
  {
    private MainWindow mainWindow = null;
    private Gtk.TextView textView = new Gtk.TextView();
    private DockItem tacVisualizingDock = null;

    public TACVisualizer() { }

    #region IPlugin implementation
    void IPlugin.Init(object context)
    {
      ISolidReflector reflector = context as ISolidReflector;
      mainWindow = reflector.GetMainWindow();
      IAssemblyBrowser browser = reflector.GetPlugins().GetService<IAssemblyBrowser>();
      browser.SelectionChanged += OnSelectionChanged;

      Gtk.Notebook nb = new Gtk.Notebook();
      nb.AppendPage(textView, new Gtk.Label("TAC Text"));
      nb.ShowAll();

      ScrolledWindow scrollWindow = new ScrolledWindow();
      Viewport viewport = new Viewport();
      scrollWindow.Add(viewport);
      viewport.Add(nb);
      scrollWindow.ShowAll();

      tacVisualizingDock = mainWindow.DockFrame.AddItem("TACVisualizer");
      tacVisualizingDock.DrawFrame = true;
      tacVisualizingDock.Label = "TAC Visualizer";
      tacVisualizingDock.Content = scrollWindow;
      tacVisualizingDock.DefaultVisible = true;
      tacVisualizingDock.Visible = true;
    }

    void IPlugin.UnInit(object context)
    {
      tacVisualizingDock.Visible = false;
      // BUG: Object not set to an instance of an object exception if only one plugin is loaded
      // and attempted to be UnInit-ed
      mainWindow.DockFrame.RemoveItem(tacVisualizingDock);
    }
    #endregion

    private void OnSelectionChanged(object sender, SelectionEventArgs args) {
      if (args.definition != null) {
        // Dump the definition
        TACPrettyPrinter.PrintPretty(args.definition, textView);
        if (args.module != null) {
          // Dump the module
          if (args.assembly != null) {
            // Dump assembly modules.
          }
        }
      }
    }
  }
}