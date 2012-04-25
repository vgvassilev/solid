// /*
//  * $Id: $
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

    public Table importFromFile(string file) {
      if (File.Exists(file)) {
        Table table = new Table(Path.GetFileNameWithoutExtension(file));
        StreamReader reader = new StreamReader(file);
        // BufferedStream bs = new BufferedStream(new FileStream(file));
        // The first row contains the column names.
        ColLexer lexer = new ColLexer(reader.ReadLine());
        string colValue;
        Row header = new Row();

        while((colValue = lexer.Lex()) != null)
          header.Columns.Add(new Column(colValue));
        table.Header = header;

        Row row = null;
        Column col = null;
        string s;
        while((s = reader.ReadLine()) != null) {
          lexer = new ColLexer(s);
          row = new Row();
          while((colValue = lexer.Lex()) != null) {
            col = new Column("");
            col.Value = colValue;
            row.Columns.Add(col);
          }
          table.Rows.Add(row);
        }
        return table;
      }
      return null;
    }

  }

}

