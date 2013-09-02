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
    private bool isDragging = false;
    private double lastX;
    private double lastY;

    public ShapeDragController() : base() {}
    public ShapeDragController(Model model, IView<Context, Model> view) : base(model, view) {}

    public override void Handle(Gdk.Event evnt) {
      Gdk.EventButton eventButton = evnt as Gdk.EventButton;
      if (eventButton != null) {
        if (eventButton.Type == Gdk.EventType.ButtonPress) {
          isDragging = true;
          lastX = eventButton.X;
          lastY = eventButton.Y;
        } else if (eventButton.Type == Gdk.EventType.ButtonRelease) {
          isDragging = false;
        }
      }

      Gdk.EventMotion eventMotion = evnt as Gdk.EventMotion;
      if (eventMotion != null) {
        if (isDragging) {
          this.Model.BeginUpdate();
          using (Context context = Gdk.CairoHelper.Create(evnt.Window)) {
            foreach (Shape shape in this.Model.GetSubModel<SelectionModel>().Selected) {
              context.Matrix = shape.Matrix;
              double dx = eventMotion.X - lastX;
              double dy = eventMotion.Y - lastY;
              context.DeviceToUserDistance(ref dx, ref dy);
              shape.Matrix.Translate(dx, dy);
            }
          }
          this.Model.EndUpdate();
          lastX = eventMotion.X;
          lastY = eventMotion.Y;
        }
      }
    }
  }
}

