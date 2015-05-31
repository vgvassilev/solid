/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using SolidV.MVC;

namespace SolidIDE.Domains
{
  [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
  public sealed class DomainAttribute : Attribute
  {
    readonly string domainName;
    public string DomainName {
      get { return domainName; }
    }
    
    public DomainAttribute(string domainName)
    {
      this.domainName = domainName;
    }
  }

}