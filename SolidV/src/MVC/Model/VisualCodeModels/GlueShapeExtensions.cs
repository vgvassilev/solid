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
  public static class GlueShapeExtensions
  {
    public static IEnumerable<IGlue> Glues(this Shape shape) {
      yield return new Glue(new Rectangle(0,0,0,0));
    }
  }
}
