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
    public ShapeSelectionController(Model model, IView<Context, Model> view) : base(model, view) {}

    public override bool Handle(Gdk.Event evnt) {
      Gdk.EventButton eventButton = evnt as Gdk.EventButton;
      if (eventButton != null) {
        if (eventButton.Type == Gdk.EventType.ButtonPress) {
          SelectionModel selection = this.Model.GetSubModel<SelectionModel>();
          using (Context context = Gdk.CairoHelper.Create(evnt.Window)) {
            ViewMode oldViewMode = View.Mode;
            View.Mode = ViewMode.Select;
            foreach (Shape shape in this.Model.GetSubModel<ShapesModel>().Shapes) {
              if (shape.IsPointInShape(new PointD(eventButton.X, eventButton.Y), context, View)) {
                if (!selection.Selected.Contains(shape)) {
                  selection.BeginUpdate();
                  if ((eventButton.State & Gdk.ModifierType.ControlMask) == 0)
                    selection.Selected.Clear();
                  selection.Selected.Add(shape);
                  selection.EndUpdate();
                } else {
                  selection.BeginUpdate();
                  if ((eventButton.State & Gdk.ModifierType.ControlMask) != 0)
                    selection.Selected.Remove(shape);
                  selection.EndUpdate();
                }
                View.Mode = oldViewMode;
                return true;
              }
            }
            View.Mode = oldViewMode;
          }
          selection.BeginUpdate();
          selection.Selected.Clear();
          selection.EndUpdate();
        }
      }

      return false;
    }
  }
}