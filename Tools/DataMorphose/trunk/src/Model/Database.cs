 /*
  * $Id$
  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
  * For further details see the nearest License.txt
  */

using System;
using System.Collections.Generic;
using System.Text;

namespace DataMorphose.Model
{
  public class Database
  {
    private string name;
    public string Name {
      get { return this.name; }
      set { name = value; }
    }

    private List<Table> tables = new List<Table>();
    public List<Table> Tables {
      get { return this.tables; }
      set { tables = value; }
    }

    public Database(string name) {
      this.name = name;
    }

    public Table GetTable(string tableName) {
      foreach (Table t in Tables)
        if (t.Name == tableName)
          return t;
      return null;
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      foreach (Table table in Tables) {
        sb.AppendLine("---" + table.Name + "---");
        sb.AppendLine(table.ToString());
        sb.AppendLine();
      }

      return sb.ToString();
    }
  }
}

