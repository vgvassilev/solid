/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  public class Viewer<C, M> : IViewer<C, M>
  {
    public Viewer()
    {
    }
    
    public virtual void DrawItem(IView<C, M> view, C context, object item)
    {
    }
    
  }
}
