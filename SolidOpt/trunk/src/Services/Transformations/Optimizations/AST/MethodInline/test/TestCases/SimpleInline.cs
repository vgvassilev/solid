// RUN: "@CSC@" /t:library /r:System.dll /r:@CMAKE_LIBRARY_OUTPUT_DIR@/SolidOpt.Services.Transformations.Optimizations.Annotations.dll @TEST_CASE@
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
