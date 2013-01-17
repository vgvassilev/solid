/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Transformations.Multimodel
{
  /// <summary>
  /// Description of ICompile.
  /// </summary>
  public interface ICompile<Source, Target> : ITransform<Source, Target>
  {
    Target Compile(Source source);
  }
}
