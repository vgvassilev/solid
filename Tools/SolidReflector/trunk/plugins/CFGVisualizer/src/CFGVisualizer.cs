using Gtk;
using MonoDevelop.Components.Docking;
using System;
using System.Collections;
using System.Collections.Generic;

using SolidOpt.Services;
using SolidReflector.Plugins.AssemblyBrowser;

namespace SolidReflector.Plugins.CFGVisualizer
{
  public class CFGVisualizer : IPlugin
  {
    private MainWindow mainWindow = null;
    private DrawingArea drawingArea = null;
    private DockItem cfgVisualizingDock = null;

    public CFGVisualizer() { }

    #region IPlugin implementation
    void IPlugin.Init(object context)
    {
      ISolidReflector reflector = context as ISolidReflector;
      mainWindow = reflector.GetMainWindow();
      IAssemblyBrowser browser = reflector.GetPlugins().GetService<IAssemblyBrowser>();
      browser.SelectionChanged += OnSelectionChanged;

      drawingArea = new DrawingArea();

      ScrolledWindow scrollWindow = new ScrolledWindow();
      Viewport viewport = new Viewport();

      scrollWindow.Add(viewport);
      viewport.Add(drawingArea);
      
      Gtk.Notebook nb = new Gtk.Notebook();
      nb.AppendPage(new TextView(), new Gtk.Label("CFG Text"));
      nb.AppendPage(scrollWindow, new Gtk.Label("CFG Visualizer"));
      nb.ShowAll();

      cfgVisualizingDock = mainWindow.DockFrame.AddItem("CFG Visualizer");
      cfgVisualizingDock.Expand = true;
      cfgVisualizingDock.DrawFrame = true;
      cfgVisualizingDock.Label = "CFG Visualizer";
      cfgVisualizingDock.Content = nb;
      cfgVisualizingDock.DefaultVisible = true;
      cfgVisualizingDock.Visible = true;
    }

    void IPlugin.UnInit(object context)
    {
      cfgVisualizingDock.Visible = false;
      // BUG: Object not set to an instance of an object exception if only one plugin is loaded
      // and attempted to be UnInit-ed
      mainWindow.DockFrame.RemoveItem(cfgVisualizingDock);
    }
    #endregion

    private void OnSelectionChanged(object sender, SelectionEventArgs args) {
      Gtk.Notebook nb = cfgVisualizingDock.Content as Gtk.Notebook;
      Gtk.TextView textView = nb.GetNthPage(0) as Gtk.TextView;
      //Gtk.DrawingArea drawingArea = nb.GetNthPage(1) as Gtk.DrawingArea;

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