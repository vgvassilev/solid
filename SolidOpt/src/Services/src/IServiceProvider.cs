/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services
{
  /// <summary>
  /// Service provider can provide more than one service.
  /// </summary>
  /// <description>
  /// The main difference between the IServiceProvider and IServiceContainer is the
  /// IServiceProvider can provide number of services by itself, thus it does not
  /// contain them.
  /// </description>
  public interface IServiceProvider : IService
  {
    /// <summary>
    /// Gets а service for a given type (generic).
    /// </summary>
    /// <returns>The service.</returns>
    /// <typeparam name="Service">The 1st type parameter.</typeparam>
    /// 
    Service GetService<Service>() where Service : class;

    /// <summary>
    /// Gets all services of a given type (generic).
    /// </summary>
    /// <returns>The services.</returns>
    /// <typeparam name="Service">The 1st type parameter.</typeparam>
    /// 
    List<Service> GetServices<Service>() where Service : class;

    /// <summary>
    /// Gets a service for a given type (non-generic).
    /// </summary>
    /// <returns>The service.</returns>
    /// <param name="serviceType">Service type.</param>
    /// 
    IService GetService(Type serviceType);

    /// <summary>
    /// Gets all services of a given type (non-generic).
    /// </summary>
    /// <returns>The services.</returns>
    /// <param name="serviceType">Service type.</param>
    /// 
    List<IService> GetServices(Type serviceType);

    /// <summary>
    /// Gets all services.
    /// </summary>
    /// <returns>The services.</returns>
    /// 
    List<IService> GetServices();
  }
}
