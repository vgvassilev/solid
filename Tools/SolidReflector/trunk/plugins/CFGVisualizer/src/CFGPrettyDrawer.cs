using Cairo;
using Mono.Cecil;
using System;
using System.Collections.Generic;

using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;
using SolidOpt.Services.Transformations.Multimodel.ILtoCFG;
using SolidV.MVC;

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
    CompositeController<Gdk.Event, Context, Model> controller = new CompositeController<Gdk.Event,
                                                                                Context, Model>();

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
      model.ModelChanged += HandleModelChanged;

      view.Viewers.Add(typeof(Model), new ModelViewer<Context>());
      view.Viewers.Add(typeof(ShapesModel), new ShapeModelViewer());
      view.Viewers.Add(typeof(RectangleShape), new RectangleShapeViewer());
      view.Viewers.Add(typeof(EllipseShape), new EllipseShapeViewer());
      view.Viewers.Add(typeof(ArrowShape), new ArrowShapeViewer());
      view.Viewers.Add(typeof(TextBlockShape), new TextBlockShapeViewer());
      view.Viewers.Add(typeof(SelectionModel), new SelectionModelViewer());

      controller.SubControllers.Add(new ShapeSelectionController(model, view));
      controller.SubControllers.Add(new ShapeDragController(model, view));
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

    public void DrawTextBlocks(MemberReference memberRef) {
      if (memberRef is MethodDefinition) {
        var builder = new ControlFlowGraphBuilder(memberRef as MethodDefinition);
        ControlFlowGraph cfg = builder.Create();
        DrawCFG(cfg);
      }
    }

    /// <summary>
    /// Draws the CFG on the canvas using its model.
    /// </summary>
    /// <param name='cfg'>
    /// The CFG object.
    /// </param>
    private void DrawCFG(ControlFlowGraph cfg) {
      Dictionary<string, TextBlockShape> basicBlocks = new Dictionary<string, TextBlockShape>();
      List<BasicBlock> drawnBlocks = new List<BasicBlock>();
      TextBlockShape textBlock = new TextBlockShape();

      foreach (BasicBlock basicBlock in cfg.RawBlocks) {
        textBlock = new TextBlockShape();
        textBlock.BlockText = "Block Name: " + basicBlock.Name +
                                                                              Environment.NewLine;
        textBlock.BlockText = textBlock.BlockText + basicBlock.First.ToString() +
                                                                              Environment.NewLine;
        textBlock.BlockText = textBlock.BlockText + "..." + Environment.NewLine;
        textBlock.BlockText = textBlock.BlockText + basicBlock.Last.ToString() +
                                                                              Environment.NewLine;
        basicBlocks.Add(basicBlock.Name, textBlock);
        scene.Shapes.Add(textBlock);
      }

      TextBlockShape fromTextShape = null;
      TextBlockShape toTextShape = null;
      int blockX = canvas.Allocation.Width / 2;
      int blockY = 10;

      foreach (BasicBlock block in cfg.RawBlocks) {
        basicBlocks.TryGetValue(block.Name, out fromTextShape);

        if (!drawnBlocks.Contains(block)) {
          fromTextShape.Rectangle = new Cairo.Rectangle(blockX, blockY, 150, 100);
          drawnBlocks.Add(block);
        }

        blockY += 140;
        blockX = 160;

        for (int i = 0; i < block.Successors.Count; i++) {
          basicBlocks.TryGetValue(block.Successors[i].Name, out toTextShape);
          drawnBlocks.Add(block.Successors[i]);
          toTextShape.Rectangle = new Cairo.Rectangle(blockX, blockY, 150, 100);
          ArrowShape arrow = new ArrowShape(fromTextShape, toTextShape);
          scene.Shapes.Add(arrow);
          blockX += 160;
        }
      }
    }
  }
}