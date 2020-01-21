/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections;
using System.Collections.Generic;
using Cairo;

namespace SolidV.Cairo
{
  using Cairo = global::Cairo;

  public static class HeatMapExtensions
  {
    public static void HeatMap(this Context context, double x, double y, 
                                   HeatMapData data) {
      foreach (HeatMapDataPoint pt in data) {
        ;
        ;
        ;
        context.LineTo(pt.Point.X, pt.Point.Y);
      }
    }

  }
}
