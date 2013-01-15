// RUN: "mcs" /t:library /out:SimpleInline.dll /r:/Users/vvassilev/workspace/my/solid/SolidOpt/build/lib/SolidOpt.Services.Optimizations.Annotations.dll
public static class TestCase {
  public static void Main() {
    InlineTest();
  }

  [SolidOpt.Services.Transformations.Optimizations.AST.MethodInline.Inlineable]
  public static void InlineTest()
  {
    double a = 5;
    double result;
    result = a + 10;
  }
}
