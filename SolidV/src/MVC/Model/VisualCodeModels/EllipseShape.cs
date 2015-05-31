/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

using Cairo;

namespace SolidV.MVC
{
  [Serializable]
  public class EllipseShape : Shape, IGluesProvider
  {
    private string title = null;
    public string Title {
      get { return title; }
      set { title = value; }
    }

    public EllipseShape(Rectangle rectangle) : base(rectangle) {}

    public IEnumerable<Glue> GetGlues() {
      yield return new ConnectorGluePoint(new PointD(Location.X + Width/2, Location.Y)).SetParent(this);
      yield return new ConnectorGluePoint(new PointD(Location.X + Width/2, Location.Y + Height)).SetParent(this);
      yield return new ConnectorGluePoint(new PointD(Location.X, Location.Y + Height/2)).SetParent(this);
      yield return new ConnectorGluePoint(new PointD(Location.X + Width, Location.Y + Height/2)).SetParent(this);
    }

  }
}
