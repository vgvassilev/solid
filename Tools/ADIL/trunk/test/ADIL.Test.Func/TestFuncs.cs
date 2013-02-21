// /*
//  * $Id:
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
//
using System;

namespace ADIL.Test.Func
{
    public static class TestFuncs
    {
        #region Simple functions

        public static float f1(float x) {
          return x*x + 1;
        }

        #endregion

        #region Sphere

        public static float unitSphere(float x, float y, float z) {
            return x*x + y*y + z*z - 1;
        }

        #endregion

        #region Vectors

        public static float vectorLengthSqr(float x, float y, float z) {
            return sqr(x) + sqr(y) + sqr(z);
        }

        public static float sqr(float x) {
            return x*x;
        }

        #endregion

        #region Internal functions

        public static double sinplusone(double x) {
            return Math.Sin(x) + 1;
        }


        public static double sinpluscos(double x) {
            return Math.Sin(x) + Math.Cos(x);
        }

        #endregion
    }
}
