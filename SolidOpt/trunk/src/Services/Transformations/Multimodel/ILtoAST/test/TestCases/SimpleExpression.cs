// RUN: "@CSC@" /t:library @TEST_CASE@
// XFAIL:
public static class TestCase {
  public static void Main() {
    int a = 4;
    int b = 5;
    a = a + b;
  }
}
