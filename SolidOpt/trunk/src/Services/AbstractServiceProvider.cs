/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services
{
	public abstract class AbstractServiceProvider : AbstractService, IServiceProvider
	{
		public virtual Service GetService<Service>() where Service: class
		{
			return this as Service;
		}
		
		public virtual List<Service> GetServices<Service>() where Service: class
		{
			Service found = GetService<Service>();
			List<Service> result = new List<Service>();
			if (found != default(Service))
				result.Add(found);
			return result;
		}
		
		//TODO: Check implementation.
		public virtual IService GetService(Type serviceType) {
			return serviceType.IsInstanceOfType(this) ? this : null;
		}
		
		//TODO: Check implementation.
		public virtual IService[] GetServices(Type serviceType) {
			IService found = GetService(serviceType);
			return (found != null) ? new IService[1] {found} : new IService[0];
		}
		
		//TODO: Check implementation.
		public virtual IService[] GetServices() {
			return new IService[1] {this};
		}
		
	}
	
}
