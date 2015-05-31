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
  public class AddNewBinaryRelationCommand : AddNewShapeCommand
  {
    public AddNewBinaryRelationCommand() {}

    public AddNewBinaryRelationCommand(IDesigner designer, Shape newShape) : base (designer, newShape) {}

    public override void Do() {
      SelectionModel sm = (Designer.CurrentSheet as Sheet<Gdk.Event, Cairo.Context, SolidV.MVC.Model>).Model.GetSubModel<SelectionModel>();
      for (int i = 0; i < sm.Selected.Count - 1; i++) {
        BinaryRelationShape binaryRelation = (BinaryRelationShape)NewShape.DeepCopy();
        binaryRelation.From = sm.Selected[i];
        binaryRelation.To = sm.Selected[i+1];
        Designer.AddShapes(binaryRelation);
      }

    }
  }

}
