/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;
using SolidV.Cairo;

namespace SolidV.MVC
{
  public class ArrowShape : ConnectorShape
  {
    private DrawArrowDelegate arrowKindHead = ArrowKinds.TriangleRoundArrow;
    public DrawArrowDelegate ArrowKindHead {
      get { return arrowKindHead; }
      set { arrowKindHead = value; }
    }

    private DrawArrowDelegate arrowKindTail = ArrowKinds.NoArrow;
    public DrawArrowDelegate ArrowKindTail {
      get { return arrowKindTail; }
      set { arrowKindTail = value; }
    }

    public ArrowShape(Shape from, Shape to): base(from, to) { }
    public ArrowShape(Shape from, Glue fromGlue, Shape to, Glue toGlue): base(from, fromGlue, to, toGlue) { }

  }
}
