// RUN: "gmcs" /t:library /out:NestedInline.dll /r:System.dll /r:../../../../../../../../../../build/lib/SolidOpt.Services.Transformations.Optimizations.Annotations.dll ../NestedInline.cs
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