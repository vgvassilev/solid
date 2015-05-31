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
  public class RectangleShape : Shape, IGluesProvider
  {
    public RectangleShape(Rectangle rectangle) : base(rectangle) {}

    IEnumerable<Glue> IGluesProvider.GetGlues() {
      yield return new ConnectorGluePoint(new PointD(Location.X + Width/2, Location.Y)).SetParent(this);
      yield return new ConnectorGluePoint(new PointD(Location.X + Width/2, Location.Y + Height)).SetParent(this);
      yield return new ConnectorGluePoint(new PointD(Location.X, Location.Y + Height/2)).SetParent(this);
      yield return new ConnectorGluePoint(new PointD(Location.X + Width, Location.Y + Height/2)).SetParent(this);
    }
  }
}
