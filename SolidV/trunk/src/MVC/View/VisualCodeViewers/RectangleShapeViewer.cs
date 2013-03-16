/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  public class RectangleShapeViewer : ShapeViewer
  {
    public RectangleShapeViewer()
    {
    }
    
    public override void DrawItem(IView<Context, Model> view, Context context, object item)
    {
      base.DrawItem(view, context, (Shape)item);
    }
    
  }
}
