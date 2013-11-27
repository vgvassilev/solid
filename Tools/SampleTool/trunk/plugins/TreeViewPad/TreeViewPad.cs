/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using SampleTool;
using SolidOpt.Services;
using SolidV.Ide.Dock;

namespace TreeViewPad
{
  public class TreeViewPad : IPlugin, IPad
  {
    #region IPlugin implementation

    void IPlugin.Init(object context) {
      ISampleTool SampleTool = context as ISampleTool;
      DockFrame frame = SampleTool.GetMainWindow().DockFrame;

      DockItem treeViewDock = frame.AddItem("TreeViewDock");
      treeViewDock.Visible = true;
      treeViewDock.Behavior = DockItemBehavior.Normal;
      treeViewDock.Expand = true;
      treeViewDock.DrawFrame = true;
      treeViewDock.Label = "Files";
      //treeViewDock.Content = nb;
      treeViewDock.DefaultVisible = true;
      treeViewDock.Visible = true;
    }

    void IPlugin.UnInit(object context) {
      throw new NotImplementedException();
    }

    #endregion

    #region IPad implementation
    void IPad.Init(DockFrame frame) {
      throw new NotImplementedException();
    }
    #endregion
  }
}

