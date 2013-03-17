/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  public class ArrowShape : BinaryRelationShape
  {
    public ArrowShape(): base() { }

    public ArrowShape(Shape from, Shape to): base(from, to) { }

  }
}
