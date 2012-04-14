.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  //  public static void Main() {
  //    int a = 4;
  //    int b = 0;
  //    int[] c = new int[2];
  //
  //    try {
  //      a = a / b;
  //    } catch (DivideByZeroException e) {
  //        Console.WriteLine("DivideByZeroException detected.");
  //      try {
  //        Console.WriteLine(c[4]);
  //    } catch (IndexOutOfRangeException ex) {
  //        Console.WriteLine("IndexOutOfRangeException detected");
  //      }
  //    }
  //
  //    Console.WriteLine("I am a WriteLine method");
  //  }
  .method public hidebysig static void Main() cil managed
  {
    .locals init (int32 V_0, int32 V_1, int32[] V_2, [mscorlib]DivideByZeroException V_3, [mscorlib]IndexOutOfRangeException V_4)

    IL_0000: ldc.i4.4
    IL_0001: stloc.0
    IL_0002: ldc.i4.0
    IL_0003: stloc.1
    IL_0004: ldc.i4.2
    IL_0005: newarr int32
    IL_000a: stloc.2
    IL_000b: ldloc.0
    IL_000c: ldloc.1
    IL_000d: div
    IL_000e: stloc.0
    IL_000f: leave IL_0042
    IL_0014: stloc.3
    IL_0015: ldstr "DivideByZeroException detected."
    IL_001a: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine([mscorlib]System.String)
    IL_001f: ldloc.2
    IL_0020: ldc.i4.4
    IL_0021: ldelem.i4
    IL_0022: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine([mscorlib]System.Int32)
    IL_0027: leave IL_003d
    IL_002c: stloc.s V_4
    IL_002e: ldstr "IndexOutOfRangeException detected"
    IL_0033: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine([mscorlib]System.String)
    IL_0038: leave IL_003d
    IL_003d: leave IL_0042
    IL_0042: ldstr "I am a WriteLine method"
    IL_0047: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine([mscorlib]System.String)
    IL_004c: ret

    //.try IL_001f to IL_002c .handler Catch IL_002c to IL_003d
    //.try IL_000b to IL_0014 .handler Catch IL_0014 to IL_0042
  }
}