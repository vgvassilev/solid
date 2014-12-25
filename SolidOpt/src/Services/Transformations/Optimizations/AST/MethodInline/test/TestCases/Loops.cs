// RUN: "@CSC@" /t:library /r:System.dll /r:@CMAKE_LIBRARY_OUTPUT_DIR@/SolidOpt.Services.Transformations.Optimizations.Annotations.dll @TEST_CASE@
// XFAIL:

using System;

using SolidOpt.Services.Transformations.Optimizations.AST.MethodInline;

class TestCase {
  public static void Main(string[] args) {
    PostTestLoop();
    PreTestLoop();
  }

  [Inlineable]
  public static void PostTestLoop()
  {
    int a;
    int b;
    int s = 0;
    do {
      a = 5;
      b = 8;
      s = a + b;
      s = b;
    } while (a == 100);
  }
  
  [Inlineable]
  public static void PreTestLoop()
  {
    int a = (int)Math.Sqrt(150*150);
    double s = 0;
    ;
    ;
    ;
    while (a >= 100) {
      a ++;
      int b = 8;
      s = Math.Sqrt(a + b);
      b = ++a;
      b = a++;
    } 
    Console.WriteLine(s);
  }

  public static void PreTestLoop1()
  {
    while (new Random().Next() > 0.5) {
      int b = 8;
      b --;
      
      Console.WriteLine(b);
    } 
  }
    
}
