using MonoDevelop.Components.Docking;
using Gtk;
using System;

using SolidReflector.Plugins.AssemblyBrowser;
using SolidOpt.Services;
using SolidOpt.Services.Transformations.CodeModel.CallGraph;
using SolidOpt.Services.Transformations.Multimodel.ILtoCG;
using Mono.Cecil;

namespace SolidReflector.Plugins.CGVisualizer
{
  public class CGVisualizer : IPlugin
  {
    private MainWindow mainWindow = null;
    private DockItem cgVisualizingDock = null;
    private Gtk.Notebook nb = new Gtk.Notebook();
    private Gtk.TextView textView = new TextView();
    private Gtk.DrawingArea drawingArea = new Gtk.DrawingArea();

    public CGVisualizer() { }

    #region IPlugin implementation
    void IPlugin.Init(object context)
    {
      ISolidReflector reflector = context as ISolidReflector;
      mainWindow = reflector.GetMainWindow();
      IAssemblyBrowser browser = reflector.GetPlugins().GetService<IAssemblyBrowser>();
      browser.SelectionChanged += OnSelectionChanged;

      ScrolledWindow scrollWindow = new ScrolledWindow();
      Viewport viewport = new Viewport();
      scrollWindow.Add(viewport);
      viewport.Add(nb);
      scrollWindow.ShowAll();

      nb.AppendPage(textView, new Gtk.Label("CG Text"));
      nb.AppendPage(drawingArea, new Gtk.Label("CG Visualizer"));
      nb.ShowAll();

      cgVisualizingDock = mainWindow.DockFrame.AddItem("CGVisualizer");
      cgVisualizingDock.DrawFrame = true;
      cgVisualizingDock.Label = "Call Graph Visualizer";
      cgVisualizingDock.Content = scrollWindow;
      cgVisualizingDock.DefaultVisible = true;
      cgVisualizingDock.Visible = true;
    }

    void IPlugin.UnInit(object context)
    {
      cgVisualizingDock.Visible = false;
      // BUG: Object not set to an instance of an object exception if only one plugin is loaded
      // and attempted to be UnInit-ed
      mainWindow.DockFrame.RemoveItem(cgVisualizingDock);
    }
    #endregion

    private void OnSelectionChanged(object sender, SelectionEventArgs args) {
      if (args.definition is MethodDefinition) {
        CGPrettyPrinter.PrintPretty(args.definition, textView);
        CGPrettyDrawer drawer = new CGPrettyDrawer(drawingArea);

        var builder = new CallGraphBuilder(args.definition as MethodDefinition);
        CallGraph currentCg = builder.Create(2);

        drawer.DrawCallGraph(currentCg);
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