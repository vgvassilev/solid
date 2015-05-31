/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

namespace SolidV.MVC
{
  public class CompositeController<Event, C, M> : AbstractController<Event, C, M>
  {
    private List<IController<Event, C, M>> subControllers = new List<IController<Event, C, M>>();
    public List<IController<Event, C, M>> SubControllers {
      get { return subControllers; }
      set { subControllers = value; }
    }

    public CompositeController() {}

    public CompositeController(M model, IView<C, M> view) : base(model, view) {}

    public override bool HandleEvent(Event evnt) {
      bool result = false;
      foreach (IController<Event, C, M> controller in SubControllers) {
        if (controller.HandleEvent(evnt))
          result = true;
      }
      return result;
    }
  }
}

