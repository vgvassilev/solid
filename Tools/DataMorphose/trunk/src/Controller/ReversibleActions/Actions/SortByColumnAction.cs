/*
 * $Id$
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
      table.SetRows(cachedRows);
    }

    public override void Redo()
    {
      List<Row> tableAsRows = table.GetAsRows();
      cachedRows.AddRange(tableAsRows);
      tableAsRows.Sort(Compare);
      table.SetRows(tableAsRows);
    }
    #endregion

    private int Compare(Row x, Row y) {
      Debug.Assert(columnIndex > -1, "Column not found!");
      
      object xCol = x.Data[columnIndex];
      object yCol = y.Data[columnIndex];
      return xCol.ToString().CompareTo(yCol.ToString());
    }
  }
}

