/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using SolidV.MVC;

namespace SolidIDE.Domains
{

  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
  public sealed class ShapeNameAttribute : Attribute
  {
    readonly string shapeName;
    public string ShapeName {
      get { return shapeName; }
    }

    public ShapeNameAttribute(string shapeName)
    {
      this.shapeName = shapeName;
    }
  }

}