/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  public class ShapeModelViewer : Viewer<Context, Model>
  {
    public ShapeModelViewer()
    {
    }
    
    public override void DrawItem(IView<Context, Model> view, Context context, object item)
    {
      foreach (Shape shape in ((ShapesModel)item).Shapes) {
        view.DrawItem(context, shape);
      }
    }
  }
}
