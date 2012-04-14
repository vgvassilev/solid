.assembly extern System.Exception {}
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  //  public static void Main() {
  //    MainClass mc = new MainClass();
  //    mc.instancemethod();
  //
  //    int a = 4;
  //    int b = 0;
  //
  //    try {
  //      a = a / b;
  //    } catch(DivideByZeroException e) {
  //      Console.WriteLine("Division by zero detected.");
  //    } catch(ArgumentOutOfRangeException e) {
  //      Console.WriteLine("Arg out of range detected.");
  //    } catch(OverflowException e) {
  //      Console.WriteLine("Overflow detected.");
  //    }
  //    Console.WriteLine("I am a WriteLine method");
  //  }
  .method public hidebysig static void Main() cil managed
  {
    .locals init (int32 V_0, int32 V_1, [mscorlib]DivideByZeroException V_2, [mscorlib]ArgumentOutOfRangeException V_3, [mscorlib]OverflowException V_4)
    IL_0000: ldc.i4.4
    IL_0001: stloc.0
    IL_0002: ldc.i4.0
    IL_0003: stloc.1
    IL_0004: ldloc.0
    IL_0005: ldloc.1
    IL_0006: div
    IL_0007: stloc.0
    IL_0008: leave IL_003e
    IL_000d: stloc.2
    IL_000e: ldstr "Division by zero detected."
    IL_0013: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine(System.String)
    IL_0018: leave IL_003e
    IL_001d: stloc.3
    IL_001e: ldstr "Arg out of range detected."
    IL_0023: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine(System.String)
    IL_0028: leave IL_003e
    IL_002d: stloc.s V_4
    IL_002f: ldstr "Overflow detected."
    IL_0034: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine(System.String)
    IL_0039: leave IL_003e
    IL_003e: ldstr "I am a WriteLine method"
    IL_0043: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine(System.String)
    IL_0048: ret

    //.try IL_0004 to IL_000d .handler Catch IL_000d to IL_001d
    //.try IL_0004 to IL_000d .handler Catch IL_001d to IL_002d
    //.try IL_0004 to IL_000d .handler Catch IL_002d to IL_003e

  }
}
