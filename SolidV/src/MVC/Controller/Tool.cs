/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{

  public class Tool<E, C, M> : ITool<E, C, M>
  {
    private IController<E, C, M> controller;
    private ICommand command;

    public Tool() {}

    public Tool(IController<E, C, M> controller, ICommand command) {
      this.controller = controller;
      this.command = command;
    }

    public IController<E, C, M> Controller() {
      return controller;
    }

    public ICommand Command() {
      return command;
    }
  }

}
