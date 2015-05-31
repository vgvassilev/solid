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
  [ShapeName("Call Block")]
  [Viewer(typeof(CallShapeViewer))]
  public class CallShape : RectangleShape, /*IDomain, IUniverseDomain,*/ IControlFlowDomain
  {
    public CallShape() : base(new Rectangle()) {}
    public CallShape(Rectangle rectangle) : base(rectangle) {}
  }

}
