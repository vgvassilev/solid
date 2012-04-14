.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  //  public static void Main() {
  //    int a = 4;
  //    int b = 0;
  //
  //    try {
  //      a = a / b;
  //    } catch(Exception e) {
  //      Console.WriteLine("Division by zero detected.");
  //    }
  //  }
  .method public hidebysig static bool Main() cil managed
  {
    IL_0000: ldc.i4.4
    IL_0001: stloc.0
    IL_0002: ldc.i4.0
    IL_0003: stloc.1
    IL_0004: ldloc.0
    IL_0005: ldloc.1
    IL_0006: div
    IL_0007: stloc.0
    IL_0008: leave IL_001d
    IL_000d: stloc.2
    IL_000e: ldstr "Division by zero detected."
    IL_0013: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine(System.String)
    IL_0018: leave IL_001d
    IL_001d: ret
  }
}