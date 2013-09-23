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
      view.Viewers.Add(typeof(EllipseShape), new EllipseShapeViewer());
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
      controller.Handle(args.Event);
    }
    
    void HandleDrawingArea1MotionNotifyEvent(object o, Gtk.MotionNotifyEventArgs args)
    {
      controller.Handle(args.Event);
    }
    
    void HandleDrawingArea1ButtonReleaseEvent(object o, Gtk.ButtonReleaseEventArgs args)
    {
      controller.Handle(args.Event);
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
      var visited = new Dictionary<CGNode, TextBlockShape>();
      DrawMethod(cg.Root, ref visited);
    }

    private void DrawMethod(CGNode node, ref Dictionary<CGNode, TextBlockShape> visited) {
      ArrowShape arrow = null;
      // When autoSize is on the rectangle parameter will be 'ignored'.
      TextBlockShape blockShape = new TextBlockShape(new Rectangle(1,1,50,50), /*autosize*/true);
      //blockShape.Title = String.Format("Block Name: {0}", node.Method.Name);
      blockShape.BlockText = node.Method.Name;
      visited.Add(node, blockShape);
      scene.Shapes.Add(blockShape);
      
      ConnectorGluePoint gluePointStart = null;
      ConnectorGluePoint gluePointEnd = null;
      
      foreach (CGNode child in node.MethodCalls) {
        DrawMethod(child, ref visited);
        gluePointStart = new ConnectorGluePoint(new PointD(visited[node].Location.X + visited[node].Width / 2, visited[node].Location.Y + visited[node].Height));
        gluePointEnd = new ConnectorGluePoint(new PointD(visited[child].Location.X + visited[child].Width / 2, visited[child].Location.Y));
        
        arrow = new ArrowShape(visited[node], gluePointStart, visited[child], gluePointEnd);
        arrow.ArrowKindHead = SolidV.Cairo.ArrowKinds.TriangleRoundArrow;
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
