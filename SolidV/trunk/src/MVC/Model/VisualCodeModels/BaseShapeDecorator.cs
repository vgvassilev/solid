// /*
//  * $Id: $
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using System;

namespace SolidV.MVC
{
  public class BaseShapeDecorator
  {
    private Shape decoratedShape;
    public Shape DecoratedShape {
      get { return decoratedShape; }
      set { decoratedShape = value; }
    }

    public BaseShapeDecorator() {
    }

    public BaseShapeDecorator(Shape decoratedShape) {
      this.DecoratedShape = decoratedShape;
    }
  }
}

