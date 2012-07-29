/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace DataMorphose.Model.Meta
{
  /// <summary>
  /// Contains description about the data of the various elements.
  /// </summary>
  public class MetaData
  {
    private Column described;
    
    private string name;
    public string Name {
      get { return this.name; }
      set { name = value; }
    }
    
    private Relation relates;
    public Relation Relates {
      get { return relates; }
      set { relates = value; }
    }
    
    public MetaData(Column described)
    {
      this.described = described;
    }
  }
}
