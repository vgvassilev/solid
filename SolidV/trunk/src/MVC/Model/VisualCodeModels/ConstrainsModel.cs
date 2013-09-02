/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

namespace SolidV.MVC
{
  
  public class ConstrainsModel : BaseStateModel
  {
    private List<IConstrain> constrains = new List<IConstrain>();
    public List<IConstrain> Constrains {
      get { return constrains; }
      set { constrains = value; }
    }
    
    public ConstrainsModel() {
    }
  }
}