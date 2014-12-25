/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  public abstract class AbstractController<Event, C, M> : IController<Event, C, M>
  {
    //public event EventHandler<Event> EventHandlers;

    private M model;
    public M Model {
      get { return model; }
      set { model = value; }
    }

    private IView<C, M> view;
    public IView<C, M> View {
      get { return view; }
      set { view = value; }
    }

    public virtual bool HandleEvent(Event evnt)
    {
      return false;
    }

    public AbstractController() {}

    public AbstractController(M model, IView<C, M> view) {
      this.Model = model;
      this.View = view;
    }
  }
}

