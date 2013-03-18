/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;

namespace SolidV.MVC
{
  public class ConnectorShape : BinaryRelationShape 
  {
    public ConnectorShape(): base() { }
    public ConnectorShape(Shape from, Shape to): base(from, to) { }
  }
}

