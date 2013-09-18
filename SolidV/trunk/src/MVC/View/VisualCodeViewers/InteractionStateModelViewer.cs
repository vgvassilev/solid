/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  public class InteractionStateModelViewer : Viewer<Context, Model>
  {
    public InteractionStateModelViewer()
    {
    }
    
    public override void DrawItem(IView<Context, Model> view, Context context, object item)
    {
      context.Save();
      context.PushGroup();
      foreach (Shape shape in ((InteractionStateModel)item).Interaction) {
        context.Save();
        view.DrawItem(context, shape);
        context.Restore();
      }
      context.PopGroupToSource();
      context.PaintWithAlpha(0.5);
      context.Restore();
    }
  }
}
