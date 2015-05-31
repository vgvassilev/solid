/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

namespace SolidV.MVC
{

  public interface IGluesProvider
  {
    IEnumerable<Glue> GetGlues();
  }

}
