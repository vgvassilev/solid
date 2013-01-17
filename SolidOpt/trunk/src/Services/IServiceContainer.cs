/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services
{
  public interface IServiceContainer : IServiceProvider
  {
    bool AddService(IService service);
    bool RemoveService(IService service);
  }
  
}
