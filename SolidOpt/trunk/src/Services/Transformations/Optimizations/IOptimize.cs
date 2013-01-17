/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Transformations.Optimizations
{
  /// <summary>
  /// Description of IOptimize.
  /// </summary>
  public interface IOptimize<Target> : ITransform<Target, Target>
  {
    Target Optimize(Target source);
  }
}
