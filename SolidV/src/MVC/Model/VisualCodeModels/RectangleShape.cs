/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  [Serializable]
  public class RectangleShape : Shape
  {
    public RectangleShape(Rectangle rectangle) : base(rectangle) {}
  }
}
