/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using SolidIDE;
using SolidOpt.Services;

using SolidV.Ide.Dock;
using SolidV.MVC;
using Cairo;

using SolidIDE.Plugins.Designer;

namespace SolidIDE.Plugins.Toolbox
{
  public class Toolbox : IPlugin, IPad, IToolbox
  {
    // External global objects (form Main program and other plugins)
    private ISolidIDE solidIDE;
    private MainWindow mainWindow;

    // Plugin global objects
    private DockItem toolboxDockItem;
    private Gtk.TreeView treeView;

    private IController<Gdk.Event, Cairo.Context, Model> currentToolController;

    #region IPlugin implementation

    void IPlugin.Init(object context) {
      solidIDE = context as ISolidIDE;
      mainWindow = solidIDE.GetMainWindow();

      // Dock with Tree
      Gtk.ScrolledWindow treeViewScrollWindow = new Gtk.ScrolledWindow();
      Gtk.Viewport treeViewViewport = new Gtk.Viewport();
      treeViewScrollWindow.Add(treeViewViewport);
      treeView = new Gtk.TreeView();
      treeViewViewport.Add(treeView);
      treeViewScrollWindow.ShowAll();
      Gtk.TreeViewColumn col = new Gtk.TreeViewColumn();
      Gtk.CellRendererText colAssemblyCell = new Gtk.CellRendererText();
      col.PackStart(colAssemblyCell, true);
      col.AddAttribute(colAssemblyCell, "text", 0);
      treeView.AppendColumn(col);
      treeView.Model = new Gtk.TreeStore(typeof(string), typeof(Tool<Gdk.Event, Cairo.Context, Model>), typeof(Shape));
      treeView.RowActivated += HandleRowActivated;
      treeView.CursorChanged += HandleCursorChanged;
      Gtk.Drag.SourceSet(
        treeView,
        Gdk.ModifierType.Button1Mask,
        new Gtk.TargetEntry[] { new Gtk.TargetEntry("application/x-solidide.shape", Gtk.TargetFlags.App, 0) },
        Gdk.DragAction.Copy
      );
      /*treeView.EnableModelDragSource(
        Gdk.ModifierType.None,
        new Gtk.TargetEntry[] { new TargetEntry("application/SolidIDE", 0, 0) },
        Gdk.DragAction.Default);*/
      treeView.DragBegin += HandleTreeViewDragBegin;
      treeView.DragDataGet += HandleTreeViewDragDataGet;
      //treeView.DragDrop += HandleTreeViewDragDrop;

      toolboxDockItem = mainWindow.DockFrame.AddItem("Toolbox");
      toolboxDockItem.Behavior = DockItemBehavior.Normal;
      toolboxDockItem.Expand = true;
      toolboxDockItem.DrawFrame = true;
      toolboxDockItem.Label = "Toolbox";
      toolboxDockItem.Content = treeViewScrollWindow;
      toolboxDockItem.DefaultVisible = true;
      toolboxDockItem.Visible = true;

      UpdateToolbox();

      // Menu
      var viewMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("View");
      solidIDE.GetMenuItem<Gtk.TearoffMenuItem>("View", "TearoffView");
      var toolboxMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("View", "Toolbox");
      toolboxMenuItem.Activated += HandleShowToolboxActivated;
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

    #region IToolbox implementation

    public Shape ShapeFromTool(ITool<Gdk.Event, Cairo.Context, Model> tool, Rectangle dropTo, Shape currentInfoObject = null) {
      var command = tool.Command() as AddNewShapeCommand;
      if (currentInfoObject != null) {
        command.NewShape = currentInfoObject.DeepCopy();
        if (dropTo.Width != 0 || dropTo.Height != 0) {
          command.NewShape.Rectangle = new Rectangle(0, 0, dropTo.Width, dropTo.Height);
          command.NewShape.Matrix.Translate(dropTo.X, dropTo.Y);
        } else {
          command.NewShape.Rectangle = new Rectangle(0, 0, currentInfoObject.Width, currentInfoObject.Height);
          command.NewShape.Matrix.Translate(dropTo.X, dropTo.Y);
        }
      } else {
        return null;
      }
      return command.NewShape;
    }

    public void DropShapeFromTool(ITool<Gdk.Event, Cairo.Context, Model> tool, Rectangle dropTo, Shape currentInfoObject = null) {
      //TODO: May we simulate drag&drop mechanism to send data?

      IServiceContainer plugins = solidIDE.GetServiceContainer();
      IDesigner designer = plugins.GetService<IDesigner>();

      if (designer != null && designer.CurrentSheet != null) {
        if (tool != null) {
          ShapeFromTool(tool, dropTo, currentInfoObject);
          tool.Command().Do();
        } else if (currentInfoObject != null) {
          designer.AddShapes(currentInfoObject.DeepCopy());
        }
      }
    }
    #endregion

    protected void UpdateToolbox() {
      Gtk.TreeStore store = treeView.Model as Gtk.TreeStore;
      //if (store == null) store = new Gtk.TreeStore(typeof(string), typeof(Tool<Gdk.Event, Cairo.Context, Model>), typeof(Shape));

      IServiceContainer plugins = solidIDE.GetServiceContainer();
      IDesigner designer = plugins.GetService<IDesigner>();

      //TODO: Scan for Shape types
      var model = (designer.CurrentSheet as Sheet<Gdk.Event, Cairo.Context, Model>).Model;
      var view = (designer.CurrentSheet as Sheet<Gdk.Event, Cairo.Context, Model>).View;
      ITool<Gdk.Event, Cairo.Context, Model> tool;
      //
      tool = new Tool<Gdk.Event, Cairo.Context, Model>(new FocusRectangleController(model,view), new AddNewShapeCommand(designer, null));
      (tool.Controller() as FocusRectangleController).FocusedRectangleFinish += HandleFocusedRectangleFinish;
      store.AppendValues("Rectangle", tool, new RectangleShape(new Rectangle(0,0,100,50)));
      //
      tool = new Tool<Gdk.Event, Cairo.Context, Model>(new FocusRectangleController(model,view), new AddNewShapeCommand(designer, null));
      (tool.Controller() as FocusRectangleController).FocusedRectangleFinish += HandleFocusedRectangleFinish;
      store.AppendValues("Ellipse", tool, new EllipseShape(new Rectangle(0,0,50,100)));
      //
      Shape Nowhere = new Glue(new Rectangle(0,0,0,0));
      tool = new Tool<Gdk.Event, Cairo.Context, Model>(null, new AddNewBinaryRelationCommand(designer, new ArrowShape(Nowhere, Nowhere)));
      store.AppendValues("Arrow Connector", tool, new ArrowShape(Nowhere, Nowhere));
    }

    protected void HandleShowToolboxActivated(object sender, EventArgs e) {
      toolboxDockItem.Visible = true;
    }

    void HandleRowActivated(object o, Gtk.RowActivatedArgs args) {
      Gtk.TreeIter iter;
      treeView.Model.GetIter(out iter, args.Path);
      //string currentName = (string)treeView.Model.GetValue(iter, 0);
      DropShapeFromTool(
        (Tool<Gdk.Event, Cairo.Context, Model>)treeView.Model.GetValue(iter, 1),
        new Rectangle(),
        (Shape)treeView.Model.GetValue(iter, 2)
      );
    }

    protected void HandleFocusedRectangleFinish(FocusRectangleController controller) {
      if (controller.FocusedRectangle.Width != 0 || controller.FocusedRectangle.Height != 0) {
        Gtk.TreeIter iter;
        treeView.Selection.GetSelected(out iter);
        //string currentName = (string)treeView.Model.GetValue(iter, 0);
        DropShapeFromTool(
          (Tool<Gdk.Event, Cairo.Context, Model>)treeView.Model.GetValue(iter, 1),
          controller.FocusedRectangle,
          (Shape)treeView.Model.GetValue(iter, 2)
        );
      }
    }

    protected void HandleCursorChanged(object o, EventArgs args) {
      Gtk.TreeIter iter;
      treeView.Selection.GetSelected(out iter);
      //string currentName = (string)treeView.Model.GetValue(iter, 0);
      Tool<Gdk.Event, Cairo.Context, Model> currentTool = (Tool<Gdk.Event, Cairo.Context, Model>)treeView.Model.GetValue(iter, 1);
      //Shape currentInfoObject = (Shape)treeView.Model.GetValue(iter, 2);

      IServiceContainer plugins = solidIDE.GetServiceContainer();
      IDesigner designer = plugins.GetService<IDesigner>();

      if (designer != null && designer.CurrentSheet != null) {
        if (currentToolController != null) {
          designer.DetachToolController(currentToolController);
          currentToolController = null;
        }
        if (currentTool != null) {
          currentToolController = currentTool.Controller();
          if (currentToolController != null) {
            designer.AttachToolController(currentToolController);
          }
        }
      }
    }

    protected void HandleTreeViewDragBegin(object o, Gtk.DragBeginArgs args) {
      Gdk.Pixmap pm = treeView.CreateRowDragIcon(treeView.Selection.GetSelectedRows()[0]);
      Gdk.Pixbuf pb = Gdk.Pixbuf.FromDrawable(pm, pm.Colormap, 0, 0, 0, 0, -1, -1);
      Gtk.Drag.SetIconPixbuf(args.Context, pb, 0, 0);
    }

    protected void HandleTreeViewDragDataGet(object o, Gtk.DragDataGetArgs args) {
      Gtk.TreeIter iter;
      treeView.Selection.GetSelected(out iter);
      //string currentName = (string)treeView.Model.GetValue(iter, 0);
      args.SelectionData.Set(
        Gdk.Atom.Intern("application/x-solidide.shape", false),
        8,
        ShapeFromTool(
          (Tool<Gdk.Event, Cairo.Context, Model>)treeView.Model.GetValue(iter, 1),
          new Rectangle(),
          (Shape)treeView.Model.GetValue(iter, 2)
        ).ToArray()
      );
    }

  }
}
