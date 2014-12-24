// RUN: "@CSC@" /t:library /r:System.dll /r:@CMAKE_LIBRARY_OUTPUT_DIR@/SolidOpt.Services.Transformations.Optimizations.Annotations.dll @TEST_CASE@
// XFAIL:

using System;

using SolidOpt.Services.Transformations.Optimizations.AST.MethodInline;

class TestCase {
  public static void Main() {
    Inline();
  }

  [Inlineable]
  public static void Inline()
  {
    double a = 5;
    double result;
    result = CalculateDiag(a);
    Console.WriteLine(a);
  }

  [Inlineable]
  public static double CalculateDiag(double a)
  {
    return Math.Sqrt(2) * a;
  }  
}