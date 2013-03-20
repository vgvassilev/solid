/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

using Cairo;

namespace SolidV.MVC
{
  public class CurvedArrowShape : PolylineArrowShape
  {
    public CurvedArrowShape(): base() { }
    public CurvedArrowShape(Shape from, Shape to): base(from, to) { }
    public CurvedArrowShape(Shape from, Shape to, List<Distance> controlPoints): base(from, to, controlPoints) { }
  }
}
