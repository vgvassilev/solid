/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Drawing;
using Cairo;

namespace SolidV.MVC
{
  [Serializable]
  public class BezierCurvedArrowShape : ConnectorShape
  {
    public BezierCurvedArrowShape(Shape from, Shape to): base(from, to) {}
    public BezierCurvedArrowShape(Shape from, Glue fromGlue, Shape to, Glue toGlue): base(from, fromGlue, to, toGlue) {}
  }
}