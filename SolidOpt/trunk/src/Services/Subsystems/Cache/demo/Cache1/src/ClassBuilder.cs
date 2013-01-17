/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services.Subsystems.Cache.Demo.Cache1
{
  /// <summary>
  /// Description of ClassBuilder.
  /// </summary>
  public class ClassBuilder
  {
    public ClassBuilder()
    {
    }
    
    public static ResultClass BuildResult(int i)
    {
      return new ResultClass("result" + i);
    }
    
    public static ResultClass BuildResult(long i)
    {
      return new ResultClass("result" + i);
    }
  }
}
