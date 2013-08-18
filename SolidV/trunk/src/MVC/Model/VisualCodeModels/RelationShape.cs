/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

using Cairo;

namespace SolidV.MVC
{
  public class RelationShape : Shape
  {
    public List<Shape> related = new List<Shape>();
    
    public RelationShape(Rectangle rectangle) : base (rectangle) {
    }
  }
}
