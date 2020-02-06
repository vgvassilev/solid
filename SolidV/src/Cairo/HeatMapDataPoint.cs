/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.Cairo
{
  [Serializable]
  public struct HeatMapDataPoint
  {
    public PointD Point;
    public float Height;
    
    public HeatMapDataPoint(PointD point, float height)
    {
        Point = point;
        Height = height;
    }
  }
}
