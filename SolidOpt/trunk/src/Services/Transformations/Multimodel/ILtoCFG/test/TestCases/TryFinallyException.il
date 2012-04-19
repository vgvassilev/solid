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
  //    } finally {
  //      Console.WriteLine("Finally...");
  //    }
  //  }
  .method public hidebysig static [mscorlib]System.Void Main () cil managed 
  {
    .locals init ([mscorlib]System.Int32 V_0, [mscorlib]System.Int32 V_1)
    .entrypoint
    IL_0000: ldc.i4.4
    IL_0001: stloc.0
    IL_0002: ldc.i4.0
    IL_0003: stloc.1
    //.try {
      IL_0004: ldloc.0
      IL_0005: ldloc.1
      IL_0006: div
      IL_0007: stloc.0
      IL_0008: leave IL_0018
    //} finally {
      IL_000d: ldstr "Finally..."
      IL_0012: call [mscorlib]System.Void [mscorlib]System.Console::WriteLine(System.String)
      IL_0017: endfinally
    //}
    IL_0018: ret
    .try IL_0004 to IL_000d finally handler IL_000d to IL_0018
  }
}