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
  [ShapeName("IO Block")]
  public class IOShape : RectangleShape, /*IDomain, IUniverseDomain,*/ IControlFlowDomain
  {
    public IOShape() : this(new Rectangle()) {}
    public IOShape(Rectangle rectangle) : base(rectangle) {
      Matrix.Init(1, 0, -0.2, 1, 0, 0);
      Matrix.Translate(2, 0);
    }
  }

}
