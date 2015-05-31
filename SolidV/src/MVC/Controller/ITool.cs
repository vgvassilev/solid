/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  public interface ITool<E, C, M> 
  {
    //IController<E, global::Cairo.Context, Model> Controller(Model model, IView<global::Cairo.Context, Model> view);
    IController<E, C, M> Controller();
    ICommand Command();
  }

}
