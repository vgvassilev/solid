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
  public delegate void FocusedRectangleFinishDelegate(FocusRectangleController controller);

  public class FocusRectangleController : AbstractController<Gdk.Event, Context, Model>
  {
    private Rectangle focusedRectangle;
    public Rectangle FocusedRectangle {
      get { return focusedRectangle; }
      set { focusedRectangle = value; }
    }

    public event FocusedRectangleFinishDelegate FocusedRectangleFinish;

    protected bool isDragging = false;
    protected double firstX;
    protected double firstY;
    //protected bool selected;
    protected FocusRectangleShape intercationObject;

    public FocusRectangleController(Model model, IView<Context, Model> view) : base(model, view) {}

    public override bool HandleEvent(Gdk.Event evnt) {
      Gdk.EventButton eventButton = evnt as Gdk.EventButton;
      if (eventButton != null) {
        if (eventButton.Type == Gdk.EventType.ButtonPress) {
          isDragging = true;
          //selected = false;
          FocusedRectangle = default(Rectangle);
          firstX = eventButton.X;
          firstY = eventButton.Y;
          intercationObject = new FocusRectangleShape(new Rectangle(firstX, firstY, 0, 0));
          //
          InteractionStateModel interState = this.Model.GetSubModel<InteractionStateModel>();
          this.Model.BeginUpdate();
          interState.Interaction.Add(intercationObject);
          this.Model.EndUpdate();
          //
          return true;
        } else if (eventButton.Type == Gdk.EventType.ButtonRelease) {
          if (isDragging) {
            isDragging = false;
            this.Model.BeginUpdate();
            this.Model.GetSubModel<InteractionStateModel>().Interaction.Clear();
            this.Model.EndUpdate();
            FocusedRectangle = intercationObject.Rectangle;
            intercationObject = null;
            //selected = true;
            OnFocusedRectangleFinish();
            return true;
          }
        }
      }

      Gdk.EventMotion eventMotion = evnt as Gdk.EventMotion;
      if (eventMotion != null) {
        if (isDragging) {
          this.Model.BeginUpdate();
          /*using (Context context = Gdk.CairoHelper.Create(evnt.Window)) {
            foreach (Shape shape in this.Model.GetSubModel<InteractionStateModel>().Interaction) {
              context.Save();
              context.Matrix = shape.Matrix;
              Distance dist = shape.DistanceToLocal(new Distance(eventMotion.X - lastX, eventMotion.Y - lastY), context);
              shape.Matrix.Translate(dist.Dx, dist.Dy);
              context.Restore();
            }
          }*/
          intercationObject.Width = eventMotion.X - firstX;
          intercationObject.Height = eventMotion.Y - firstY;
          //intercationObject.Rectangle = new Rectangle(
          //  intercationObject.Rectangle.X, intercationObject.Rectangle.Y,
          //  eventMotion.X - intercationObject.Rectangle.X, eventMotion.Y - intercationObject.Rectangle.Y
          //);
          this.Model.EndUpdate();
          return true;
        }
      }

      Gdk.EventKey eventKey = evnt as Gdk.EventKey;
      if (eventKey != null) {
        if (isDragging && eventKey.Key == Gdk.Key.Escape) {
          this.Model.BeginUpdate();
          this.Model.GetSubModel<InteractionStateModel>().Interaction.Clear();
          this.Model.EndUpdate();
          intercationObject = null;
          isDragging = false;
          //selected = false;
          return true;
        }
      }

      return false;
    }

    protected void OnFocusedRectangleFinish() {
      if (FocusedRectangleFinish != null) FocusedRectangleFinish(this);
    }

  }
}
