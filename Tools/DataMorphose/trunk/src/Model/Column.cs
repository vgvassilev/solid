// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using System;
using System.Collections.Generic;

namespace DataMorphose.Model
{
  public class Column
  {
    private string name;
    public string Name {
      get { return this.name; }
      set { name = value; }
    }

    private List<object> values = new List<object>();
    public List<object> Values {
      get { return this.values; }
      set { values = value; }
    }

    public Column(string name) {
      this.name = name;
    }
  }
}

