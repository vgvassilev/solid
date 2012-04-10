.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  //  public static void Main() {
  //   int caseSwitch = 2;
  //   switch (caseSwitch)
  //   {
  //       case 1:
  //           Console.WriteLine("Case 0 or 1");
  //           break;
  //       case 2:
  //       case 3:
  //           Console.WriteLine("Case 2 or 3");
  //     break;
  //     case 4:
  //           Console.WriteLine("Case 4");
  //           break;
  //   }
  // }
	.method public hidebysig static void Main(string[] args) cil managed
  {
  .entrypoint
  .maxstack 2
  .locals init ([0] int32 caseSwitch,[1] int32 CS$4$0000)
  IL_0000: nop
  IL_0001: ldc.i4.2
  IL_0002: stloc.0
  IL_0003: ldloc.0
  IL_0004: stloc.1
  IL_0005: ldloc.1
  IL_0006: ldc.i4.1
  IL_0007: sub
  IL_0008: switch (IL_001f,IL_002c,IL_002c,IL_0039)
  IL_001d: br.s IL_0046
  IL_001f: ldstr "Case 1"
  IL_0024: call void [mscorlib]System.Console::WriteLine(string)
  IL_0029: nop
  IL_002a: br.s IL_0046
  IL_002c: ldstr "Case 2 or 3"
  IL_0031: call void [mscorlib]System.Console::WriteLine(string)
  IL_0036: nop
  IL_0037: br.s IL_0046
  IL_0039: ldstr "Case 4"
  IL_003e: call void [mscorlib]System.Console::WriteLine(string)
  IL_0043: nop
  IL_0044: br.s IL_0046
  IL_0046: ret
  }
}
