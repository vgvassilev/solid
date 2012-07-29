// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using DataMorphose.Model;

using System;
using System.IO;


namespace DataMorphose.Import
{
  public class CSVImporter
  {
    class ColLexer {
      private string line = null;
      private int curLineIndex = 0;
      public ColLexer(string line) {
        this.line = line;
      }

      /// <summary>
      /// Reads symbol by symbol 
      /// </summary> 
      public string Lex() {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        while (curLineIndex < line.Length) {
          if (line[curLineIndex] != '|')
            sb.Append(line[curLineIndex]);
          else {
            curLineIndex++;
            return sb.ToString().Trim();
          }
          curLineIndex++;
        }
        return (sb.Length == 0) ? null : sb.ToString().Trim();
      }
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
        Table table = new Table(Path.GetFileNameWithoutExtension(file));
        StreamReader reader = new StreamReader(file);
        // BufferedStream bs = new BufferedStream(new FileStream(file));
        // The first row contains the column names.
        ColLexer lexer = new ColLexer(reader.ReadLine());
        string colValue;
        Record header = new Record();

        while((colValue = lexer.Lex()) != null)
          header.Fields.Add(new Field(colValue));
        table.Header = header;

        Record row = null;
        Field col = null;
        string s;
        while((s = reader.ReadLine()) != null) {
          lexer = new ColLexer(s);
          row = new Record();
          while((colValue = lexer.Lex()) != null) {
            col = new Field("");
            col.Value = colValue;
            row.Fields.Add(col);
          }
          table.Records.Add(row);
        }
        return table;
      }
      return null;
    }

  }
}

