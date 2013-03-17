/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

namespace SolidV.MVC
{
  public class SelectionModel : BaseStateModel
  {
    private List<Shape> selected = new List<Shape>();
    public List<Shape> Selected {
      get { return selected; }
      set { selected = value; }
    }
    
    public SelectionModel() {
    }
  }
}

