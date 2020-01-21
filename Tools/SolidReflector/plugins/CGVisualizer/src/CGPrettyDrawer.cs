// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using System;
using SolidOpt.Services.Transformations.CodeModel.CallGraph;
using SolidV.MVC;
using Cairo;
using System.Collections.Generic;

namespace SolidReflector.Plugins.CGVisualizer
{
  public class CGPrettyDrawer
  {
    /// <summary>
    /// The surface that is being painted on.
    /// </summary>
    /// 
    Gtk.DrawingArea canvas = null;
    Model model = new Model();
    ShapesModel scene = new ShapesModel();
    SelectionModel selection = new SelectionModel();
    View<Context, Model> view = new View<Context, Model>();
    CompositeController<Gdk.Event, Context, Model> controller = new CompositeController<Gdk.Event,
    Context, Model>();
    private InteractionStateModel interaction = new InteractionStateModel();

    public CGPrettyDrawer(Gtk.DrawingArea drawingArea) {
      this.canvas = drawingArea;
      
      canvas.AddEvents((int) Gdk.EventMask.ButtonPressMask);
      canvas.AddEvents((int) Gdk.EventMask.ButtonReleaseMask);
      canvas.AddEvents((int) Gdk.EventMask.PointerMotionMask);
      
      canvas.ButtonPressEvent += HandleDrawingArea1ButtonPressEvent;
      canvas.ButtonReleaseEvent += HandleDrawingArea1ButtonReleaseEvent;
      canvas.ExposeEvent += HandleDrawingArea1ExposeEvent;
      canvas.MotionNotifyEvent += HandleDrawingArea1MotionNotifyEvent;
      
      model.RegisterSubModel<ShapesModel>(scene);
      model.RegisterSubModel<SelectionModel>(selection);
      model.RegisterSubModel<InteractionStateModel>(interaction);

      model.ModelChanged += HandleModelChanged;
      
      view.Viewers.Add(typeof(Model), new ModelViewer<Context>());
      view.Viewers.Add(typeof(ShapesModel), new ShapeModelViewer());
      view.Viewers.Add(typeof(RectangleShape), new RectangleShapeViewer());
      view.Viewers.Add(typeof(EllipseShape), new RoundedRectangleShapeViewer ());
      view.Viewers.Add(typeof(ArrowShape), new ArrowShapeViewer());
      view.Viewers.Add(typeof(TextBlockShape), new TextBlockShapeViewer());
      view.Viewers.Add(typeof(SelectionModel), new SelectionModelViewer());
      view.Viewers.Add(typeof(InteractionStateModel), new InteractionStateModelViewer());
      
      controller.SubControllers.Add(new ShapeSelectionController(model, view));
      ChainController<Gdk.Event, Context, SolidV.MVC.Model> cc = 
        new ChainController<Gdk.Event, Context, SolidV.MVC.Model>();
      cc.SubControllers.Add(new ConnectorDragController(model, view));
      cc.SubControllers.Add(new ShapeDragController(model, view));
      controller.SubControllers.Add(cc);
    }

    /// <summary>
    /// Invalidate the canvas.
    /// </summary>
    /// <param name='model'>
    /// Model.
    /// </param>
    void HandleModelChanged(object model)
    {
      canvas.QueueDraw();
    }

    void HandleDrawingArea1ButtonPressEvent(object o, Gtk.ButtonPressEventArgs args) {
      canvas.GrabFocus();
      controller.HandleEvent(args.Event);
    }
    
    void HandleDrawingArea1MotionNotifyEvent(object o, Gtk.MotionNotifyEventArgs args)
    {
      controller.HandleEvent(args.Event);
    }
    
    void HandleDrawingArea1ButtonReleaseEvent(object o, Gtk.ButtonReleaseEventArgs args)
    {
      controller.HandleEvent(args.Event);
    }
    
    void HandleDrawingArea1ExposeEvent(object o, Gtk.ExposeEventArgs args) {
      using (Cairo.Context context = Gdk.CairoHelper.Create(((Gtk.DrawingArea)o).GdkWindow)) {
        view.Draw(context, model);
      }
    }

    public void DrawCallGraph(CallGraph cg) {
      DrawCG(cg);
    }
    
    private void DrawCG(CallGraph cg) {
      var visited = new Dictionary<CGNode, EllipseShape>();
      DrawMethod(cg.Root, ref visited);
    }

    private void DrawMethod(CGNode node, ref Dictionary<CGNode, EllipseShape> visited) {
      ArrowShape arrow = null;
      // When autoSize is on the rectangle parameter will be 'ignored'.
      EllipseShape blockShape = new EllipseShape(new Rectangle(1,1,70,70));
      blockShape.Title = String.Format("{0}", node.Method.Name);
      visited.Add(node, blockShape);
      scene.Shapes.Add(blockShape);
      
      ConnectorGluePoint gluePointStart = null;
      ConnectorGluePoint gluePointEnd = null;
      
      foreach (CGNode child in node.MethodCalls) {
        DrawMethod(child, ref visited);
        gluePointStart = new ConnectorGluePoint(new PointD(visited[node].Location.X + visited[node].Width / 2, visited[node].Location.Y + visited[node].Height));
        gluePointEnd = new ConnectorGluePoint(new PointD(visited[child].Location.X + visited[child].Width / 2, visited[child].Location.Y));
        
        arrow = new ArrowShape(visited[node], gluePointStart, visited[child], gluePointEnd);
        arrow.ArrowKindHead = SolidV.Cairo.ArrowKinds.SharpArrow;
        arrow.ArrowKindTail = SolidV.Cairo.ArrowKinds.NoArrow;
        
        gluePointEnd.Parent = visited[child];
        gluePointStart.Parent = visited[node];
        
        scene.Shapes.Add(arrow);
        visited[node].Items.Add(gluePointStart);
        visited[child].Items.Add(gluePointEnd);
      }
    }
  }
}
