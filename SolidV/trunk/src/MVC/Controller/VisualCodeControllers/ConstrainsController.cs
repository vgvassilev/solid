/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;
using Gdk;

namespace SolidV.MVC
{
  
  public class ConstrainsController : AbstractController<Gdk.Event, Context, Model>
  {
    public ConstrainsController() : base() {}
    public ConstrainsController(Model model, IView<Context, Model> view) : base(model, view) {}
    
    public override bool Handle(Gdk.Event evnt) {
      ConstrainsModel constrains = this.Model.GetSubModel<ConstrainsModel>();
      //TODO: implementation
      return false;
    }
  }
}
