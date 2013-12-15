/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

namespace SolidV.MVC
{
  public class ChainController<Event, C, M> : CompositeController<Event, C, M>
  { 
    public ChainController(M model, IView<C, M> view) : base(model, view) {}
    
    public override bool HandleEvent(Event evnt) {
      foreach (IController<Event, C, M> controller in SubControllers) {
        if (controller.HandleEvent(evnt)) return true;
      }
      return false;
    }
  }
}

