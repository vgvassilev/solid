/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  public abstract class AbstractController<Event> : IController<Event>
  {
    public event EventHandler<Event> EventHandlers;

    public AbstractController() {
    }


  }
}

