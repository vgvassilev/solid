// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */

using System;
using System.Collections.Generic;

namespace DataMorphose.Model
{
  public class Table
  {
    private string name;
    public string Name {
      get { return this.name; }
      set { name = value; }
    }

    private Record header = new Record();
    public Record Header {
      get { return this.header; }
      set { header = value; }
    }

    private List<Record> records = new List<Record>();
    public List<Record> Records {
      get { return this.records; }
      set { records = value; }
    }

    public Table(string name) {
      this.name = name;
    }
  }
}

