// /*
//  * $Id$
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

    public Row() {
    }
  }
}

