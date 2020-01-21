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

  public class HeatMapData : IEnumerable<HeatMapDataPoint>
  {
    private HeatMapDataPoint[] data;

    public IEnumerator<HeatMapDataPoint> GetEnumerator()
    {
      foreach (var item in data) yield return item;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
