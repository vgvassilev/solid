/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using Cairo;

using System;
using System.Collections.Generic;
using System.Text;

using SolidV;
using SolidV.MVC;

using DataMorphose.Model;

namespace DataMorphose.Plugins.Visualizer
{
  public class SchemaVisualizer
  {
    private Gtk.DrawingArea canvas = null;
    private SolidV.MVC.Model model = new SolidV.MVC.Model();
    private ShapesModel scene = new ShapesModel();
    private SelectionModel selection = new SelectionModel();
    private View<Context, SolidV.MVC.Model> view = new View<Context, SolidV.MVC.Model>();
    private CompositeController<Gdk.Event, Context, SolidV.MVC.Model> controller = 
                                    new CompositeController<Gdk.Event, Context, SolidV.MVC.Model>();
    
    public SchemaVisualizer(Gtk.DrawingArea canvas) {
      this.canvas = canvas;

      model.RegisterSubModel<ShapesModel>(scene);

      canvas.AddEvents((int) Gdk.EventMask.ButtonPressMask);
      canvas.AddEvents((int) Gdk.EventMask.ButtonReleaseMask);
      canvas.AddEvents((int) Gdk.EventMask.PointerMotionMask);

      canvas.ButtonPressEvent += HandleDrawingArea1ButtonPressEvent;
      canvas.ButtonReleaseEvent += HandleDrawingArea1ButtonReleaseEvent;
      canvas.ExposeEvent += HandleDrawingArea1ExposeEvent;
      canvas.ConfigureEvent += HandleConfigureEvent;
      canvas.MotionNotifyEvent += HandleDrawingArea1MotionNotifyEvent;

      model.RegisterSubModel<ShapesModel>(scene);
      model.RegisterSubModel<SelectionModel>(selection);
      model.ModelChanged += HandleModelChanged;

      view.Viewers.Add(typeof(SolidV.MVC.Model), new ModelViewer<Context>());
      view.Viewers.Add(typeof(ShapesModel), new ShapeModelViewer());
      view.Viewers.Add(typeof(RectangleShape), new RectangleShapeViewer());
      view.Viewers.Add(typeof(EllipseShape), new EllipseShapeViewer());
      view.Viewers.Add(typeof(ArrowShape), new ArrowShapeViewer());
      view.Viewers.Add(typeof(TextBlockShape), new TextBlockShapeViewer());
      view.Viewers.Add(typeof(SelectionModel), new SelectionModelViewer());

      controller.SubControllers.Add(new ShapeSelectionController(model, view));
      controller.SubControllers.Add(new ShapeDragController(model, view));
    }

    void HandleConfigureEvent (object o, Gtk.ConfigureEventArgs args) {
      controller.Handle(args.Event);
    }

    /// <summary>
    /// Invalidate the canvas.
    /// </summary>
    /// <param name='model'>
    /// Model.
    /// </param>
    public void HandleModelChanged(object model) {
      canvas.QueueDraw();
    }

    public void HandleDrawingArea1ButtonPressEvent(object o, Gtk.ButtonPressEventArgs args) {
      controller.Handle(args.Event);
    }

    public void HandleDrawingArea1MotionNotifyEvent(object o, Gtk.MotionNotifyEventArgs args) {
      controller.Handle(args.Event);
    }

    public void HandleDrawingArea1ButtonReleaseEvent(object o, Gtk.ButtonReleaseEventArgs args) {
      controller.Handle(args.Event);
    }

    public void HandleDrawingArea1ExposeEvent(object o, Gtk.ExposeEventArgs args) {
      using (Cairo.Context context = Gdk.CairoHelper.Create(((Gtk.DrawingArea)o).GdkWindow)) {
        view.Draw(context, model);
      }
    }

    public void DrawSchema(DataModel model) {
      List<TextBlockShape> drawnBlocks = new List<TextBlockShape>();
      Dictionary<string, TextBlockShape> basicBlocks = 
        new Dictionary<string, TextBlockShape>();

      int x = 20, y = 30;
      TextBlockShape textBlock = new TextBlockShape(new Rectangle(x, y, 40, 40), /*autoSize*/true);
      foreach (Table t in model.DB.Tables) {
        textBlock = new TextBlockShape(new Cairo.Rectangle(x, y, 40, 40), /*autoSize*/true);
        x += 200; 

        textBlock.Style.Border = new SolidPattern(new Color(0, 0, 0));
        textBlock.Title = t.Name;
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < t.Columns.Count; i++) 
          sb.AppendLine(t.Columns[i].Meta.Name);

        textBlock.BlockText = sb.ToString();

        basicBlocks.Add(t.Name, textBlock);
        scene.Shapes.Add(textBlock);

        drawnBlocks.Add(textBlock);
      }

      int maxY = 0;
      // Add some test arrows to the canvas
      for (int i = 0, e = drawnBlocks.Count - 1; i < e; i++) {
        ArrowShape arrow = new ArrowShape(drawnBlocks[i], drawnBlocks[i+1]);
        if (maxY < drawnBlocks[i].Height)
          maxY = (int)drawnBlocks[i].Height;

        scene.Shapes.Add(arrow);
      }

      // Set the size of the DrawingArea in order to have the scroller moving properly

      canvas.SetSizeRequest((int)(drawnBlocks[drawnBlocks.Count -1].Rectangle.X 
                              + drawnBlocks[drawnBlocks.Count -1].Rectangle.Width), maxY);
    }
  }
}

