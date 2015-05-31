/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

using SolidV.MVC;
using SolidIDE.Plugins.Designer;

namespace SolidIDE.Plugins.Toolbox
{
  public class AddNewShapeCommand : ICommand
  {
    private Shape newShape;
    public Shape NewShape {
      get { return newShape; }
      set { newShape = value; }
    }

    public AddNewShapeCommand() {}

    public AddNewShapeCommand(Shape newShape) {
      this.newShape = newShape;
    }

    public virtual void Do() {
      Toolbox.designer.AddShapes(NewShape.DeepCopy());
    }
  }

}
