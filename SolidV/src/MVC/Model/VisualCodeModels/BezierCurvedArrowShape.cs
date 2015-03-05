/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Drawing;
using Cairo;

namespace SolidV.MVC
{
  [Serializable]
  public class BezierCurvedArrowShape : CurvedArrowShape
  {
    private double connectorSize = 7;
    public double ConnectorSize {
      get { return connectorSize; }
      set { connectorSize = value; }
    }

    private double extraThickness = 3;
    public double ExtraThickness {
      get { return extraThickness; }
      set { extraThickness = value; }
    }

    private double lineWidth = 1;
    public double LineWidth {
      get { return lineWidth; }
      set { lineWidth = value; }
    }

    public BezierCurvedArrowShape(Shape from, Shape to): base(from, to) {}
    public BezierCurvedArrowShape(Shape from, Glue fromGlue, Shape to, Glue toGlue): base(from, fromGlue, to, toGlue) {}
  }
}