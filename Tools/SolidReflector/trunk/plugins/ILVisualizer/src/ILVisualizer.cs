using Gtk;
using MonoDevelop.Components.Docking;
using System;

using SolidOpt.Services;
using SolidReflector.Plugins.AssemblyBrowser;

namespace SolidReflector.Plugins.ILVisualizer
{
  public class ILVisualizer : IPlugin
  {
    private DockItem ilVisualizingDock = null;

    public ILVisualizer() { }

    #region IPlugin implementation
    void IPlugin.Init(object context)
    {
      ISolidReflector reflector = context as ISolidReflector;
      var MainWindow = reflector.GetMainWindow();
      IAssemblyBrowser browser = reflector.GetPlugins().GetService<IAssemblyBrowser>();
      browser.SelectionChanged += OnSelectionChanged;

      Gtk.Notebook nb = new Gtk.Notebook();
      nb.AppendPage( new TextView(), new Gtk.Label("IL Text"));
      nb.ShowAll();

      ilVisualizingDock = MainWindow.DockFrame.AddItem("ILVisualizer");
      ilVisualizingDock.Expand = true;
      ilVisualizingDock.DrawFrame = true;
      ilVisualizingDock.Label = "IL Visualizer";
      ilVisualizingDock.Content = nb;
      ilVisualizingDock.DefaultVisible = true;
      ilVisualizingDock.Visible = true;
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