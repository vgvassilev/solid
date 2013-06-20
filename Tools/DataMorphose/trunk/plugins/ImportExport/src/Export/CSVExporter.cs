/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System.Text;
using System.IO;

using DataMorphose.Model;

namespace DataMorphose.Plugins.ImportExport.Export
{
  public class CSVExporter
  {
    // TODO: Add documentation and test for that.
    public void ExportDatabase(Database db, string dbFile) {
      string path = Path.GetDirectoryName(dbFile);
      foreach (Table table in db.Tables) {
        // Append the contents of the table to a file.
        File.WriteAllText(Path.Combine(path, table.Name + ".csv"), table.ToString());
        // Append the file name to the database description file.
        File.AppendText(dbFile).WriteLine(path + table.Name + ".csv");
      }
    }
  }
}