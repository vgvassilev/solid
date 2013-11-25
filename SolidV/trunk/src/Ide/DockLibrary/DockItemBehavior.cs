/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.Ide.Dock
{
  [Flags]
  public enum DockItemBehavior
  {
    Normal,
    NeverFloating = 1 << 0,
    NeverVertical = 1 << 1,
    NeverHorizontal = 1 << 2,
    CantClose = 1 << 3,
    CantAutoHide = 1 << 4,
    NoGrip = 1 << 5,
    Sticky = 1 << 6,  // Visibility is the same for al layouts
    Locked = NoGrip,
  }
}
