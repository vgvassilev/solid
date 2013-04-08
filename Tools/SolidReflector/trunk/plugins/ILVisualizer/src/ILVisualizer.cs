using System;
using System.Collections.Generic;

using SolidOpt.Services;

using SolidReflector.Plugins;
using SolidReflector.Plugins.AssemblyBrowser;

using Mono.Cecil;
using MonoDevelop.Components.Docking;
using Gtk;

namespace SolidReflector.Plugins.ILVisualizer
{
  public class ILVisualizer : IPlugin
  {

    private DockItem ilVisualizingDock = null;
    public ILVisualizer() { }

    #region IPlugin implementation
    void IPlugin.Init(ISolidReflector reflector)
    {
      var MainWindow = reflector.GetMainWindow();
      IAssemblyBrowser browser = reflector.GetPlugins().GetService<IAssemblyBrowser>();
      browser.SelectionChanged += OnSelectionChanged;

      ilVisualizingDock = MainWindow.DockFrame.AddItem("ILVisualizer");
      ilVisualizingDock.Expand = true;
      ilVisualizingDock.DrawFrame = true;
      ilVisualizingDock.Label = "IL Visualizer";
      Gtk.Notebook nb = new Gtk.Notebook();
      nb.AppendPage( new TextView(), new Gtk.Label("IL Text"));
      nb.ShowAll();
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

