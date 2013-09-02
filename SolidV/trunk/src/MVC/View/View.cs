/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

namespace SolidV.MVC
{
  public class View<C, M> : IView<C, M>
  {
    private ViewMode mode = ViewMode.Render;
    public ViewMode Mode {
      get { return mode; }
      set { mode = value; }
    }

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
//      IViewer<C, M> viewer;
//      if (Viewers.TryGetValue(itemType, out viewer)) {
//        viewer.DrawItem(this, context, item);
//        return;
//      }

      IViewer<C, M> viewer;
      Type itemType = item.GetType();
      while (itemType != null && !Viewers.TryGetValue(itemType, out viewer)) {
        itemType = itemType.BaseType;
      }
      if (itemType != null) viewer.DrawItem(this, context, item);
    }
    
  }
}
