.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  //  public static void Main() {
  //    int a = 4;
  //    if (a % 2 == 0)
  //      throw new Exception("a is an even number");
  //    a = 5;
  //    if (a % 2 == 1)
  //      throw new Exception("a is an odd number");
  //  }
  .method public hidebysig static void Main() cil managed
  {
  .entrypoint
  .maxstack 2
  .locals init ([0] int32 caseSwitch,[1] int32 CS$4$0000)
  IL_0000: ldc.i4.4
  IL_0001: stloc.0
  IL_0002: ldloc.0
  IL_0003: ldc.i4.2
  IL_0004: rem
  IL_0005: brtrue IL_0015
  IL_000a: ldstr "a is an even number"
  IL_000f: newobj System.Void System.Exception::.ctor(System.String)
  IL_0014: throw
  IL_0015: ldc.i4.5
  IL_0016: stloc.0
  IL_0017: ldloc.0
  IL_0018: ldc.i4.2
  IL_0019: rem
  IL_001a: ldc.i4.1
  IL_001b: bne.un IL_002b
  IL_0020: ldstr "a is an odd number"
  IL_0025: newobj System.Void System.Exception::.ctor(System.String)
  IL_002a: throw
  IL_002b: ret
  }
}