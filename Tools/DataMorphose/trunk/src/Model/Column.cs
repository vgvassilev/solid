// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using System;
using System.Collections.Generic;

namespace DataMorphose.Model
{
  /// <summary>
  /// Column.
  /// </summary>
  public class Column
  {
    private string name;
    public string Name {
      get { return this.name; }
      set { name = value; }
    }

    private object val = null;
    public object Value {
      get { return this.val; }
      set { val = value; }
    }

    public Column(string name) {
      this.name = name;
    }
  }
}

