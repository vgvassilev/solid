/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  public class ArrowShape : ConnectorShape
  {

    public enum ArrowKinds {
      Squared,
      Rounded,
      Triangled
    }

    private ArrowKinds arrowKindHead = ArrowKinds.Squared;
    public ArrowKinds ArrowKindHead {
      get { return arrowKindHead; }
      set { arrowKindHead = value; }
    }

    private ArrowKinds arrowKindTail = ArrowKinds.Rounded;
    public ArrowKinds ArrowKindTail {
      get { return arrowKindTail; }
      set { arrowKindTail = value; }
    }

    public ArrowShape(Shape from, Shape to): base(from, to) {
      
    }

 
  }
}
