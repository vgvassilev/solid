/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System.Text;
using System.Collections.Generic;
using System.IO;
using System;

using DataMorphose.Model;

namespace DataMorphose.Plugins.ImportExport.Export
{
  public class CSVExporter
  {
    public void ExportDatabase(Database db, string dbFile) {
    string path = Path.GetDirectoryName(dbFile);
    StringBuilder sb = new StringBuilder();

    foreach (Table table in db.Tables) {
      string file = table.Name + ".txt";
      // Append the contents of the table to a file.
      File.WriteAllText(Path.Combine(path, file), table.ToString(), Encoding.UTF8);      
      sb.AppendLine(file);
      }
      // Append the file name to the database description file.
      File.WriteAllText(Path.Combine(path, dbFile), sb.ToString());
    }
  }
}