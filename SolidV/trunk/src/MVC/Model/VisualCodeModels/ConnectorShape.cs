/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  /// <summary>
  /// Connector shape class, providing gluing functionality.
  /// </summary>
  /// 
  [Serializable]
  public class ConnectorShape : BinaryRelationShape 
  {
    private Glue fromGlue;
    public Glue FromGlue {
      get { return fromGlue; }
      set { fromGlue = value;}
    }

    private Glue toGlue;
    public Glue ToGlue {
      get { return toGlue; }
      set { toGlue = value;}
    }

    public ConnectorShape(Shape from, Shape to): this(from, null, to, null) { }

    public ConnectorShape(Shape from, Glue fromGlue, Shape to, Glue toGlue): base(from, to)
    {
      this.fromGlue = fromGlue; 
      this.toGlue = toGlue; 
    }
  }
}
