using System;
using System.Collections;
using System.Collections.Generic;

using SolidReflector;
using SolidReflector.Plugins;
using SolidReflector.Plugins.AssemblyBrowser;

using SolidOpt.Services.Transformations;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

using Gtk;
using MonoDevelop.Components.Docking;

using SolidV.MVC;

namespace SolidReflector.Plugins.CFGVisualizer
{
  public class CFGVisualizer : IPlugin
  {
    private DockItem cfgVisualizingDock = null;

    public CFGVisualizer ()
    {
    }
    #region IPlugin implementation
    void IPlugin.Init (ISolidReflector reflector)
    {
      var MainWindow = reflector.GetMainWindow();
      IAssemblyBrowser browser = reflector.GetPlugins().GetService<IAssemblyBrowser>();
      browser.SelectionChanged += OnSelectionChanged;

      cfgVisualizingDock = MainWindow.DockFrame.AddItem("CFG Visualizer");
      //ilVisualizingDock.Behavior = DockItemBehavior.Locked;
      cfgVisualizingDock.Expand = true;
      cfgVisualizingDock.DrawFrame = true;
      cfgVisualizingDock.Label = "CFG Visualizer";
      Gtk.Notebook nb = new Gtk.Notebook();
      nb.AppendPage(new TextView(), new Gtk.Label("CFG Text"));
      nb.AppendPage(new DrawingArea(), new Gtk.Label("CFG Visualizer"));
      nb.ShowAll();
      cfgVisualizingDock.Content = nb;
      cfgVisualizingDock.DefaultVisible = true;
      cfgVisualizingDock.Visible = true;
    }
    #endregion
    private void OnSelectionChanged(object sender, SelectionEventArgs args) {
      Gtk.Notebook nb = cfgVisualizingDock.Content as Gtk.Notebook;
      Gtk.TextView textView = nb.GetNthPage(0) as Gtk.TextView;
      Gtk.DrawingArea drawingArea = nb.GetNthPage(1) as Gtk.DrawingArea;
      if (args.definition != null) {
        // Dump the definition
        CFGPrettyPrinter.PrintPretty(args.definition, textView);
        CFGPrettyDrawer drawer = new CFGPrettyDrawer(drawingArea);
        drawer.DrawTextBlocks(args.definition);
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

