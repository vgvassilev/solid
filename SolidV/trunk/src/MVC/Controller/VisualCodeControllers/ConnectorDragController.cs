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

  public class ConnectorDragController : ShapeDragController
  {
    protected Glue dragGlue = null;
    protected ConnectorShape dragConnector = null;

    public ConnectorDragController() : base() {}
    public ConnectorDragController(Model model, IView<Context, Model> view) : base(model, view) {}

    private Glue CheckGlues(PointD point, Context context, params Shape[] glueShapes)
    {
      foreach (Shape g in glueShapes) {
        if (g != null && g is Glue && g.IsPointInShape(point, context, View)) return g as Glue;
      }
      return null;
    }

    private Glue CheckGlues(PointD point, Context context, List<Shape> glueShapes)
    {
      foreach (Shape g in glueShapes) {
        if (g != null && g is Glue && g.IsPointInShape(point, context, View)) return g as Glue;
      }
      return null;
    }

    public override bool Handle(Gdk.Event evnt) {
      Gdk.EventButton eventButton = evnt as Gdk.EventButton;
      if (eventButton != null) {
        if (eventButton.Type == Gdk.EventType.ButtonPress) {
          using (Context context = Gdk.CairoHelper.Create(evnt.Window)) {
            foreach (Shape shape in this.Model.GetSubModel<ShapesModel>().Shapes) {
              if (shape is ConnectorShape) {
                Glue g = CheckGlues(new PointD(eventButton.X, eventButton.Y), context, (shape as ConnectorShape).FromGlue, (shape as ConnectorShape).ToGlue);
                if (g != null) {
                  dragGlue = g;
                  isDragging = true;
                  moved = false;
                  dragConnector = shape as ConnectorShape;
//                  lastX = eventButton.X;
//                  lastY = eventButton.Y;
                  return true;
                }
              }
            }
          }
          foreach (Shape shape in this.Model.GetSubModel<SelectionModel>().Selected) {
            if (shape is ConnectorShape) return true;
          }
        } else if (eventButton.Type == Gdk.EventType.ButtonRelease) {
          if (dragGlue != null) {
            using (Context context = Gdk.CairoHelper.Create(evnt.Window)) {
              foreach (Shape shape in this.Model.GetSubModel<ShapesModel>().Shapes) {
                List<Shape> glues = new List<Shape>();
                foreach (Shape subShape in shape.Items) {
                  if (subShape is Glue) glues.Add(subShape);
                }
                Glue g = CheckGlues(new PointD(eventButton.X, eventButton.Y), context, glues);
                if (g != null) {
                  this.Model.BeginUpdate();
                  
                  if (dragGlue == dragConnector.FromGlue) {
                    dragConnector.FromGlue = g;
                    dragConnector.From = g.Parent;
                  } else {
                    dragConnector.ToGlue = g;
                    dragConnector.To = g.Parent;
                  }

                  this.Model.EndUpdate();

                  break;
                }
              }
            }

            isDragging = false;
            dragGlue = null;
            return true;
          }
        }
      }

      Gdk.EventMotion eventMotion = evnt as Gdk.EventMotion;
      if (eventMotion != null) {
        if (isDragging && dragGlue != null) {
/*
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
*/
          return true;
        }
      }

      return base.Handle(evnt);
    }
  }
}
