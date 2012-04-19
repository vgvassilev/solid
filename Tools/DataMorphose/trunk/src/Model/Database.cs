// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */

using System;
using System.Collections.Generic;

namespace DataMorphose.Model
{
  public class Database
  {
    private string name;
    public string Name {
      get { return this.name; }
      set { name = value; }
    }

    private List<Table> tables = new List<Table>();
    public List<Table> Tables {
      get { return this.tables; }
      set { tables = value; }
    }

    public Database(string name) {
      this.name = name;
    }
  }
}

