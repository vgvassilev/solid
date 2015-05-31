/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

using SolidIDE;
using SolidOpt.Services;

using SolidV.Ide.Dock;
using SolidV.MVC;
using Cairo;

using SolidIDE.Plugins.Designer;
using SolidIDE.Domains;

namespace SolidIDE.Plugins.Toolbox
{
  public class Toolbox : IPlugin, IPad, IToolbox
  {
    // External global objects (form Main program and other plugins)
    private ISolidIDE solidIDE;
    private MainWindow mainWindow;
    public static IServiceContainer plugins;
    public static IDesigner designer;

    // Plugin global objects
    private DockItem toolboxDockItem;
    private Gtk.TreeView treeView;

    private IController<Gdk.Event, Cairo.Context, Model> currentToolController;

    private Dictionary<string, Gtk.TreeIter> domainDictionary = new Dictionary<string, Gtk.TreeIter>();

    #region IPlugin implementation

    void IPlugin.Init(object context) {
      solidIDE = context as ISolidIDE;
      mainWindow = solidIDE.GetMainWindow();

      plugins = solidIDE.GetServiceContainer();
      designer = plugins.GetService<IDesigner>();

      // Dock with Tree
      Gtk.ScrolledWindow treeViewScrollWindow = new Gtk.ScrolledWindow();
      Gtk.Viewport treeViewViewport = new Gtk.Viewport();
      treeViewScrollWindow.Add(treeViewViewport);
      treeView = new Gtk.TreeView();
      treeViewViewport.Add(treeView);
      treeViewScrollWindow.ShowAll();
      var r = new Gtk.CellRendererPixbuf();
      r.Xalign = 0.5f;
      Gtk.TreeViewColumn col = treeView.AppendColumn("*", r, "pixbuf", 3);
      col.MinWidth = 48;
      col.Alignment = 0.5f;
      treeView.AppendColumn("Primitives", new Gtk.CellRendererText(), "text", 0);
      treeView.Model = new Gtk.TreeStore(
        typeof(string), 
        typeof(Tool<Gdk.Event, Cairo.Context, Model>), 
        typeof(Shape), 
        typeof(Gdk.Pixbuf));
      treeView.RowActivated += HandleRowActivated;
      treeView.CursorChanged += HandleCursorChanged;
      Gtk.Drag.SourceSet(
        treeView,
        Gdk.ModifierType.Button1Mask,
        new Gtk.TargetEntry[] { 
          new Gtk.TargetEntry("application/x-solidide.shape", Gtk.TargetFlags.App, 0)
        },
        Gdk.DragAction.Copy
      );
      treeView.DragBegin += HandleTreeViewDragBegin;
      treeView.DragDataGet += HandleTreeViewDragDataGet;

      toolboxDockItem = mainWindow.DockFrame.AddItem("Toolbox");
      toolboxDockItem.Behavior = DockItemBehavior.Normal;
      toolboxDockItem.Icon = global::Gdk.Pixbuf.LoadFromResource("package.png");
      toolboxDockItem.Expand = true;
      toolboxDockItem.DrawFrame = false;
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
      var updateToolboxMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("View", "Update Toolbox");
      updateToolboxMenuItem.Activated += HandleUpdateToolboxMenuItemActivated;
    }

    void RunClicked(object sender, EventArgs e)
    {
      
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

    public Shape ShapeFromTool(ITool<Gdk.Event, Cairo.Context, Model> tool, Rectangle dropTo,
      Shape currentInfoObject = null)
    {
      if (tool == null || currentInfoObject == null)
        return null;
      
      var command = tool.Command() as AddNewShapeCommand;
      command.NewShape = currentInfoObject.DeepCopy();
      if (dropTo.Width != 0 || dropTo.Height != 0) {
        command.NewShape.Rectangle = new Rectangle(0, 0, dropTo.Width, dropTo.Height);
        command.NewShape.Matrix.Translate(dropTo.X, dropTo.Y);
      } else {
        command.NewShape.Rectangle = new Rectangle(
          0, 0, currentInfoObject.Width, currentInfoObject.Height
        );
        command.NewShape.Matrix.Translate(dropTo.X, dropTo.Y);
      }
      return command.NewShape;
    }

    public void DropShapeFromTool(ITool<Gdk.Event, Cairo.Context, Model> tool, Rectangle dropTo,
      Shape currentInfoObject = null)
    {
      //TODO: May we simulate drag&drop mechanism to send data?

      //IServiceContainer plugins = solidIDE.GetServiceContainer();
      //IDesigner designer = plugins.GetService<IDesigner>();

      if (designer != null && designer.CurrentSheet != null) {
        if (tool != null) {
          ShapeFromTool(tool, dropTo, currentInfoObject);
          tool.Command().Do();
        } else if (currentInfoObject != null) {
          designer.AddShapes(currentInfoObject.DeepCopy());
        }
      }
    }

    public Gdk.Pixbuf MakePixbufFromShape(Shape shape) {
      int w = (int)shape.Width;
      int h = (int)shape.Height;
      Cairo.ImageSurface surface = new Cairo.ImageSurface(Cairo.Format.RGB24, w+4, h+4);
      Gdk.Pixmap pm = new Gdk.Pixmap(null, w+4, h+4, 24);
      using (Cairo.Context c = Gdk.CairoHelper.Create(pm)) {
        //c.Save();
        //c.Operator = Operator.Clear;
        //c.Paint();
        //c.Restore();
        c.SetSourceRGB(1, 1, 1);
        //c.Operator = Operator.Source;
        c.Paint();
        //IServiceContainer plugins = solidIDE.GetServiceContainer();
        //IDesigner designer = plugins.GetService<IDesigner>();
        var view = (designer.CurrentSheet as Sheet<Gdk.Event, Cairo.Context, Model>).View;
        shape.Matrix.Translate(2, 2);
        view.DrawItem(c, shape);
      }
      return Gdk.Pixbuf.FromDrawable(pm, Gdk.Colormap.System, 0, 0, 0, 0, w+4, h+4);
    }

    #endregion

    public void AddShapeToToolbox(Gtk.TreeStore store, Shape shape) {
      //IServiceContainer plugins = solidIDE.GetServiceContainer();
      //IDesigner designer = plugins.GetService<IDesigner>();

      var model = (designer.CurrentSheet as Sheet<Gdk.Event, Cairo.Context, Model>).Model;
      var view = (designer.CurrentSheet as Sheet<Gdk.Event, Cairo.Context, Model>).View;

      System.Attribute[] attrs = System.Attribute.GetCustomAttributes(shape.GetType());

      ITool<Gdk.Event, Cairo.Context, Model> tool = null;
      string name = null;

      foreach (System.Attribute attr in attrs) {
        if (attr is ToolAttribute) {
          tool = ((ToolAttribute.ToolProvider<Gdk.Event, Cairo.Context, Model>)
            Activator.CreateInstance((attr as ToolAttribute).ToolProviderType)).GetTool();
        } else if (attr is ShapeNameAttribute) {
          name = (attr as ShapeNameAttribute).ShapeName;
        }
      }
      if (tool == null) { // Default shape creator tool
        tool = new Tool<Gdk.Event, Cairo.Context, Model>(
          new FocusRectangleController(model, view),
          new AddNewShapeCommand()
        );
        (tool.Controller() as FocusRectangleController).FocusedRectangleFinish += 
          HandleFocusedRectangleFinish;
      }
      if (name == null) {
        name = shape.GetType().Name;
      }

      // Register additional viewers
      foreach (System.Attribute attr in attrs) {
        if (attr is ViewerAttribute) {
          if (!((view as View<Cairo.Context, Model>).Viewers.ContainsKey(shape.GetType()))) {
            (view as View<Cairo.Context, Model>).Viewers.Add(
              shape.GetType(),
              (IViewer<Cairo.Context, Model>)Activator.CreateInstance((attr as ViewerAttribute).ViewerType)
            );
          }
        }
      }

      // Generate shape preview
      Shape preview = shape.DeepCopy(); //TODO: Get shape preview form domain if exists
      preview.Rectangle = new Rectangle(0, 0, 32, 16);
      Gdk.Pixbuf previewImage = MakePixbufFromShape(preview);
      Shape template = shape.DeepCopy(); //TODO: Get template shape from domain is exists
      template.Rectangle = new Rectangle(0, 0, 70, 40); 

      // Add shapes to treeview
      bool isUniverse = true;
      foreach (System.Attribute attr in attrs) {
        if (attr is DomainAttribute) {
          string shapeDomain = ((DomainAttribute)attr).DomainName;
          Gtk.TreeIter domainNode;
          if (!domainDictionary.TryGetValue(shapeDomain, out domainNode)) {
            domainNode = store.AppendValues(shapeDomain + " Domain");
            domainDictionary.Add(shapeDomain, domainNode);
          }
          store.AppendValues(domainNode, name, tool, template, previewImage);
          isUniverse = false;
        }
      }
      if (isUniverse) {
        Gtk.TreeIter domainNode;
        string shapeDomain = "Universe";
        if (!domainDictionary.TryGetValue(shapeDomain, out domainNode)) {
          domainNode = store.AppendValues(shapeDomain + " Domain");
          domainDictionary.Add(shapeDomain, domainNode);
        }
        store.AppendValues(domainNode, name, tool, template, previewImage);
      }
    }

    protected void UpdateToolbox() {
      Gtk.TreeStore store = treeView.Model as Gtk.TreeStore;
      store.Clear();
      domainDictionary.Clear();

      // Scan all shapes from plugins and add items to toolbox
      var shapes = solidIDE.GetServiceContainer().GetServices<Shape>();
      foreach (Shape shape in shapes) {
        AddShapeToToolbox(store, shape);
      }

      //TODO: Remove
      // begin-remove
      AddShapeToToolbox(store, new RectangleShape(new Rectangle(0,0,100,50)));
      AddShapeToToolbox(store, new EllipseShape(new Rectangle(0,0,100,50)));
      AddShapeToToolbox(store,
        new ArrowShape(
          new Glue(new Rectangle(0,8,0,0)), 
          new Glue(new Rectangle(30,8,0,0))
        )
      );
      // end-remove

      treeView.ExpandAll();
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
      var currentTool = (Tool<Gdk.Event, Cairo.Context, Model>)treeView.Model.GetValue(iter, 1);
      //Shape currentInfoObject = (Shape)treeView.Model.GetValue(iter, 2);

      //IServiceContainer plugins = solidIDE.GetServiceContainer();
      //IDesigner designer = plugins.GetService<IDesigner>();

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

    protected void HandleUpdateToolboxMenuItemActivated(object sender, EventArgs e)
    {
      UpdateToolbox();
    }

  }
}
