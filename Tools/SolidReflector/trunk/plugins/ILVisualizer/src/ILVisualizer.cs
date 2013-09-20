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
    private Gtk.Notebook nb = new Gtk.Notebook();
    private TextView textView = new TextView();

    public ILVisualizer() { }

    #region IPlugin implementation
    void IPlugin.Init(object context)
    {
      ISolidReflector reflector = context as ISolidReflector;
      mainWindow = reflector.GetMainWindow();
      IAssemblyBrowser browser = reflector.GetPlugins().GetService<IAssemblyBrowser>();
      browser.SelectionChanged += OnSelectionChanged;

      nb.AppendPage(textView, new Gtk.Label("IL Text"));

      ScrolledWindow scrollWindow = new ScrolledWindow();
      Viewport viewport = new Viewport();
      scrollWindow.Add(viewport);
      viewport.Add(nb);
      scrollWindow.ShowAll();

      ilVisualizingDock = mainWindow.DockFrame.AddItem("ILVisualizer");
      ilVisualizingDock.Expand = true;
      ilVisualizingDock.DrawFrame = true;
      ilVisualizingDock.Label = "IL Visualizer";
      ilVisualizingDock.Content = scrollWindow;
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
      if (args.assembly != null) {
        ILPrettyPrinter.PrintAssembly(args.assembly, textView);
        if (args.module != null) {
          ILPrettyPrinter.PrintModule(args.module, textView);
          if (args.definition != null) {
            // Dump the definition
            ILPrettyPrinter.PrintPretty(args.definition, textView);
          }
        }
      }
    }
  }
}