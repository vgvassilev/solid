/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.MVC
{
  public class ShapeDragController : AbstractController<Gdk.Event, Context, Model>
  {
    protected bool isDragging = false;
    protected double lastX;
    protected double lastY;
    protected bool moved = false;

    public ShapeDragController() : base() {}
    public ShapeDragController(Model model, IView<Context, Model> view) : base(model, view) {}

    public override bool Handle(Gdk.Event evnt) {
      Gdk.EventButton eventButton = evnt as Gdk.EventButton;
      if (eventButton != null) {
        if (eventButton.Type == Gdk.EventType.ButtonPress) {
          isDragging = true;
          moved = false;
          lastX = eventButton.X;
          lastY = eventButton.Y;
          return true;
        } else if (eventButton.Type == Gdk.EventType.ButtonRelease) {
          isDragging = false;
          return true;
        }
      }

      Gdk.EventMotion eventMotion = evnt as Gdk.EventMotion;
      if (eventMotion != null) {
        if (isDragging) {
          this.Model.BeginUpdate();
          using (Context context = Gdk.CairoHelper.Create(evnt.Window)) {
            foreach (Shape shape in this.Model.GetSubModel<SelectionModel>().Selected) {
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

      return false;
    }
  }
}

