/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System.Text;

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
      if (curLineIndex > line.Length)
        return null;

      StringBuilder sb = new StringBuilder();
      while (curLineIndex < line.Length) {
        if (line[curLineIndex] != separator) {
          sb.Append(line[curLineIndex]);
        }
        else {
          curLineIndex++;
          return sb.ToString();
        }
        curLineIndex++;
      }
      if (sb.Length == 0 && line[curLineIndex - 1] == separator && curLineIndex == line.Length) {
        curLineIndex++;
        return "";
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