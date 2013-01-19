// RUN: "gmcs" /t:library /out:SimpleInline.dll /r:System.dll /r:/Users/vvassilev/workspace/my/solid/SolidOpt/build/lib/SolidOpt.Services.Transformations.Optimizations.Annotations.dll ../SimpleInline.cs
// XFAIL:

using System;

using SolidOpt.Services.Transformations.Optimizations.AST.MethodInline;

class TestCase {
  [InlineableAttribute]
  public static void Main(string[] args) {
    InlineTest();
  }

  [InlineableAttribute]
  public static int InlineTest()
  {
    int a = 5;
    int result;
    result = a + 10;
    return result;
  }
}
