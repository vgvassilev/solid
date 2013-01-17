/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Subsystems.Cache.Demo.Cache1
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Runtime.Serialization.Formatters.Binary;
  using SolidOpt.Services.Subsystems.Cache;
  
  /// <summary>
  /// Description of TestDefault.
  /// </summary>
  public class TestDefault: TestBase
  {
    public TestDefault(): base()
    {
      this.cache = new CacheManager<int,ResultClass>();
    }
  }
}
