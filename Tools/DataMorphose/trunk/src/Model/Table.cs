/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DataMorphose.Model
{
  public class Table
  {
    private string name;
    public string Name {
      get { return this.name; }
      set { name = value; }
    }

    private List<Column> columns;
    public List<Column> Columns {
      get { return this.columns; }
      set { columns = value; }
    }

    public Table(string name, int columnCount) {
      this.columns = new List<Column>(columnCount);
      this.name = name;
    }

    public void SetRows(List<Row> rows) {
      for (int i = 0; i < rows.Count; i++) {
        Debug.Assert(rows[i].Data.Length == Columns.Count, "Different column sizes?");
        if (rows[i].Data.Length != Columns.Count)
          Console.WriteLine("Dif sizes");
        for (int j = 0; j < Columns.Count; j++)
          Columns[j].Values[i] = rows[i].Data[j];
      }
    }

    /// <summary>
    /// Represents the table as sequence of rows.
    /// </summary>
    /// <returns>
    /// The list of rows.
    /// </returns>
    public List<Row> GetAsRows() {
      List<Row> result = new List<Row>();
      int colCount = Columns[0].Values.Count;
      for (int i = 0; i < colCount; i++)
        result.Add(GetRow(i));
      return result;
    }

    public Row GetRow(int rowNo) {
      object[] result = new object[Columns.Count];
      for(int i = 0; i < Columns.Count; i++) {
        result[i] = Columns[i].Values[rowNo];
      }
      return new Row(result);
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder();

      for (int i = 0; i < Columns.Count; i++) {
        sb.Append(Columns[i].Meta.Name);
        sb.Append('|');
      }
      sb.AppendLine();


      for (int i = 0; i < Columns[0].Values.Count; i++) {
        for (int j = 0; j < Columns.Count; j++) {
          sb.Append(Columns[j].Values[i].ToString());
          sb.Append('|');
        }
        sb.AppendLine();
      }

      return sb.ToString();
    }
  }
}

