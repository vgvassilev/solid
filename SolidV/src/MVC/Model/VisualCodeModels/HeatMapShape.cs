/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

using Cairo;
using SolidV.Cairo;

namespace SolidV.MVC
{
  [Serializable]
  public class HeatMapShape : Shape
  {
    private HeatMapData heatMap = new HeatMapData(0);
    public HeatMapData HeatMap {
      get { return heatMap; }
      set { heatMap = value; }
    }

    private double alpha = 0.5;
    public double Alpha {
      get { return alpha; }
      set { alpha = value; }
    }

    private double blurFactor = 0.85;
    public double BlurFactor {
      get { return blurFactor; }
      set { blurFactor = value; }
    }

    private byte [] colorScheme = null;
    public byte[] ColorScheme {
      get {
        return colorScheme;
      }
      set {
        colorScheme = value;
      }
    }

    public HeatMapShape(Rectangle rectangle) : base(rectangle) { }

    public void AddPoint(HeatMapDataPoint p)
    {
      HeatMap.AddPoint(p);
    }

  }
}
