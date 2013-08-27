/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.MVC
{
  public class ShapeSelectionController : SelectionController<Gdk.Event, Context, Model>
  {
    public ShapeSelectionController() : base() {}
    public ShapeSelectionController(Model model, IView<Context, Model> view) : base(model, view) {}

    public override void Handle(Gdk.Event evnt) {
      Gdk.EventButton eventButton = evnt as Gdk.EventButton;
      if (eventButton != null) {
        if (eventButton.Type == Gdk.EventType.ButtonPress) {
          SelectionModel selection = this.Model.GetSubModel<SelectionModel>();
          foreach (Shape shape in this.Model.GetSubModel<ShapesModel>().Shapes) {
            if (shape.Rectangle.X <= eventButton.X && shape.Rectangle.Y <= eventButton.Y &&
              shape.Rectangle.X + shape.Rectangle.Width >= eventButton.X && shape.Rectangle.Y + 
                shape.Rectangle.Height >= eventButton.Y) {
              if (selection.Selected.IndexOf(shape) < 0) {
                this.Model.BeginUpdate();
                if ((eventButton.State & Gdk.ModifierType.ControlMask) == 0)
                  selection.Selected.Clear();
                selection.Selected.Add(shape);
                this.Model.EndUpdate();
              }
              else {
                this.Model.BeginUpdate();
                if ((eventButton.State & Gdk.ModifierType.ControlMask) != 0)
                  selection.Selected.Remove(shape);
                this.Model.EndUpdate();
              }
              return;
            }
          }
          this.Model.BeginUpdate();
          selection.Selected.Clear();
          this.Model.EndUpdate();
        }
      }
    }
  }
}