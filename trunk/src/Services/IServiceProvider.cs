/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services
{
	public interface IServiceProvider : IService
	{
		Service GetService<Service>() where Service : class;
		List<Service> GetServices<Service>() where Service : class;
		IService GetService(Type serviceType);
		IService[] GetServices(Type serviceType);
		IService[] GetServices();
	}
	
}

