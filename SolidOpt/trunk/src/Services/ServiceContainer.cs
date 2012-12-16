/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace SolidOpt.Services
{
	public class ServiceContainer : AbstractServiceContainer
	{
        public ServiceContainer(): base() {}
        public ServiceContainer(IServiceProvider parent): base(parent) {}
	}
	
}
