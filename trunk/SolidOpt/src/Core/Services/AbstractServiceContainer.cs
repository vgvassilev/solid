// OpenF
// Authors: 2004 A.Penev
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//

// 01.01.2003 Initial version. A.Penev (alexander_penev@yahoo.com)

using System;
using System.Collections;

namespace SolidOpt.Core.Services
{
	public abstract class AbstractServiceContainer : AbstractServiceProvider, IServiceContainer
	{
		public ArrayList services;
		public IServiceProvider parent;
		
		public AbstractServiceContainer()
		{
			// AbstractServiceContainer(null);
			this.services = new ArrayList();
			this.parent = null;
		}
		
		public AbstractServiceContainer(IServiceProvider parent)
		{
			this.services = new ArrayList();
			this.parent = parent;
		}
		
		public override IService GetService(Type serviceType)
		{
			IService found = base.GetService(serviceType);
			if (found != null) return found;
			
			foreach (IService service in services) {
				if (service is IServiceProvider)
					found = (service as IServiceProvider).GetService(serviceType);
				else 
					found = serviceType.IsInstanceOfType(service) ? service : null;
				if (found != null) return found;
			}

			if (parent != null) {
				found = parent.GetService(serviceType);
				if (found != null) return found;
			}
			
			return null;
		}

		public override IService[] GetServices(Type serviceType)
		{
			ArrayList foundServices = new ArrayList();
			
			foreach (IService service in services) {
				if (service is IServiceProvider)
					foundServices.AddRange((service as IServiceProvider).GetServices(serviceType));
				else 
					if (serviceType.IsInstanceOfType(service)) foundServices.Add(service);
			}
			
			if (parent != null)
				foundServices.AddRange(parent.GetServices(serviceType));
			
			return (IService[])foundServices.ToArray(typeof(IService));
		}
		
		public virtual bool AddService(IService service)
		{
			services.Add(service);
			return true;
		}
		
		public virtual bool RemoveService(IService service)
		{
			services.Remove(service);
			return true;
		}
		
	}
	
}
