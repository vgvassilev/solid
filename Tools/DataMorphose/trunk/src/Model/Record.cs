// /*
//  * $Id: Row.cs 560 2012-04-30 19:38:29Z ppetrova $
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using System;
using System.Collections.Generic;

namespace DataMorphose.Model
{
  public class Row
  {
    private List<Column> columns = new List<Column>();
    public List<Column> Columns {
      get { return this.columns; }
      set { columns = value; }
    }
    
    public object[] GetColumnsValues() {
      object[] result = new object[Columns.Count];
      for(int i = 0; i < Columns.Count; i++) {
        result[i] = Columns[i].Value;
      }
      return result;
    }

    public Row() {
    }
  }
}

