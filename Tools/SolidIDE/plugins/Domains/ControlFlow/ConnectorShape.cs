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
  [ShapeName("Connector Block")]
  public class ConnectorShape : EllipseShape, /*IDomain, IUniverseDomain,*/ IControlFlowDomain
  {
    public ConnectorShape() : this(new Rectangle()) {}

    public ConnectorShape(Rectangle rectangle) : base(new Rectangle(rectangle.X, rectangle.Y, 10, 10)) {
      Style = Style.DeepCopy();
      Style.Fill = new SolidPattern(0, 0, 0);
    }

    public override Rectangle Rectangle {
      set { base.Rectangle = new Rectangle(value.X, value.Y, 10, 10); }
    }
  }

}
