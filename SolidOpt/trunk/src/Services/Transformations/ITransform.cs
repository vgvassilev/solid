/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using SolidOpt.Services;

namespace SolidOpt.Services.Transformations
{
  /// <summary>
  /// Description of ITransform.
  /// </summary>
  public interface ITransform<Source, Target> : IService
  {
    Target Transform(Source source);
  }
}
