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
using System.Collections.Generic;

namespace SolidOpt.Core.Services
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
		
	}
	
}
