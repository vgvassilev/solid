/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.MVC
{
  public class Glue: Shape, IGlue
  {
    public Glue(Glue glue) : base(glue) {}
    public Glue(Rectangle rectangle) : base(rectangle) {}
  }
}

