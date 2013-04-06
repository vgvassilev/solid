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
  /// Default implementation for the IServiceProvider's members.
  /// </summary>
  public class ServiceProvider : Service, IServiceProvider
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
    
    public virtual IService GetService(Type serviceType) {
      return IsTypeProvideService(this.GetType(), serviceType) ? this : null;
    }
    
    public virtual List<IService> GetServices(Type serviceType) {
      List<IService> result = new List<IService>(1);
      IService found = GetService(serviceType);
      if (found != null) result.Add(found);
      return result;
    }
    
    public virtual List<IService> GetServices() {
      List<IService> result = new List<IService>(1);
      result.Add(this);
      return result;
    }
    
    protected static bool IsTypeProvideService(Type type, Type serviceTypeOrOpenGeneric) {
      if (serviceTypeOrOpenGeneric.IsAssignableFrom(type)) return true;
      
      while (type != null && type != typeof(object)) {
        if ((serviceTypeOrOpenGeneric.ContainsGenericParameters && type.IsGenericType && serviceTypeOrOpenGeneric.GetGenericTypeDefinition() == type.GetGenericTypeDefinition()) ||
            (serviceTypeOrOpenGeneric == type))
          return true;
        
        foreach (Type inter in type.GetInterfaces())
          if (IsTypeProvideService(inter, serviceTypeOrOpenGeneric)) return true;
        
        type = type.BaseType;
      }
      
      return false;
    }
    
  }  
}
