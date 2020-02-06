/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using Gtk;
using System;

using SolidIDE;
using SolidOpt.Services;

using SolidV.Ide.Dock;
using SolidV.MVC;

namespace SolidIDE.Plugins.Designer
{
  public class DesignerPlugin : IPlugin, IDesigner
  {
    // External global objects (form Main program and other plugins)
    private ISolidIDE solidIDE;
    private MainWindow mainWindow;

    // Plugin global objects
    private DockItem designerDockItem;
    private Gtk.Notebook noteBook;

    private Sheet<Gdk.Event, Cairo.Context, SolidV.MVC.Model> currentSheet;
    private DrawingArea Canvas;

    private CompositeController<Gdk.Event, Cairo.Context, Model> controller;
    private ChainController<Gdk.Event, Cairo.Context, Model> chainController;

    #region IPlugin implementation

    void IPlugin.Init(object context) {
      solidIDE = context as ISolidIDE;
      mainWindow = solidIDE.GetMainWindow();

      // Notebook
      noteBook = new Gtk.Notebook();

      designerDockItem = mainWindow.DockFrame.AddItem("Designer");
      designerDockItem.Behavior =
        DockItemBehavior.CantAutoHide |
        DockItemBehavior.CantClose |
        DockItemBehavior.NeverFloating;
      designerDockItem.Expand = true;
      designerDockItem.DrawFrame = true;
      designerDockItem.Label = "Designer";
      designerDockItem.Content = noteBook;
      designerDockItem.DefaultVisible = true;
      designerDockItem.Visible = true;

      // Menu
      var fileMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("File");
      var saveMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("File", "Save");
      saveMenuItem.Activated += HandleSaveActivated;
      var viewMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("View");
      var designerMenuItem = solidIDE.GetMenuItem<Gtk.ImageMenuItem>("View", "Designer");
      designerMenuItem.Activated += HandleShowDesignerActivated;

      // Test
      var model = new Model();
      ShapesModel scene = new ShapesModel();
      AnnotationModel annotation = new AnnotationModel();
      SelectionModel selection = new SelectionModel();
      InteractionStateModel interaction = new InteractionStateModel();
      var view = new View<Cairo.Context, Model>();
      controller = new CompositeController<Gdk.Event, Cairo.Context, Model>();

      model.RegisterSubModel<ShapesModel>(scene);
      model.RegisterSubModel<SelectionModel>(selection);
      model.RegisterSubModel<AnnotationModel> (annotation);
      model.RegisterSubModel<InteractionStateModel>(interaction);

      model.ModelChanged += HandleModelChanged;

      //TODO: Scan plugins for shapes and viewers
      view.Viewers.Add(typeof(Model), new ModelViewer<Cairo.Context>());
      view.Viewers.Add(typeof(ShapesModel), new ShapeModelViewer());
      view.Viewers.Add(typeof(AnnotationModel), new ShapeModelViewer ());
      view.Viewers.Add(typeof(RectangleShape), new RectangleShapeViewer());
      view.Viewers.Add(typeof(EllipseShape), new EllipseShapeViewer());
      view.Viewers.Add(typeof(DiamondShape), new DiamondShapeViewer());
      view.Viewers.Add(typeof(ArrowShape), new ArrowShapeViewer());
      view.Viewers.Add(typeof(BezierCurvedArrowShape), new BezierCurvedArrowShapeViewer());
      view.Viewers.Add(typeof(TextBlockShape), new TextBlockShapeViewer());
      view.Viewers.Add(typeof(SelectionModel), new SelectionModelViewer());
      view.Viewers.Add(typeof(Glue), new GlueViewer());
      view.Viewers.Add(typeof(InteractionStateModel), new InteractionStateModelViewer());
      view.Viewers.Add(typeof(FocusRectangleShape), new FocusRectangleShapeViewer());
      view.Viewers.Add(typeof(HeatMapShape), new HeatMapShapeViewer());

      controller.SubControllers.Add(new ShapeSelectionController(model, view));
      chainController = new ChainController<Gdk.Event, Cairo.Context, Model>();
      //chainController.SubControllers.Add(new ConnectorDragController(model, view));
      chainController.SubControllers.Add(new ShapeDragController(model, view));
      chainController.SubControllers.Add(new AnnotationController(model, view));
      controller.SubControllers.Add(chainController);

      IDesigner designer = this;
      designer.AddSheet("Sheet", new Sheet<Gdk.Event, Cairo.Context, Model>(model, view, controller));

      Shape hm = new HeatMapShape(new Cairo.Rectangle(1, 1, 1000, 1000));
      annotation.Shapes.Add(hm);
    }

    void IPlugin.UnInit(object context) {
      designerDockItem.Visible = false;
      // BUG: Object not set to an instance of an object exception if only one plugin is loaded
      // and attempted to be UnInit-ed
      mainWindow.DockFrame.RemoveItem(designerDockItem);
    }

    #endregion

    #region IDesigner

    void IDesigner.AddShapes(params Shape[] shapes) {
      currentSheet.Model.BeginUpdate();
      currentSheet.Model.GetSubModel<ShapesModel>().Shapes.AddRange(shapes);
      currentSheet.Model.EndUpdate();
    }

    public ISheet CurrentSheet {
      get { return currentSheet; }
      set {
        if (currentSheet != value) {
          currentSheet = value as Sheet<Gdk.Event, Cairo.Context, SolidV.MVC.Model>;
          //
        }
      }
    }

    ISheet IDesigner.AddSheet(string Label, ISheet sheet) {
      if (this.currentSheet == null) {
        Canvas = new DrawingArea();
        Canvas.AddEvents((int)Gdk.EventMask.ButtonPressMask);
        Canvas.AddEvents((int)Gdk.EventMask.ButtonReleaseMask);
        Canvas.AddEvents((int)Gdk.EventMask.PointerMotionMask);
        //
        Canvas.ButtonPressEvent += HandleButtonPressEvent;
        Canvas.ButtonReleaseEvent += HandleButtonReleaseEvent;
        Canvas.ExposeEvent += HandleExposeEvent;
        Canvas.MotionNotifyEvent += HandleMotionNotifyEvent;
        Gtk.Drag.DestSet(
          Canvas,
          DestDefaults.All, new Gtk.TargetEntry[] {
            new Gtk.TargetEntry("application/x-solidide.shape", Gtk.TargetFlags.App, 0)
          },
          Gdk.DragAction.Copy
        );
        Canvas.DragDataReceived += HandleCanvasDragDataReceived;
        //
        noteBook.AppendPage(Canvas, new Gtk.Label(Label));
        noteBook.ShowAll(); //Canvas.Show();
      }
      CurrentSheet = sheet as Sheet<Gdk.Event, Cairo.Context, SolidV.MVC.Model>;
      return sheet;
    }

    public void AttachToolController(IController<Gdk.Event, Cairo.Context, Model> toolController) {
      chainController.SubControllers.Add(toolController);
      //chainController.SubControllers.Insert(0, toolController);
    }

    public void DetachToolController(IController<Gdk.Event, Cairo.Context, Model> toolController) {
      chainController.SubControllers.Remove(toolController);
    }

    Gtk.Notebook IDesigner.GetNotebook() {
      return noteBook;
    }

    public event CurrentSheetChangedDelegate CurrentSheetChanged;

    #endregion

    private void HandleModelChanged(object model)
    {
      Canvas.QueueDraw();
    }

    private void HandleButtonPressEvent(object o, Gtk.ButtonPressEventArgs args) {
      Canvas.GrabFocus();
      currentSheet.Controller.HandleEvent(args.Event);
    }

    private void HandleMotionNotifyEvent(object o, Gtk.MotionNotifyEventArgs args)
    {
      currentSheet.Controller.HandleEvent(args.Event);
    }

    private void HandleButtonReleaseEvent(object o, Gtk.ButtonReleaseEventArgs args)
    {
      currentSheet.Controller.HandleEvent(args.Event);
    }

    private void HandleExposeEvent(object o, Gtk.ExposeEventArgs args) {
      using (Cairo.Context context = Gdk.CairoHelper.Create((o as DrawingArea).GdkWindow)) {
        context.Antialias = Cairo.Antialias.Subpixel;
        currentSheet.View.Draw(context, currentSheet.Model);
      }
    }

    private void HandleSaveActivated(object sender, EventArgs e) {
      //using (StreamWriter file = new System.IO.StreamWriter(currentPath)) {
        //file.Write(textView.Buffer.Text);
      //}
    }

    private void HandleShowDesignerActivated(object sender, EventArgs e) {
      designerDockItem.Visible = true;
    }

    private void HandleCanvasDragDataReceived(object o, DragDataReceivedArgs args)
    {
      if (args.SelectionData.Target.Name == "application/x-solidide.shape") {
        Shape shape = (Shape)args.SelectionData.Data.FromArray();
        if (shape != null) {
          shape.Matrix.Translate(args.X, args.Y);
          (this as IDesigner).AddShapes(shape);
          Gtk.Drag.Finish(args.Context, true, false, args.Time);
        }
      }

      Gtk.Drag.Finish(args.Context, false, false, args.Time);
    }

  }
}
