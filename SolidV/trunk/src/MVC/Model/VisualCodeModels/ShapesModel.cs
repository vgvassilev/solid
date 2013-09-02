/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;
using Cairo;

namespace SolidV.MVC
{
  public class ShapesModel : Model
  {
    private IAutoArrange autoArranger = null;

    private List<Shape> shapes = new List<Shape>();

    /// <summary>
    /// Gets or sets the shapes in the current model.
    /// </summary>
    /// <value>The shapes.</value>
    /// 
    public List<Shape> Shapes {
      get { return shapes; }
      set { shapes = value; }
    }

    public ShapesModel()
    {
      autoArranger = new ForceDirectedBlockAutoArrangment(this);
    }

    public ShapesModel(IAutoArrange customAutoArrange) {
      autoArranger = customAutoArrange;
    }

    /// <summary>
    /// Automatically arranges the shapes in the model.
    /// </summary>
    public void AutoArrange() {
      BeginUpdate();
      autoArranger.Arrange();
      EndUpdate();
    }
  }
}
