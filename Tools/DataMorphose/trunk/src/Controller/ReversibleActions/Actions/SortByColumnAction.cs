/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using DataMorphose.Model;

using System.Diagnostics;
using System.Collections.Generic;

namespace DataMorphose.Actions
{
  public class SortByColumnAction : Action
  {
    private Table table = null;
    private List<Row> cachedRows = new List<Row>();

    private int columnIndex;

    public SortByColumnAction(Table table, int columnIndex) {
      this.table = table;
      this.columnIndex = columnIndex;
    }

    #region implemented abstract members of DataMorphose.Action
    public override void Undo()
    {
//      table.Columns = cachedRows;
    }

    public override void Redo()
    {
//      cachedRows.AddRange(table.Columns);
//      table.Columns.Sort(Compare);
    }
    #endregion

    private int Compare(Row x, Row y) {
      Debug.Assert(columnIndex > -1, "Column not found!");
      
//      Column xCol = x.Columns[columnIndex];
//      Column yCol = y.Columns[columnIndex];
      return 0; //xCol.Value.ToString().CompareTo(yCol.Value.ToString());
    }
  }
}

