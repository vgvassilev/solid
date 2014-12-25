/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  /// <summary>
  /// A decoration for a shape. A decorator is another visual wrapper over the standard shapes.
  /// One can decorate arbitrary shape, adding additional functionality, and the system will still
  /// treat it as a shape.
  /// </summary>
  /// 
  public abstract class ShapeDecorator
  {
    private Shape decoratedShape;
    public Shape DecoratedShape {
      get { return decoratedShape; }
      set { decoratedShape = value; }
    }

    public ShapeDecorator(Shape decoratedShape) {
      this.decoratedShape = decoratedShape;
    }
  }
}

