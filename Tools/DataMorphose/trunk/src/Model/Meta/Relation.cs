/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

namespace DataMorphose.Model.Meta
{
  /// <summary>
  /// Description of Relation.
  /// </summary>
  public class Relation
  {
    private Column to = null;
    public Column To {
      get { return to; }
      set { to = value; }
    }
    
    public Relation(Column col)
    {
      to = col;
    }
  }
}
