using Gtk;
using MonoDevelop.Components.Docking;
using System;

using SolidOpt.Services;
using SolidReflector.Plugins.AssemblyBrowser;

namespace SolidReflector.Plugins.ILVisualizer
{
  public class ILVisualizer : IPlugin
  {
    private MainWindow mainWindow;
    private DockItem ilVisualizingDock = null;

    public ILVisualizer() { }

    #region IPlugin implementation
    void IPlugin.Init(object context)
    {
      ISolidReflector reflector = context as ISolidReflector;
      mainWindow = reflector.GetMainWindow();
      IAssemblyBrowser browser = reflector.GetPlugins().GetService<IAssemblyBrowser>();
      browser.SelectionChanged += OnSelectionChanged;

      Gtk.Notebook nb = new Gtk.Notebook();
      nb.AppendPage( new TextView(), new Gtk.Label("IL Text"));
      nb.ShowAll();

      ilVisualizingDock = mainWindow.DockFrame.AddItem("ILVisualizer");
      ilVisualizingDock.Expand = true;
      ilVisualizingDock.DrawFrame = true;
      ilVisualizingDock.Label = "IL Visualizer";
      ilVisualizingDock.Content = nb;
      ilVisualizingDock.DefaultVisible = true;
      ilVisualizingDock.Visible = true;
    }

    void IPlugin.UnInit(object context)
    {
      ilVisualizingDock.Visible = false;
      // BUG: Object not set to an instance of an object exception if only one plugin is loaded
      // and attempted to be UnInit-ed
      mainWindow.DockFrame.RemoveItem(ilVisualizingDock);
    }
    #endregion

    private void OnSelectionChanged(object sender, SelectionEventArgs args) {
      Gtk.Notebook nb = ilVisualizingDock.Content as Gtk.Notebook;
      Gtk.TextView textView = nb.CurrentPageWidget as Gtk.TextView;
      if (args.definition != null) {
        // Dump the definition
        ILPrettyPrinter.PrintPretty(args.definition, textView);
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