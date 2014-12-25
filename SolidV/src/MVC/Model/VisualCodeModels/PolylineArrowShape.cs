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
  public class PolylineArrowShape : ConnectorShape
  {
    private List<Distance> points = new List<Distance>();
    public List<Distance> Points {
      get { return points; }
      set { points = value; }
    }

    public PolylineArrowShape(Shape from, Shape to): base(from, to) { }
    public PolylineArrowShape(Shape from, Glue fromGlue, Shape to, Glue toGlue): base(from, fromGlue, to, toGlue) { }
    public PolylineArrowShape(Shape from, Shape to, List<Distance> polyline): this(from, null, to, null) { }
    public PolylineArrowShape(Shape from, Glue fromGlue, Shape to, Glue toGlue, List<Distance> polyline): base(from, fromGlue, to, toGlue) {
      this.Points = polyline;
    }
  }
}
