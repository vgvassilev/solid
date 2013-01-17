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
    public List<IService> services;
    public IServiceProvider parent;
    
    public AbstractServiceContainer()
    {
      this.services = new List<IService>();
      this.parent = null;
    }
    
    public AbstractServiceContainer(IServiceProvider parent)
    {
      this.services = new List<IService>();
      this.parent = parent;
    }
    
    public override List<Service> GetServices<Service>()
    {
      List<Service> foundServices = new List<Service>();
      
      foreach (object service in services) {
        if (service is IServiceProvider) {
          foundServices.AddRange((service as IServiceProvider).GetServices<Service>());
        }
        else {
          Service s = service as Service;
          if (s != default(Service)) {
            foundServices.Add(service as Service);
          }
        }
      }
      
      if (parent != null)
        foundServices.AddRange(parent.GetServices<Service>());
      
      return foundServices;
    }
    
    public override Service GetService<Service>()
    {      
      Service foundService = base.GetService<Service>();
      if (foundService != default(Service)) return foundService;
      
      foreach (object service in services) {
        if (service is IServiceProvider) {
          foundService = (service as IServiceProvider).GetService<Service>();
        }
        else {
          foundService = service as Service;
        }
        
        if (foundService != default(Service)) {
          return foundService;
        }
      }

      if (parent != null) {
        foundService = parent.GetService<Service>();
        if (foundService != default(Service)) return foundService;
      }
      
      return default(Service);
    }
    
    public override IService GetService(Type serviceType)
    {
      IService found = base.GetService(serviceType);
      if (found != null) return found;
      
      foreach (IService service in services) {
        if (service is IServiceProvider) {
          found = (service as IServiceProvider).GetService(serviceType);
          if (found != null) return found;
        }
        else {
          if (IsTypeProvideService(service.GetType(), serviceType)) return service;
          //found = serviceType.IsAssignableFrom(service.GetType()) ? service : null;
        }
      }

      if (parent != null) {
        return parent.GetService(serviceType);
      }
      
      return null;
    }
    
    public override List<IService> GetServices(Type serviceType)
    {
      List<IService> foundServices = new List<IService>();
      
      foreach (IService service in services) {
        if (service is IServiceProvider)
          foundServices.AddRange((service as IServiceProvider).GetServices(serviceType));
        else 
          if (IsTypeProvideService(service.GetType(), serviceType)) foundServices.Add(service);
      }
      
      if (parent != null)
        foundServices.AddRange(parent.GetServices(serviceType));
      
      return foundServices;
    }
    
    public override List<IService> GetServices()
    {
      List<IService> foundServices = new List<IService>();
      
      foreach (IService service in services) {
        if (service is IServiceProvider)
          foundServices.AddRange((service as IServiceProvider).GetServices());
        else 
          foundServices.Add(service);
      }
      
      if (parent != null)
        foundServices.AddRange(parent.GetServices());
      
      return foundServices;
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
