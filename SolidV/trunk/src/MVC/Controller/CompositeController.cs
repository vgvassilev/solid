/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

namespace SolidV.MVC
{
  public class CompositeController<Event> : AbstractController<Event>
  {
    private List<IController<Event>> subControllers = new List<IController<Event>>();
    public List<IController<Event>> SubControllers {
      get { return subControllers; }
      set { subControllers = value; }
    }

    public CompositeController() {
    }
    
  }
}

