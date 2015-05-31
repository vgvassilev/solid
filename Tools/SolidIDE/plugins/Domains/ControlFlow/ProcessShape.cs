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
  [ShapeName("Process Block")]
  public class ProcessShape : RectangleShape, /*IDomain, IUniverseDomain,*/ IControlFlowDomain
  {
    public ProcessShape() : base(new Rectangle()) {}
    public ProcessShape(Rectangle rectangle) : base(rectangle) {}
  }

}
