.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main(string[] args) {
  //   Console.WriteLine("This is switch test");
  //   int x = 3;
  //     switch (x) {
  //       case 1: Console.WriteLine("1");
  //       break;
  //       case 2: Console.WriteLine("2");
  //       break;
  //       case 3: Console.WriteLine("3");
  //       break;
  //     }
  // }
  .method public hidebysig static void Main(string[] args) cil managed
  {
      .entrypoint
      .maxstack 2
      .locals init (
          [0] int32 x,
          [1] int32 CS$4$0000)
      L_0000: nop
      L_0001: ldstr "This is switch test"
      L_0006: call void [mscorlib]System.Console::WriteLine(string)
      L_000b: nop
      L_000c: ldc.i4.3
      L_000d: stloc.0
      L_000e: ldloc.0
      L_000f: stloc.1
      L_0010: ldloc.1
      L_0011: ldc.i4.1
      L_0012: sub
      L_0013: switch (L_0026, L_0033, L_0040)
      L_0024: br.s L_004d
      L_0026: ldstr "1"
      L_002b: call void [mscorlib]System.Console::WriteLine(string)
      L_0030: nop
      L_0031: br.s L_004d
      L_0033: ldstr "2"
      L_0038: call void [mscorlib]System.Console::WriteLine(string)
      L_003d: nop
      L_003e: br.s L_004d
      L_0040: ldstr "3"
      L_0045: call void [mscorlib]System.Console::WriteLine(string)
      L_004a: nop
      L_004b: br.s L_004d
      L_004d: ret
  }
}