/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  public interface IView<C, M>
  {
    ViewMode Mode { get; set; }
    void Draw(C context, M model);
    void DrawItem(C context, object item);
  }
}
