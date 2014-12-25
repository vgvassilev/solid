/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using DataMorphose.Actions;
using DataMorphose.Model;

namespace DataMorphose.Transform
{
  /// <summary>
  /// Transformation, which splits given column into 2 or more columns.
  /// </summary>
  public class SplitColumn : DataMorphose.Actions.Action
  {
    private Table table;
    private Column column;
    private char delimiter;

    //FIXME: For now we pass in the table, which is wrong. The table has to be accessible through
    // the meta data of the column.
    public SplitColumn(Table table, Column column, char delimiter)
    {
      this.table = table;
      this.column = column;
      this.delimiter = delimiter;
    }

    public override void Redo()
    {
      // FIXME: we have to implement the cases where we have to split non string values
      //if (column.Meta.Type != typeof(String))
      //  throw new NotImplementedException("Implement the cases for non string values");

      Column splitCol = new Column(column.Meta.Name + "_split");
      int insertIndex = table.Columns.IndexOf(column);
      table.Columns.Insert(insertIndex, splitCol);
      int lastIndex = 0;
      string s;
      for (int i = 0; i < column.Values.Count; i++) {
        s = column.Values[i] as string;
        lastIndex = s.LastIndexOf(delimiter);
        splitCol.Values.Add(s.Substring(lastIndex + 1)); // excluding the delimiter itself
        s = s.Substring(0, lastIndex); // excluding the delimiter itself
      }
    }

    public override void Undo()
    {
      throw new NotImplementedException ();
    }

  }
}

