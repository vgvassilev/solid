/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using Cairo;

using SolidV.MVC;

namespace SolidIDE.Domains.ControlFlow
{

  [Serializable]
  [Domain("ControlFlow")]
  [ShapeName("Arrow Connector")]
  [Tool(typeof(ArrowShape.ToolProvider))]
  public class ArrowShape : SolidV.MVC.ArrowShape, /*IDomain, IUniverseDomain,*/ IControlFlowDomain
  {
    private static Shape Nowhere1 = new Glue(new Rectangle(0,8,0,0));
    private static Shape Nowhere2 = new Glue(new Rectangle(30,8,0,0));

    public ArrowShape() : base(Nowhere1, Nowhere2) {}
    public ArrowShape(Shape from, Shape to) : base(from, to) {}
    public ArrowShape(Shape from, Glue fromGlue, Shape to, Glue toGlue) : base(from, fromGlue, to, toGlue) {}

    internal class ToolProvider : ToolAttribute.ToolProvider<Gdk.Event, Cairo.Context, Model>
    {
      public override Tool<Gdk.Event, Cairo.Context, Model> GetTool() {
        return new Tool<Gdk.Event, Cairo.Context, Model>(
          null,
          new SolidIDE.Plugins.Toolbox.AddNewConnectorShapeCommand(new ArrowShape(Nowhere1, Nowhere2))
        );
      }
    }

  }

}
