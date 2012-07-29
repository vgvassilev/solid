// /*
//  * $Id: Column.cs 557 2012-04-29 20:34:57Z ppetrova $
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using System;
using System.Collections.Generic;

using DataMorphose.Model.Meta;

namespace DataMorphose.Model
{
  /// <summary>
  /// Column - the smallest unit of the database. All the elements of the column have the same
  /// relations and properties.
  /// </summary>
  public class Column
  {
    private MetaData meta;
    public MetaData Meta {
      get { return meta; }
      set { meta = value; }
    }

    private List<object> values = new List<object>();
    public List<object> Values {
      get { return this.values; }
      set { values = value; }
    }

    public Column(string name) {
      meta = new MetaData(this);
      meta.Name = name;
    }
  }
}

