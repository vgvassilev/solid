/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;
using System.Linq;


using SolidV.MVC;
using SolidIDE.Plugins.Designer;

namespace SolidIDE.Plugins.Toolbox
{
  public class AddNewConnectorShapeCommand : AddNewShapeCommand
  {
    public AddNewConnectorShapeCommand() {}

    public AddNewConnectorShapeCommand(Shape newShape) : base (newShape) {}

    public override void Do() {
      SelectionModel sm = (Toolbox.designer.CurrentSheet as Sheet<Gdk.Event, Cairo.Context, SolidV.MVC.Model>)
        .Model.GetSubModel<SelectionModel>();
      for (int i = 0; i < sm.Selected.Count - 1; i++) {
        ConnectorShape connector = (ConnectorShape)NewShape.DeepCopy();
        connector.From = sm.Selected[i];
        connector.FromGlue = (Glue)connector.From.Glues().FirstOrDefault();
        connector.To = sm.Selected[i+1];
        connector.ToGlue = (Glue)connector.To.Glues().FirstOrDefault();
        Toolbox.designer.AddShapes(connector);
      }

    }
  }

}
