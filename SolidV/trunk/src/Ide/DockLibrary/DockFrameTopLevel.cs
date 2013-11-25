/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Gtk;

namespace SolidV.Ide.Dock
{
  class DockFrameTopLevel: EventBox
  {
    int x, y;
    
    public int X {
      get { return x; }
      set {
        x = value;
        if (Parent != null)
          Parent.QueueResize ();
      }
    }
    
    public int Y {
      get { return y; }
      set {
        y = value;
        if (Parent != null)
          Parent.QueueResize ();
      }
    }
  }

}
