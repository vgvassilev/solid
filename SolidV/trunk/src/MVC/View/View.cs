/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

namespace SolidV.MVC
{
  public class View<C, M> : IView<C, M>
  {
    private Dictionary<Type, IViewer<C, M>> viewers = new Dictionary<Type, IViewer<C, M>>();
    public Dictionary<Type, IViewer<C, M>> Viewers {
      get { return viewers; }
      set { viewers = value; }
    }
    
    public View()
    {
    }
    
    public void Draw(C context, M model) {
      DrawItem(context, model);
    }
    
    public void DrawItem(C context, object item) {
      IViewer<C, M> viewer;
      if (Viewers.TryGetValue(item.GetType(), out viewer)) {
        viewer.DrawItem(this, context, item);
      }
    }
    
  }
}
