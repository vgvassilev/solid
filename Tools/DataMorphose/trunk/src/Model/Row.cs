// /*
//  * $Id: Row.cs 560 2012-04-30 19:38:29Z ppetrova $
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using System;
using System.Collections.Generic;
using DataMorphose.Model;

namespace DataMorphose.Model
{
  /// <summary>
  /// Class representing a raw of data from a table.
  /// </summary>
  public class Row
  {
    private object[] data;
   
    public Row(object[] data) {
      this.data = data;
    }

    public object[] ToArray() {
      return data;
    }
  }
}

