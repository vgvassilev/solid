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
    public HeatMapData HeatMap { get; set; } = new HeatMapData();

    public HeatMapShape(Rectangle rectangle) : base(rectangle) {}
  }
}
