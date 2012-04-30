/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System.Collections.Generic;

using DataMorphose.Model;

namespace DataMorphose
{
  public class SortByColumnAction : Action
  {
    private Table table = null;
    private List<Row> cachedRows = null;

    private Column column;

    public SortByColumnAction(Table table, Column column) {
      this.table = table;
      this.column = column;
    }

    #region implemented abstract members of DataMorphose.Action
    public override void Undo()
    {
      table.Rows = cachedRows;
    }

    public override void Redo()
    {
      table.Rows.Sort(Compare);
    }
    #endregion

    private int Compare(Row x, Row y) {
     // if (x.Columns.IndexOf(column))
      return 0; //if (Row)
    }
  }
}

