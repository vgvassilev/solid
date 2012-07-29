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
  public class Record
  {
    private List<Field> fields = new List<Field>();
    public List<Field> Fields {
      get { return this.fields; }
      set { fields = value; }
    }
    
    public object[] GetColumnsValues() {
      object[] result = new object[Fields.Count];
      for(int i = 0; i < Fields.Count; i++) {
        result[i] = Fields[i].Value;
      }
      return result;
    }

    public Record() {
    }
  }
}

