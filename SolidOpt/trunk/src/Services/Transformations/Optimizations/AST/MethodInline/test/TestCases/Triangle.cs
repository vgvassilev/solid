// RUN: "gmcs" /t:library /out:Triangle.dll /r:System.dll /r:../../../../../../../../../../build/lib/SolidOpt.Services.Transformations.Optimizations.Annotations.dll ../Triangle.cs
// XFAIL:

using System;

using SolidOpt.Services.Transformations.Optimizations.AST.MethodInline;

class TestCase {
  public static void Main(string[] args) {
    Triangle();
    NonInlined();
    Triangle1();
  }

  [InlineableAttribute]
  public static void Triangle()
  {
    int a = 5;
    int b = 8;
    int c = 10;
    int P = a + b + c;
    Console.WriteLine("Perimeter is {0}", P);
    
    int p = P/2;
    double S = Math.Sqrt(p * (p-a) * (p-b) * (p-c));
    Console.WriteLine("Area is {0}", S);
  }

  public static void NonInlined() {
    string s = Console.ReadLine();
    Console.WriteLine("Not for inlining");
  }

  [InlineableAttribute]
  public static void Triangle1()
  {
    int a;
    int b;
    int c;
    int P;
//      Console.WriteLine("a");
    do {
      a = 5;
      b = 8;
      c = 10;
      P = a + b + c;
      Console.WriteLine("Perimeter is {0}", P);
      
      int p = P/2;
      double S = Math.Sqrt(p * (p-a) * (p-b) * (p-c));
      if (S == 0)
        Console.WriteLine("Area is Zero");
      else
        Console.WriteLine("Area is {0}", S);
    } while (a == 5);
    a++;
    Console.WriteLine(a);
    b++;
    Console.WriteLine(b);
  }
}
