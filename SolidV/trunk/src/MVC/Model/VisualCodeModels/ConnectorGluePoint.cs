/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.MVC
{
  [Serializable]
  public class ConnectorGluePoint: GlueRegion
  {
    public const double PointSize = 6;
    public const double PointCenter = 3;

    public ConnectorGluePoint(Rectangle rectangle) : base(rectangle) {}
    public ConnectorGluePoint(PointD point) : base(new Rectangle(point.X-PointCenter, point.Y-PointCenter, PointSize, PointSize)) {}
  }
}
