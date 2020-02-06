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

  [Serializable]
  public class HeatMapData : IEnumerable<HeatMapDataPoint>
  {
    private HeatMapDataPoint[] data;

    public HeatMapData(long size = 0)
    {
      data = new HeatMapDataPoint[size];
    }

    public IEnumerator<HeatMapDataPoint> GetEnumerator()
    {
      foreach (var item in data) yield return item;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public void AddPoint(HeatMapDataPoint p)
    {
      Array.Resize(ref data, data.Length + 1);
      data[data.Length - 1] = p;
    }
  }
}
