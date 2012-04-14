.assembly extern mscorlib { }
.assembly TestCase { }


.class public auto ansi beforefieldinit TestCase
 extends [mscorlib]System.Object
{

  .field private static int32 exc_not_thrown
  .field private static int32 flt_caught

  //  C# does not support filter exception handlers
  .method public static specialname void .cctor ()
  {
    IL_0000: ldc.i4.0
    IL_0001: stsfld [mscorlib]System.Int32 TestCase::exc_not_thrown
    IL_0006: ldc.i4.0
    IL_0007: stsfld [mscorlib]System.Int32 TestCase::flt_caught
    IL_000c: ret
  }

  .method public static int32 Main ()
  {
    IL_0000: ldc.i4.0
    IL_0001: ldc.i4.0
    IL_0002: div
    IL_0003: ldc.i4.1
    IL_0004: stsfld [mscorlib]System.Int32 TestCase::exc_not_thrown
    IL_0009: leave IL_001d
    IL_000e: pop
    IL_000f: ldc.i4.1
    IL_0010: endfilter
    IL_0012: ldc.i4.1
    IL_0013: stsfld [mscorlib]System.Int32 TestCase::flt_caught
    IL_0018: leave IL_001d
    IL_001d: ldsfld [mscorlib]System.Int32 TestCase::exc_not_thrown
    IL_0022: brtrue IL_003d
    IL_0027: ldsfld [mscorlib]System.Int32 TestCase::flt_caught
    IL_002c: brfalse IL_003d
    IL_0031: ldstr "PASS"
    IL_0036: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine(System.String)
    IL_003b: ldc.i4.0
    IL_003c: ret
    IL_003d: ldstr "FAIL"
    IL_0042: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine(System.String)
    IL_0047: ldc.i4.1
    IL_0048: ret
    
    //  .filter IL_000e to IL_0010
    // .try IL_0000 to IL_000e
    // .handler Filter IL_0012 to IL_001d}
  }
}