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

    public bool pointInShape(double x, double y, Gdk.Event evnt, Shape shape, Context context) {
      context.Save();
      context.Transform(shape.Matrix);
      View.DrawItem(context, shape);
      bool result = context.InFill(x, y) || context.InStroke(x, y);
      context.NewPath();
      context.Restore();
      return result;
    }

    public override void Handle(Gdk.Event evnt) {
      Gdk.EventButton eventButton = evnt as Gdk.EventButton;
      if (eventButton != null) {
        if (eventButton.Type == Gdk.EventType.ButtonPress) {
          SelectionModel selection = this.Model.GetSubModel<SelectionModel>();
          using (Context context = Gdk.CairoHelper.Create(evnt.Window)) {
            double x,y;
            ViewMode oldViewMode = View.Mode;
            View.Mode = ViewMode.Select;
            foreach (Shape shape in this.Model.GetSubModel<ShapesModel>().Shapes) {
              context.Matrix = shape.Matrix;
              x = eventButton.X;
              y = eventButton.Y;
              context.DeviceToUser(ref x, ref y);
              //if (shape.Rectangle.X <= x && shape.Rectangle.Y <= y &&
              //  shape.Rectangle.X + shape.Rectangle.Width >= x && shape.Rectangle.Y + shape.Rectangle.Height >= y &&
              if (pointInShape(x, y, evnt, shape, context)) {
                if (selection.Selected.IndexOf(shape) < 0) {
                  this.Model.BeginUpdate();
                  if ((eventButton.State & Gdk.ModifierType.ControlMask) == 0)
                    selection.Selected.Clear();
                  selection.Selected.Add(shape);
                  this.Model.EndUpdate();
                } else {
                  this.Model.BeginUpdate();
                  if ((eventButton.State & Gdk.ModifierType.ControlMask) != 0)
                    selection.Selected.Remove(shape);
                  this.Model.EndUpdate();
                }
                View.Mode = oldViewMode;
                return;
              }
            }
            View.Mode = oldViewMode;
          }
          this.Model.BeginUpdate();
          selection.Selected.Clear();
          this.Model.EndUpdate();
        }
      }
    }
  }
}