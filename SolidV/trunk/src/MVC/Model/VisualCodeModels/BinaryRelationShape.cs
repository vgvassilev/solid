/*
 * $Id:
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  public class BinaryRelationShape : RelationShape
  {
    public Shape Form {
      get { return related[0]; }
      set { related[0] = value; }
    }

    public Shape To {
      get { return related[1]; }
      set { related[1] = value; }
    }

    public BinaryRelationShape(): base() {
      related.Add(null);
      related.Add(null);
    }

    public BinaryRelationShape(Shape from, Shape to): base() {
        related.Add(from);
        related.Add(to);
    }
  }
}
