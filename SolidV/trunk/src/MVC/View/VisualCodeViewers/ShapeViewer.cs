/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  public class ShapeViewer : Viewer<Context, Model>
  {
    public ShapeViewer()
    {
    }
    
    public override void DrawItem(IView<Context, Model> view, Context context, object item)
    {
      context.Rectangle(((Shape)item).Rectangle);
    }
  }
}
