/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  public delegate void EventHandler<Event>(Event evnt);

  public interface IController<Event, C, M>
  {
    //event EventHandler<Event> EventHandlers;
    void Handle(Event evnt);
  }
}
