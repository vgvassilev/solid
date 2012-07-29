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

    private List<Column> columns = new List<Column>();
    public List<Column> Columns {
      get { return this.columns; }
      set { columns = value; }
    }

    public Table(string name) {
      this.name = name;
    }
    
    public object[] GetRow(int rowNo) {
      object[] result = new object[Columns.Count];
      for(int i = 0; i < Columns.Count; i++) {
        result[i] = Columns[i].Values[rowNo];
      }
      return result;
    }
  }
}

