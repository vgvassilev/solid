/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

using Cairo;

namespace SolidV.MVC
{
  [Serializable]
  public class BinaryRelationShape : RelationShape
  {
    public Shape From {
      get { return related[0]; }
      set { related[0] = value; }
    }

    public Shape To {
      get { return related[1]; }
      set { related[1] = value; }
    }

    public BinaryRelationShape(Shape from, Shape to) : 
      base(new Rectangle(from.Location.X, from.Location.Y, // FIXME: Use min and max
                         from.Location.X + to.Width, from.Location.Y + from.Height)) {
        related.Add(from);
        related.Add(to);
    }

  }
}
