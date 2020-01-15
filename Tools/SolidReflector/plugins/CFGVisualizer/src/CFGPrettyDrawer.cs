using Cairo;
using Mono.Cecil;
using System;
using System.Collections.Generic;

using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;
using SolidOpt.Services.Transformations.Multimodel.ILtoCFG;
using SolidV.MVC;
using Mono.Cecil.Cil;

namespace SolidReflector.Plugins.CFGVisualizer
{
  public class CFGPrettyDrawer
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
    CompositeController<Gdk.Event, Context, Model> controller;

    private InteractionStateModel interaction = new InteractionStateModel();

    public CFGPrettyDrawer(Gtk.DrawingArea drawingArea) {
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
      view.Viewers.Add(typeof(Label), new LabelViewer());
      view.Viewers.Add(typeof(TextBlockShape), new RoundedTextBlockShapeViewer());
      view.Viewers.Add(typeof(SelectionModel), new SelectionModelViewer());
      view.Viewers.Add(typeof(Glue), new GlueViewer());
      view.Viewers.Add(typeof(InteractionStateModel), new InteractionStateModelViewer());

      controller = new CompositeController<Gdk.Event, Context, Model>(model, view);
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

    public void DrawTextBlocks(ControlFlowGraph<Instruction> cfg) {
      DrawCFG(cfg);
    }

    private void DrawBasicBlock(BasicBlock<Instruction> basicBlock, 
                                ref Dictionary<BasicBlock<Instruction>, TextBlockShape> visited) {
      if (visited.ContainsKey(basicBlock))
        return;
      
      ArrowShape arrow = null;
      string labelText = "";
      // When autoSize is on the rectangle parameter will be 'ignored'.
      TextBlockShape blockShape = new TextBlockShape(new Rectangle(1,1,50,50), /*autosize*/true);
      blockShape.Title = String.Format("Block Name: {0}", basicBlock.Name);
      blockShape.BlockText = basicBlock.ToString();
      visited.Add(basicBlock, blockShape);
      scene.Shapes.Add(blockShape);
      
      ConnectorGluePoint gluePointStart = null;
      ConnectorGluePoint gluePointEnd = null;

      SolidV.MVC.Label lb = null;

      foreach (BasicBlock<Instruction> successor in basicBlock.Successors) {
        DrawBasicBlock(successor, ref visited);

        gluePointStart = new ConnectorGluePoint(new PointD(visited[basicBlock].Location.X + visited[basicBlock].Width / 2, visited[basicBlock].Location.Y + visited[basicBlock].Height));
        gluePointEnd = new ConnectorGluePoint(new PointD(visited[successor].Location.X + visited[successor].Width / 2, visited[successor].Location.Y));

        //arrow = new ArrowShape(visited[basicBlock], gluePointStart, visited[successor], gluePointEnd);
        arrow = new BezierCurvedArrowShape(visited[basicBlock], gluePointStart, visited[successor], gluePointEnd);
        arrow.ArrowKindHead = SolidV.Cairo.ArrowKinds.SharpArrow;
        arrow.ArrowKindTail = SolidV.Cairo.ArrowKinds.NoArrow;

        gluePointEnd.Parent = visited[successor];
        gluePointStart.Parent = visited[basicBlock];

        switch (basicBlock.Last.OpCode.FlowControl) {
          case FlowControl.Cond_Branch:
            if ((basicBlock.Last.Operand as Instruction).Offset == successor.First.Offset)
              labelText = "IfTrue";
            else
              labelText = "IfFalse";
            break;
          default:
            labelText = "branch";
            break;
        }

        lb = new SolidV.MVC.Label(arrow, labelText);
        lb.Parent = arrow;
        arrow.Items.Add(lb);

        blockShape.Items.Add(arrow);

        visited[basicBlock].Items.Add(gluePointStart);
        visited[successor].Items.Add(gluePointEnd);
      }
    }

    /// <summary>
    /// Draws the CFG on the canvas using its model.
    /// </summary>
    /// <param name='cfg'>
    /// The CFG object.
    /// </param>
    private void DrawCFG(ControlFlowGraph<Instruction> cfg) {
      var visited = new Dictionary<BasicBlock<Instruction>, TextBlockShape>(10);
      DrawBasicBlock(cfg.Root, ref visited);
      
      //scene.AutoArrange();
      
      int maxX = 0, maxY = 0;
      foreach (Shape shape in scene.Shapes) {
        if ((shape.Rectangle.X + shape.Rectangle.Width) > maxX)
          maxX = (int)(shape.Rectangle.X + shape.Rectangle.Width);
        if ((shape.Rectangle.Y + shape.Rectangle.Height) > maxY) 
          maxY = (int)(shape.Rectangle.Y + shape.Rectangle.Height);
      }
      
      canvas.SetSizeRequest(maxX + 10, maxY + 10);
    }
  }
}