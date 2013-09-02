/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.MVC
{
  
  public class ConstrainsController : AbstractController<Gdk.Event, Context, Model>
  {
    public ConstrainsController() : base() {}
    public ConstrainsController(Model model, IView<Context, Model> view) : base(model, view) {}
    
    public override void Handle(Gdk.Event evnt) {
      ConstrainsModel constrains = this.Model.GetSubModel<ConstrainsModel>();
      //TODO: implementation
    }
  }
}
