/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;
using System.Text;

using DataMorphose.Model;

namespace DataMorphose.Model
{
  /// <summary>
  /// Class representing a raw of data from a table.
  /// </summary>
  public class Row
  {
    private object[] data;
    public object[] Data {
      get { return this.data; }
    }

    public string[] GetAsStringArray() {
      string[] result = new string[Data.Length];
      for (int i = 0, e = Data.Length; i < e; ++i)
        result[i] = Data[i].ToString();

      return result;
    }

    public Row(object[] data) {
      this.data = data;
    }

    public override string ToString ()
    {
      StringBuilder sb = new StringBuilder();
      foreach (object val in Data) {
        sb.Append(val.ToString());
        sb.Append('|');
      }

      return sb.ToString();
    }
  }
}

