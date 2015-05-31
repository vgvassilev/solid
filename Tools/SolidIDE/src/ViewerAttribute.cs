/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using SolidV.MVC;

namespace SolidIDE.Domains
{

  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
  public sealed class ViewerAttribute : Attribute
  {
    readonly Type viewerType;
    public Type ViewerType {
      get { return viewerType; }
    }

    public ViewerAttribute(Type viewerType)
    {
      this.viewerType = viewerType;
    }

  }

}