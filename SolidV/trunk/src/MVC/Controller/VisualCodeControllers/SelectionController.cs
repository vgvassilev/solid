/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  public class SelectionController<Event, C, M> : AbstractController<Event, C, M>
  {
    public SelectionController() : base() {}
    public SelectionController(M model, IView<C, M> view) : base(model, view) {}

    public override void Handle(Event evnt) {
      //TODO: implementation
    }
  }
}

