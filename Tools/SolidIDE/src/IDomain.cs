/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using SolidOpt.Services;

namespace SolidIDE
{
  /// <summary>
  /// Exposes methods and events a domain has to implement in order to be compatible with
  /// the SolidIDE's Domain
  /// </summary>
  /// 
  public interface IDomain : IService
  {
    /// <summary>
    /// Gets the ...
    /// </summary>
    /// <returns>
    /// The ....
    /// </returns>
    /// 
    string GetDomain();
  }

}