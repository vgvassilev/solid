/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Core.Loader.Demo.Loadee
{
  // Disable warnings for never used and unreachable code, because this is test
  // 168 - variable declared but never used
  // 162 - unreachable code detected
  // 219 - variable assigned but the value never used
  
  #pragma warning disable 168, 162, 219
  
  /// <summary>
  /// This is a test program. It should be removed when appropraite tests are created. 
  /// </summary>
  //TODO: Replace this with test cases
  class Program
  {
    public static void Main(string[] args)
    {
//      Triangle();
//      Triangle1();
      Console.WriteLine("test");
      //InlineTest();
      Console.ReadKey(true);
    }

//    [MethodInliner.Inlineable]
    [SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Inlineable]
//    [MethodInliner.SideEffects(false)]
    public static double CalculateArea(double a, double b, double c)
    {
      if (a==0 || b==0 || c==0) return 0;
      
      double p = (a + b + c) / 2 ;
      return Math.Sqrt(p * (p-a) * (p-b) * (p-c));
    }
    
    [SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Inlineable]
    public static double CalculateArea1(double a, double b, double c)
    {
      try {
        if (a==0 || b==0 || c==0) return 0;
        
        double p = (a + b + c) / 2 ;
        return Math.Sqrt(p * (p-a) * (p-b) * (p-c));
      }
      catch {
        return -1;
      }
    }
    
//    [MethodInliner.InlineAttribute]
//    public static void OutParamAssign()
//    {
//      Dictionary<int, string> dict = new Dictionary<int, string>();
//      dict[0] = "abc";
//      string s;
//      if (!dict.TryGetValue(0, out s)){
//        s = "10";
//      }
//      Console.WriteLine(s);
//      
//      string s1;
//      dict.TryGetValue(0, out s1);
//      s1 = "11";
//      Console.WriteLine(s1);
//    }
    
//    [MethodInliner.Inlineable]
    [SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Inlineable]
//    [MethodInliner.SideEffects(true)]
    public static int Inlinee(int p, int q)
    {
      byte x = 5;
      int y = 2;
//      p=5;
//      q=6;
      if (x == 6) {
        Console.WriteLine(x);
        return p + q;
        
      }
      else {
        Console.WriteLine(p+q);
        return 10;
      }
////      byte v;
////      Console.WriteLine(x);
//      return 1;
      return p+q;
    }
    
//    [MethodInliner.Inlineable]
//    public static void Inlinee1(params byte [] ints)
//    {
//      int sum = 0;
//      for (int i = 0; i < ints.Length; i++) {
//        sum += ints[i];
//      }
//      return;
//    }
//    [MethodInliner.Inlineable]
//    public static void Inlinee1(params object [] ints)
//    {
//      for (int i = 0; i < ints.Length; i++) {
//        Console.WriteLine(ints[i]);
//      }
//      return;
//    }
  }
  #pragma warning restore 168, 162, 219
}
  class TestThis
  {
    private static int number = 0;
    
//    [MethodInliner.Inlineable]
    [SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Inlineable]
    public int Test()
    {
      Console.WriteLine("This is this");
      return 0;
    }
    
//    [MethodInliner.Inlineable]
    [SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Inlineable]
    public static int Inc ()
    {
      return number + 1;
    }
    
//    [MethodInliner.Inlineable]
    [SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.Inlineable]
    public static int Inc (int number)
    {
      return (TestThis.number + number + 1) * (number + TestThis.number + 1);
      //return (TestThis.number + number + 1) * (1 + number + TestThis.number);
    }
  }