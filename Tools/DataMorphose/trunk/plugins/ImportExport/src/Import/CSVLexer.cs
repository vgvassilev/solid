/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
namespace DataMorphose.Plugins.ImportExport.Import
{
  class CSVLexer {
    private string line = null;
    private int curLineIndex = 0;
    private char separator;

    public CSVLexer(string line) : this(line, '|') {
    }

    public CSVLexer(string line, char separator) {
      this.line = line;
      this.separator = separator;
    }

    /// <summary>
    /// Reads symbol by symbol
    /// </summary>
    public string Lex() {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      while (curLineIndex < line.Length) {
        if (line[curLineIndex] != separator)
          sb.Append(line[curLineIndex]);
        else {
          curLineIndex++;
          return sb.ToString().Trim();
        }
        curLineIndex++;
      }
      return (sb.Length == 0) ? null : sb.ToString().Trim();
    }

    public int GetSeparatorCount() {
      int result = 0;
      foreach (char c in line) {
        if (c == separator)
          result++;
      }
      return result;
    }
  }
}