using MonoDevelop.Components.Docking;
using System;

using SolidOpt.Services;
using SolidReflector.Plugins.AssemblyBrowser;

namespace SolidReflector.Plugins.TACVisualizer
{
  public class TACVisualizer : IPlugin
  {
    private DockItem tacVisualizingDock = null;

    public TACVisualizer() { }

    #region IPlugin implementation
    void IPlugin.Init(object context)
    {
      ISolidReflector reflector = context as ISolidReflector;
      var MainWindow = reflector.GetMainWindow();
      IAssemblyBrowser browser = reflector.GetPlugins().GetService<IAssemblyBrowser>();
      browser.SelectionChanged += OnSelectionChanged;

      Gtk.Notebook nb = new Gtk.Notebook();
      nb.AppendPage(new Gtk.TextView(), new Gtk.Label("TAC Text"));
      nb.ShowAll();

      tacVisualizingDock = MainWindow.DockFrame.AddItem("TACVisualizer");
      tacVisualizingDock.DrawFrame = true;
      tacVisualizingDock.Label = "Three Address Code Visualizer";
      tacVisualizingDock.Content = nb;
      tacVisualizingDock.DefaultVisible = true;
      tacVisualizingDock.Visible = true;
    }
    #endregion

    private void OnSelectionChanged(object sender, SelectionEventArgs args) {
      Gtk.Notebook nb = tacVisualizingDock.Content as Gtk.Notebook;
      Gtk.TextView textView = nb.CurrentPageWidget as Gtk.TextView;
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