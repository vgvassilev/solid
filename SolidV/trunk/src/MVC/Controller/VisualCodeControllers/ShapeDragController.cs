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
          foreach (Shape shape in this.Model.GetSubModel<SelectionModel>().Selected) {
            shape.Rectangle = new Rectangle(shape.Rectangle.X + eventMotion.X - lastX, 
                                              shape.Rectangle.Y + eventMotion.Y - lastY,
                                              shape.Rectangle.Width, shape.Rectangle.Height);
          }
          this.Model.EndUpdate();
          lastX = eventMotion.X;
          lastY = eventMotion.Y;
        }
      }
    }
  }
}

