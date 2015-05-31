/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;
using Cairo;

namespace SolidV.MVC
{
  public class ShapeDragController : AbstractController<Gdk.Event, Context, Model>
  {
    protected bool isDragging = false;
    protected double lastX;
    protected double lastY;
    protected double firstX;
    protected double firstY;
    protected bool moved = false;

    public ShapeDragController(Model model, IView<Context, Model> view) : base(model, view) {}

    public override bool HandleEvent(Gdk.Event evnt) {
      Gdk.EventButton eventButton = evnt as Gdk.EventButton;
      if (eventButton != null) {
        if (eventButton.Type == Gdk.EventType.ButtonPress) {
          isDragging = true;
          moved = false;
          firstX = eventButton.X;
          firstY = eventButton.Y;
          lastX = firstX;
          lastY = firstY;
          //
          using (Context context = Gdk.CairoHelper.Create(evnt.Window)) {
            ViewMode oldViewMode = View.Mode;
            View.Mode = ViewMode.Select;
            foreach (Shape shape in this.Model.GetSubModel<SelectionModel>().Selected) {
              if (shape.IsPointInShape(new PointD(eventButton.X, eventButton.Y), context, View)) {
                InteractionStateModel interState = this.Model.GetSubModel<InteractionStateModel>();
                this.Model.BeginUpdate();
                interState.Interaction.AddRange((List<Shape>)this.Model.GetSubModel<SelectionModel>().Selected.DeepCopy());
                this.Model.EndUpdate();
                View.Mode = oldViewMode;
                return true;
              }
            }
            View.Mode = oldViewMode;
          }
          //
          isDragging = false;
          return false;
        } else if (eventButton.Type == Gdk.EventType.ButtonRelease) {
          if (isDragging) {
            isDragging = false;
            this.Model.BeginUpdate();
            using (Context context = Gdk.CairoHelper.Create(evnt.Window)) {
              foreach (Shape shape in this.Model.GetSubModel<SelectionModel>().Selected) {
                context.Save();
                context.Matrix = shape.Matrix;
                Distance dist = shape.DistanceToLocal(new Distance(eventButton.X - firstX, eventButton.Y - firstY), context);
                shape.Matrix.Translate(dist.Dx, dist.Dy);
                context.Restore();
              }
            }
            this.Model.GetSubModel<InteractionStateModel>().Interaction.Clear();
            this.Model.EndUpdate();
            return true;
          }
        }
      }

      Gdk.EventMotion eventMotion = evnt as Gdk.EventMotion;
      if (eventMotion != null) {
        if (isDragging) {
          this.Model.BeginUpdate();
          using (Context context = Gdk.CairoHelper.Create(evnt.Window)) {
            foreach (Shape shape in this.Model.GetSubModel<InteractionStateModel>().Interaction) {
              context.Save();
              context.Matrix = shape.Matrix;
              Distance dist = shape.DistanceToLocal(new Distance(eventMotion.X - lastX, eventMotion.Y - lastY), context);
              shape.Matrix.Translate(dist.Dx, dist.Dy);
              context.Restore();
            }
          }
          this.Model.EndUpdate();
          lastX = eventMotion.X;
          lastY = eventMotion.Y;
          moved = true;
          return true;
        }
      }

      Gdk.EventKey eventKey = evnt as Gdk.EventKey;
      if (eventKey != null) {
        if (isDragging && eventKey.Key == Gdk.Key.Escape) {
          this.Model.BeginUpdate();
          this.Model.GetSubModel<InteractionStateModel>().Interaction.Clear();
          this.Model.EndUpdate();
          isDragging = false;
          moved = false;
          return true;
        }
      }

      return false;
    }
  }
}
