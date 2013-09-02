/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.MVC
{
  public class GlueRegion: Glue
  {
    public GlueRegion(GlueRegion glueRegion) : base(glueRegion) {}
    public GlueRegion(Rectangle rectangle) : base(rectangle) {}
  }
}
