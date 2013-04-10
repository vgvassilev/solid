using MonoDevelop.Components.Docking;
using Gtk;
using System;

using SolidReflector.Plugins.AssemblyBrowser;

namespace SolidReflector.Plugins.CGVisualizer
{
  public class CGVisualizer : IPlugin
  {
    private DockItem cgVisualizingDock = null;

    public CGVisualizer() { }

    #region IPlugin implementation
    void IPlugin.Init(ISolidReflector reflector)
    {
      var MainWindow = reflector.GetMainWindow();
      IAssemblyBrowser browser = reflector.GetPlugins().GetService<IAssemblyBrowser>();
      browser.SelectionChanged += OnSelectionChanged;

      Gtk.Notebook nb = new Gtk.Notebook();
      nb.AppendPage(new TextView(), new Gtk.Label("CG Text"));
      nb.ShowAll();

      cgVisualizingDock = MainWindow.DockFrame.AddItem("CGVisualizer");
      cgVisualizingDock.DrawFrame = true;
      cgVisualizingDock.Label = "Call Graph Visualizer";
      cgVisualizingDock.Content = nb;
      cgVisualizingDock.DefaultVisible = true;
      cgVisualizingDock.Visible = true;
    }
    #endregion

    private void OnSelectionChanged(object sender, SelectionEventArgs args) {
      Gtk.Notebook nb = cgVisualizingDock.Content as Gtk.Notebook;
      Gtk.TextView textView = nb.CurrentPageWidget as Gtk.TextView;
      if (args.definition != null) {
        // Dump the definition
        CGPrettyPrinter.PrintPretty(args.definition, textView);
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