/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  public class ShapeViewer : Viewer<Context, Model>, SolidOpt.Services.IService
  {
    public ShapeViewer()
    {
    }

    public virtual void DrawShape(IView<Context, Model> view, Context context, Shape shape) {
      //
    }
    
    public override void DrawItem(IView<Context, Model> view, Context context, object item)
    {
      Shape shape = (Shape)item;
      
      context.Transform(shape.Matrix);
      context.Save();
      DrawShape(view, context, shape);
      context.Restore();
      foreach (Shape subShape in shape.Items) {
        context.Save();
        view.DrawItem(context, subShape);
        context.Restore();
      }
    }
  }
}
