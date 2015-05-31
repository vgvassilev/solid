/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using SolidV.MVC;

namespace SolidIDE.Plugins.Designer
{
  public class Sheet<E, C, M>: ISheet
  {
    private M model;
    public M Model {
      get { return model; }
      set { model = value; }
    }

    private IView<C, M> view;
    public IView<C, M> View {
      get {return view; }
      set { view = value; }
    }

    private IController<E, C, M> controller;
    public IController<E, C, M> Controller {
      get { return controller; }
      set { controller = value; }
    }

    public Sheet(M model, IView<C, M> view, IController<E, C, M> controller) {
      this.model = model;
      this.view = view;
      this.controller = controller;
    }
   
  }

}
