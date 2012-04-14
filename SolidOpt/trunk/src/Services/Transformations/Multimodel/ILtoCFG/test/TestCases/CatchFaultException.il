.assembly extern mscorlib { }
.assembly 'TestCase' { }

.class public auto ansi beforefieldinit TestCase
 extends [mscorlib]System.Object
{
  .field private static int32 exc_caught
  .field private static int32 flt_caught

  //  C# does not support fault exception handler
  .method public static specialname void .cctor ()
  {
    IL_0000: ldc.i4.0
    IL_0001: stsfld System.Int32 TestCase::exc_caught
    IL_0006: ldc.i4.0
    IL_0007: stsfld System.Int32 TestCase::flt_caught
    IL_000c: ret
  }
  
  .method public static int32 Main ()
  {
    IL_0000: ldstr "#jibba!!jabba"
    IL_0005: newobj [mscorlib]System.Void [mscorlib]System.Uri::.ctor(System.String)
    IL_000a: leave IL_0021
    IL_000f: ldc.i4.1
    IL_0010: stsfld [mscorlib]System.Int32 TestCase::exc_caught
    IL_0015: leave IL_0021
    IL_001a: ldc.i4.1
    IL_001b: stsfld [mscorlib]System.Int32 TestCase::flt_caught
    IL_0020: endfinally
    IL_0021: ldsfld [mscorlib]System.Int32 TestCase::exc_caught
    IL_0026: brfalse IL_0041
    IL_002b: ldsfld [mscorlib]System.Int32 TestCase::flt_caught
    IL_0030: brfalse IL_0041
    IL_0035: ldstr "PASS"
    IL_003a: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine(System.String)
    IL_003f: ldc.i4.0
    IL_0040: ret
    IL_0041: ldstr "FAIL"
    IL_0046: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine(System.String)
    IL_004b: ldc.i4.1
    IL_004c: ret

    // .try IL_0000 to IL_000f
    // .handler Catch IL_000f to IL_001a
    // .try IL_0000 to IL_001a
    // .handler Fault IL_001a to IL_0021
  }
}