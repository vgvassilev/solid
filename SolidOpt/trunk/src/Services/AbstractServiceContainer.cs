/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace SolidOpt.Services
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
		
		public override List<Service> GetServices<Service>()
		{
			List<Service> foundServices = new List<Service>();
			
			Console.WriteLine(">>>search: "+typeof(Service));
				
			foreach (object service in services) {
				Console.WriteLine(">>>test: "+service.ToString());
				
				if (service is IServiceProvider) {
					Console.WriteLine(">>>found: IServiceProvider");
					foundServices.AddRange((service as IServiceProvider).GetServices<Service>());
				}
				else {
					Service s = service as Service;
					if (s != default(Service)) {
						Console.WriteLine(">>>found: "+typeof(Service));
						foundServices.Add(service as Service);
					}
				}
				
//					if (serviceType.IsInstanceOfType(service)) foundServices.Add(service);
//					if (service.GetType().GetInterface(typeof(IService).FullName) != null)
//						foundServices.Add(service);
//					if (service.GetType().IsInstanceOfType(serviceType))
//						foundServices.Add(service);
			}
			
			if (parent != null)
				foundServices.AddRange(parent.GetServices<Service>());
			
			return foundServices;
		}
		
		public override Service GetService<Service>()
		{			
			Console.WriteLine(">>>search: "+typeof(Service));
			
			Service found = base.GetService<Service>();
			if (found != default(Service)) return found;
			
			foreach (object service in services) {
//	Type t1 = typeof(Service);
//	Type t2 = service.GetType();
//	
//	Console.WriteLine("-T1--------------");
//	Type[] typeArray2 = t1.GetInterfaces();
//	for (int j = 0; j < typeArray2.Length; j++) {
//		Console.WriteLine(typeArray2[j].FullName);
//	}
//	Console.WriteLine("-T2--------------");
//	typeArray2 = t2.GetInterfaces();
//	for (int j = 0; j < typeArray2.Length; j++) {
////		Console.WriteLine(typeArray2[j].Module.FullyQualifiedName);
//		Console.WriteLine(typeArray2[j].FullName);
//	}

				if (service is IServiceProvider) {
					Console.WriteLine(">>>found: IServiceProvider");
					found = (service as IServiceProvider).GetService<Service>();
				}
				else {
					found = service as Service;
				}
				
				if (found != default(Service)) {
					Console.WriteLine(">>>found: "+typeof(Service));
					return found;
				}
			}

			if (parent != null) {
				found = parent.GetService<Service>();
				if (found != default(Service)) return found;
			}
			
			return default(Service);
		}
		
		//TODO: Check implementation.
		public override IService GetService(Type serviceType)
		{
			IService found = base.GetService(serviceType);
			if (found != null) return found;
			
			foreach (IService service in services) {
				if (service is IServiceProvider)
					found = (service as IServiceProvider).GetService(serviceType);
				else {
//					found = serviceType.IsInstanceOfType(service) ? service : null;
//					Type t1 = service.GetType();
//					
//	Console.WriteLine("----------------");
//	if (serviceType.IsInterface)
//    {
//        Type[] typeArray2 = t1.GetInterfaces();
//        for (int j = 0; j < typeArray2.Length; j++)
//        {
//            Console.WriteLine(typeArray2[j]);
//        }
//    }
//					found = serviceType.IsInstanceOfType(service) ? service : null;
					found = serviceType.IsAssignableFrom(service.GetType()) ? service : null;
				}
				if (found != null) return found;
			}

			if (parent != null) {
				found = parent.GetService(serviceType);
				if (found != null) return found;
			}
			
			return null;
		}

		//TODO: Check implementation.
		public override IService[] GetServices(Type serviceType)
		{
			ArrayList foundServices = new ArrayList();
			
			foreach (IService service in services) {
				
				Console.WriteLine(">>>"+serviceType);
				
				if (service is IServiceProvider)
					foundServices.AddRange((service as IServiceProvider).GetServices(serviceType));
				else 
//					if (serviceType.IsInstanceOfType(service)) foundServices.Add(service);
//					if (service.GetType().GetInterface(typeof(IService).FullName) != null)
//						foundServices.Add(service);
//					if (service.GetType().IsInstanceOfType(serviceType))
					if (serviceType.IsAssignableFrom(service.GetType()))
						foundServices.Add(service);
			}
			
			if (parent != null)
				foundServices.AddRange(parent.GetServices(serviceType));
			
			return (IService[])foundServices.ToArray(typeof(IService));
		}
		
		//TODO: Check implementation.
		public override IService[] GetServices()
		{
			ArrayList foundServices = new ArrayList();
			
			foreach (IService service in services) {
				
				if (service is IServiceProvider)
					foundServices.AddRange((service as IServiceProvider).GetServices());
				else 
//					if (serviceType.IsInstanceOfType(service)) foundServices.Add(service);
//					if (service.GetType().GetInterface(typeof(IService).FullName) != null)
//						foundServices.Add(service);
					foundServices.Add(service);
			}
			
			if (parent != null)
				foundServices.AddRange(parent.GetServices());
			
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
