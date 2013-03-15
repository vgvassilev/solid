// /*
//  * $Id:
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
//
using System;
using Cairo;

namespace SolidV.Cairo
{
    public class ExtendedContext: Context
    {
        public ExtendedContext(IntPtr state): base(state) {}
        public ExtendedContext(Surface surface): base(surface) {}

        private void DrawArrowHead() {
            //TODO: Use property for current arrow head type
            RelMoveTo(1, 0);
            RelLineTo(-1, 0);
            RelLineTo(0, 1);
        }

        private void DrawArrowTail() {
            //TODO: Use property for current arrow tail type
            //RelMoveTo(1, 0);
            //RelLineTo(-1, 0);
            //RelLineTo(0, 1);
        }

        public void ArrowLineTo(double x, double y) {
            DrawArrowTail();
            LineTo(x, y);
            DrawArrowHead();
        }

        public void ArrowLineTo(PointD p) {
            DrawArrowTail();
            LineTo(p);
            DrawArrowHead();
        }
        
        public void ArrowCurveTo(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            DrawArrowTail();
            CurveTo(x1, y1, x2, y2, x3, y3);
            DrawArrowHead();
        }

        public void ArrowCurveTo(PointD p1, PointD p2, PointD p3) {
            DrawArrowTail();
            CurveTo(p1, p2, p3);
            DrawArrowHead();
        }

        public void RelArrowLineTo(double x, double y) {
            DrawArrowTail();
            RelLineTo(x, y);
            DrawArrowHead();
        }
        
        public void RelArrowLineTo(Distance d) {
            DrawArrowTail();
            RelLineTo(d);
            DrawArrowHead();
        }
        
        public void RelArrowCurveTo(double dx1, double dy1, double dx2, double dy2, double dx3, double dy3)
        {
            DrawArrowTail();
            RelCurveTo(dx1, dy1, dx2, dy2, dx3, dy3);
            DrawArrowHead();
        }
        
        public void RelArrowCurveTo(Distance d1, Distance d2, Distance d3) {
            DrawArrowTail();
            RelCurveTo(d1, d2, d3);
            DrawArrowHead();
        }
    }
}

