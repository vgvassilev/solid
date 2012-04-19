// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */

using System;
using System.Collections.Generic;

namespace DataMorphose.Model
{
  public class Table
  {
    private string name;
    public string Name {
      get { return this.name; }
      set { name = value; }
    }

    private Row header = new Row();
    public Row Header {
      get { return this.header; }
      set { header = value; }
    }

    private List<Row> rows = new List<Row>();
    public List<Row> Rows {
      get { return this.rows; }
      set { rows = value; }
    }

    public Table(string name) {
      this.name = name;
    }
  }
}

