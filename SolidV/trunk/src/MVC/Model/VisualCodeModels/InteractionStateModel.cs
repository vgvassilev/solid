/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

namespace SolidV.MVC
{
  public class InteractionStateModel : BaseStateModel
  {
    private List<Shape> interaction = new List<Shape>();
    public List<Shape> Interaction {
      get { return interaction; }
      set { interaction = value; }
    }
    
    public InteractionStateModel() {
    }
  }
}