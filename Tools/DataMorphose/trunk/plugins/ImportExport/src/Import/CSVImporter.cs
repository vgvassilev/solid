/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using DataMorphose.Model;

using System;
using System.IO;


namespace DataMorphose.Plugins.ImportExport.Import
{
  public class CSVImporter
  {
    private readonly bool firstRowIsHeader;

    public CSVImporter(bool firstRowIsHeader) {
      this.firstRowIsHeader = firstRowIsHeader;
    }

    /// <summary>
    /// Imports DB from a list of files.
    /// </summary>
    /// <returns>
    /// The DB from files.
    /// </returns>
    /// <param name='file'>
    /// File.
    /// </param>
    public Database importDBFromFiles(string file) {
      if (!File.Exists(file))
        return null;

      Database db = new Database(Path.GetFileNameWithoutExtension(file));
      string containingDir = Path.GetDirectoryName(file);
      foreach(string line in File.ReadLines(file)) {
        db.Tables.Add(importFromFile(Path.Combine(containingDir, line)));
      }

      return db;
    }

    /// <summary>
    /// Reads row by row from a file and puts the result into table
    /// </summary>
    /// <returns>
    /// The from file.
    /// </returns>
    /// <param name='file'>
    /// File.
    /// </param>
    public Table importFromFile(string file) {
      if (File.Exists(file)) {
        StreamReader reader = new StreamReader(file);
        // BufferedStream bs = new BufferedStream(new FileStream(file));
        // The first row contains the column names.
        CSVLexer lexer = new CSVLexer(reader.ReadLine());
        Table table = new Table(Path.GetFileNameWithoutExtension(file), lexer.GetSeparatorCount());
        string colValue;
        while ((colValue = lexer.Lex()) != null)
          // If the first line contains the header
          if (firstRowIsHeader)
            table.Columns.Add(new Column(colValue));
          else
            table.Columns.Add(new Column());

        string s;
        int i;
        while((s = reader.ReadLine()) != null) {
          lexer = new CSVLexer(s);
          i = 0;
          while((colValue = lexer.Lex()) != null) {
            table.Columns[i].Values.Add(colValue);
            i++;
          }
          if (table.Columns[i-1].Values.Count != table.Columns[0].Values.Count)
            throw new ArgumentOutOfRangeException();
        }
        return table;
      }
      return null;
    }

  }
}

