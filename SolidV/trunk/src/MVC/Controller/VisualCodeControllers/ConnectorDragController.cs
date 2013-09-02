/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.MVC
{

  public class ConnectorDragController : AbstractController<Gdk.Event, Context, Model>
  {
    private bool isDragging = false;
    private double lastX;
    private double lastY;
    
    public ConnectorDragController() : base() {}
    public ConnectorDragController(Model model, IView<Context, Model> view) : base(model, view) {}
    
    public override void Handle(Gdk.Event evnt) {
      //TODO: Implementation
    }
  }
}
