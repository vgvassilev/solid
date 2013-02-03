// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly extern mscorlib { }
.assembly TestCase { }

.class public auto ansi beforefieldinit TestCase
 extends [mscorlib]System.Object
{
  .field private static int32 exc_caught
  .field private static int32 flt_caught

  //  C# does not support fault exception handler
  .method public static specialname void .cctor ()
  {
    IL_0000: ldc.i4.0
    IL_0001: stsfld [mscorlib]System.Int32 TestCase::exc_not_thrown
    IL_0006: ldc.i4.0
    IL_0007: stsfld [mscorlib]System.Int32 TestCase::flt_1_caught
    IL_000c: ldc.i4 0
    IL_0011: stsfld [mscorlib]System.Int32 TestCase::flt_2_caught
    IL_0016: ret
  }

  .method public static int32 Main ()
  {
    .try {
      IL_0000: ldc.i4.0
      IL_0001: ldc.i4.0
      IL_0002: div
      IL_0003: ldc.i4.1
      IL_0004: stsfld [mscorlib]System.Int32 TestCase::exc_not_thrown
      IL_0009: leave IL_002c
    } filter {
      IL_000e: pop
      IL_000f: ldc.i4.0
      IL_0010: endfilter
    } {
      IL_0012: ldc.i4.1
      IL_0013: stsfld [mscorlib]System.Int32 TestCase::flt_1_caught
      IL_0018: leave IL_002c
    } filter {
      IL_001d: pop
      IL_001e: ldc.i4.1
      IL_001f: endfilter
    } {
      IL_0021: ldc.i4.1
      IL_0022: stsfld [mscorlib]System.Int32 TestCase::flt_2_caught
      IL_0027: leave IL_002c
    }
    IL_002c: ldsfld [mscorlib]System.Int32 TestCase::exc_not_thrown
    IL_0031: brtrue IL_0056
    IL_0036: ldsfld [mscorlib]System.Int32 TestCase::flt_1_caught
    IL_003b: brtrue IL_0056
    IL_0040: ldsfld [mscorlib]System.Int32 TestCase::flt_2_caught
    IL_0045: brfalse IL_0056
    IL_004a: ldstr "PASS"
    IL_004f: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine(System.String)
    IL_0054: ldc.i4.0
    IL_0055: ret
    IL_0056: ldstr "FAIL"
    IL_005b: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine(System.String)
    IL_0060: ldc.i4.1
    IL_0061: ret
    
    //  .filter IL_000e to IL_0010
    // .try IL_0000 to IL_000e
    // .handler Filter IL_0012 to IL_001d
    //  .filter IL_001d to IL_001f
    // .try IL_0000 to IL_000e
    // .handler Filter IL_0021 to IL_002c
  }
}