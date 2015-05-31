/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;

using SolidIDE;
using SolidOpt.Services;

using SolidV.Ide.Dock;
using SolidV.MVC;

using SolidIDE.Plugins.Designer;

namespace SolidIDE.Plugins.Inspector
{
  public class Inspector : IPlugin, IPad, IInspector
  {
    // External global objects (form Main program and other plugins)
    private ISolidIDE solidIDE;
    private MainWindow mainWindow;

    // Plugin global objects
    private DockItem InspectorDockItem;
    private SolidV.Gtk.InspectorGrid.InspectorGrid inspectorGrid;

    private SelectionModel currentSelectionModel;

    #region IPlugin implementation

    void IPlugin.Init(object context) {
      solidIDE = context as ISolidIDE;
      mainWindow = solidIDE.GetMainWindow();

      // Dock with PropertyGrid
      Gtk.ScrolledWindow inspectorGridScrollWindow = new Gtk.ScrolledWindow();
      Gtk.Viewport inspectorGridViewport = new Gtk.Viewport();
      inspectorGridScrollWindow.Add(inspectorGridViewport);
      inspectorGrid = new SolidV.Gtk.InspectorGrid.InspectorGrid();
      inspectorGridViewport.Add(inspectorGrid);
      inspectorGridScrollWindow.ShowAll();

      InspectorDockItem = mainWindow.DockFrame.AddItem("Inspector");
      InspectorDockItem.Behavior = DockItemBehavior.Normal;
      InspectorDockItem.Expand = true;
      InspectorDockItem.DrawFrame = true;
      InspectorDockItem.Label = "Inspector";
      InspectorDockItem.Content = inspectorGridScrollWindow;
      InspectorDockItem.DefaultVisible = true;
      InspectorDockItem.Visible = true;

      IServiceContainer plugins = solidIDE.GetServiceContainer();
      IDesigner designer = plugins.GetService<IDesigner>();
      designer.CurrentSheetChanged += HandleDesignerCurrentSheetChanged;

      // Menu
      var viewMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("View");
      var InspectorMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("View", "Inspector");
      InspectorMenuItem.Activated += HandleShowInspectorActivated;
    }

    void IPlugin.UnInit(object context) {
      // throw new NotImplementedException();
    }

    #endregion

    #region IPad implementation

    void IPad.Init(DockFrame frame) {
      // throw new NotImplementedException();
    }

    #endregion

    #region IInspector implementation

    #endregion

    protected void HandleShowInspectorActivated(object sender, EventArgs e) {
      InspectorDockItem.Visible = true;
    }

    protected void HandleDesignerCurrentSheetChanged(ISheet sheet) {
      SelectionModel selection = (sheet as Sheet<Gdk.Event, Cairo.Context, SolidV.MVC.Model>).Model.GetSubModel<SelectionModel>();
      if (selection != currentSelectionModel) {
        currentSelectionModel.ModelChanged -= HandleSelectionModelChanged;
      }
      currentSelectionModel = selection;
      currentSelectionModel.ModelChanged += HandleSelectionModelChanged;  
    }

    protected void HandleSelectionModelChanged(object model) {
      SelectionModel selectionModel = model as SelectionModel;
      if (selectionModel.Selected.Count != 0) {
        var selected = selectionModel.Selected[0];
        // FIXME: Handle when more than one.
        inspectorGrid.CurrentObject = selected;
        //inspectorGrid.Populate();
      } else {
        inspectorGrid.CurrentObject = null;
      }
    }
  }
}
