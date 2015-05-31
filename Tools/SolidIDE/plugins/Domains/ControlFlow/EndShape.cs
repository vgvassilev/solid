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
  [ShapeName("End Block")]
  public class EndShape : EllipseShape, /*IDomain, IUniverseDomain,*/ IControlFlowDomain, IGluesProvider
  {
    public EndShape() : this(new Rectangle()) {}
    public EndShape(Rectangle rectangle) : base(rectangle) {
      var text = new TextBlockShape(rectangle);
      text.BlockText = "End";
      Items.Add(text);
    }

    public IEnumerable<Glue> GetGlues() {
      //foreach (Glue glue in base.GetGlues()) yield return glue;
      //yield return new ConnectorGluePoint(Center).SetParent(this);
      yield return new ConnectorGluePoint(new PointD(Location.X + Width/2, Location.Y))
        .SetParent(this);
    }

  }

}
