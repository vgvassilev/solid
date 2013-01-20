// RUN: "gmcs" /t:library /out:SimpleExpression.dll ../SimpleExpression.cs
public static class TestCase {
  public static void Main() {
    int a = 4;
    int b = 5;
    a = a + b;
  }
}
