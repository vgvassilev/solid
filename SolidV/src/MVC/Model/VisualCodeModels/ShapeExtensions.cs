/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Runtime.Serialization;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;

namespace SolidV.MVC
{
  public static partial class ShapeExtensions
  {
    public static Shape WithParent(this Shape shape, Shape parent) {
      shape.Parent = parent;
      return shape;
    }
    
  }
}

