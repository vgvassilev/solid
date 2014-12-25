/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  public interface IViewer<C, M>
  {
    void DrawItem(IView<C, M> View, C context, object item);
  }
}
