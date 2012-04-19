// /*
//  * $Id: $
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */

using DataMorphose.Model;

using System;
using System.IO;

using NUnit.Framework;

namespace DataMorphose.Test
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
      return null;
    }

  }


  [TestFixture()]
  public class Test
  {
    private void importDBFromFiles() {
      //Database db = new Database("Test DB");
      string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..");
      filePath = Path.Combine(filePath, "..");
      filePath = Path.Combine(filePath, "..");
      filePath = Path.Combine(filePath, "test");
      filePath = Path.Combine(filePath, "DemoDB");
      filePath = Path.Combine(filePath, "Text");

      Table Users = importFromFile(Path.Combine(filePath, "Users.txt"));
      Table UsersDetails = importFromFile(Path.Combine(filePath, "UserDetails.txt"));
      Assert.IsTrue(Users.Name == "Users", "Table Users expected");
      Assert.IsTrue(UsersDetails.Name == "UserDetails", "Table UserDetails expected");
      int i;
    }

    private Table importFromFile(string file) {
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
            col.Values.Add(colValue);
            row.Columns.Add(col);
          }
          table.Rows.Add(row);
        }
        return table;
      }
      return null;
    }


    [Test()]
    public void TestCase() {
      importDBFromFiles();
    }
  }
}

