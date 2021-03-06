/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV
{
  public class ViewMode
  {
    public static ViewMode Render = new ViewModeRender();
    public static ViewMode Select = new ViewModeSelect();

    public class ViewModeRender: ViewMode {}
    public class ViewModeSelect: ViewMode {}
  }
}

