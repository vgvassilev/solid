/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  public class ModelViewer<C> : Viewer<C, Model>
  {
    public ModelViewer()
    {
    }
    
    public override void DrawItem(IView<C, Model> view, C context, object item)
    {
      foreach (Model model in ((Model)item).SubModels) {
        view.DrawItem(context, model);
      }
    }
  }
}
