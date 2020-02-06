/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;
using Cairo;
using SolidV.Cairo;

namespace SolidV.MVC
{
  public class AnnotationController : AbstractController<Gdk.Event, Context, Model>
  {
    public AnnotationController(Model model, IView<Context, Model> view) : base (model, view) { }

    public override bool HandleEvent(Gdk.Event evnt)
    {
      var eventMotion = evnt as Gdk.EventMotion;
      if (eventMotion != null) {
        if ((eventMotion.State & Gdk.ModifierType.ControlMask) != 0) {
          this.Model.BeginUpdate();
          var annotation = this.Model.GetSubModel<AnnotationModel>();
          if (annotation.Shapes.Count > 0) {
            (annotation.Shapes[0] as HeatMapShape).HeatMap.AddPoint(
              new HeatMapDataPoint(new PointD(eventMotion.X, eventMotion.Y), 120));
          }
          this.Model.EndUpdate();
          return true;
        } 
      }

      return false;
    }
  }
}
