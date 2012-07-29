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
    private List<Record> cachedRows = new List<Record>();

    private int columnIndex;

    public SortByColumnAction(Table table, int columnIndex) {
      this.table = table;
      this.columnIndex = columnIndex;
    }

    #region implemented abstract members of DataMorphose.Action
    public override void Undo()
    {
      table.Records = cachedRows;
    }

    public override void Redo()
    {
      cachedRows.AddRange(table.Records);
      table.Records.Sort(Compare);
    }
    #endregion

    private int Compare(Record x, Record y) {
      Debug.Assert(columnIndex > -1, "Column not found!");
      
      Field xCol = x.Fields[columnIndex];
      Field yCol = y.Fields[columnIndex];
      return xCol.Value.ToString().CompareTo(yCol.Value.ToString());
    }
  }
}

