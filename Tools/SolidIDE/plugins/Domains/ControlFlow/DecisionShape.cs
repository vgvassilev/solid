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
  [ShapeName("Decision Block")]
  public class DecisionShape : DiamondShape, /*IDomain, IUniverseDomain,*/ IControlFlowDomain
  {
    public DecisionShape() : base(new Rectangle()) {}
    public DecisionShape(Rectangle rectangle) : 
      base(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Width)) {}
  }

}
