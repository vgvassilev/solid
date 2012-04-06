.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object

{
  // public static void Main(string[] args) {
  //   int x = 5;
  //   while (x > 0) {
  //     Console.WriteLine(x.ToString());
  //     x--;
  //   }
  // }
  .method public hidebysig static void Main(string[] args) cil managed
  {
      .entrypoint
      .maxstack 2
      .locals init (
          [0] int32 x,
          [1] bool CS$4$0000)
      L_0000: nop
      L_0001: ldc.i4.5
      L_0002: stloc.0
      L_0003: br.s L_0018
      L_0005: nop
      L_0006: ldloca.s x
      L_0008: call instance string [mscorlib]System.Int32::ToString()
      L_000d: call void [mscorlib]System.Console::WriteLine(string)
      L_0012: nop
      L_0013: ldloc.0
      L_0014: ldc.i4.1
      L_0015: sub.ovf
      L_0016: stloc.0
      L_0017: nop
      L_0018: ldloc.0
      L_0019: ldc.i4.0
      L_001a: cgt
      L_001c: stloc.1
      L_001d: ldloc.1
      L_001e: brtrue.s L_0005
      L_0020: ret
  }
}