/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services
{
  /// <summary>
  /// Provides and contains services.
  /// </summary>
  /// <description>
  /// A service container could define number of services like the ServiceProvider.
  /// It could also register other services provided by somebody else (either a 
  /// service provider or service container).
  /// </description>
  public interface IServiceContainer : IServiceProvider
  {
    /// <summary>
    /// Adds a service.
    /// </summary>
    /// <returns><c>true</c>, if service was added, <c>false</c> otherwise.</returns>
    /// <param name="service">Service.</param>
    ///
    bool AddService(IService service);

    /// <summary>
    /// Removes a service.
    /// </summary>
    /// <returns><c>true</c>, if service was removed, <c>false</c> otherwise.</returns>
    /// <param name="service">Service.</param>
    /// 
    bool RemoveService(IService service);
  }
  
}
