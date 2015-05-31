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
  [ShapeName("Start Block")]
  public class StartShape : EllipseShape, /*IDomain, IUniverseDomain,*/ IControlFlowDomain, IGluesProvider
  {
    public StartShape() : this(new Rectangle()) {}
    public StartShape(Rectangle rectangle) : base(rectangle) {
      var text = new TextBlockShape(rectangle);
      text.BlockText = "Start";
      Items.Add(text);
    }

    public IEnumerable<Glue> GetGlues() {
      //foreach (Glue glue in base.GetGlues()) yield return glue;
      //yield return new ConnectorGluePoint(Center).SetParent(this);
      yield return new ConnectorGluePoint(new PointD(Location.X + Width/2, Location.Y + Height))
        .SetParent(this);
    }

  }

}
