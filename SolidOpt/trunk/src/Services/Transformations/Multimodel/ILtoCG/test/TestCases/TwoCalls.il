// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
// public static int A()
// {
//   return 3 + 4;
// }
//
// public static int B(int b)
// {
//   return b + 1;
// }
//
// public static void Main()
// {
//   A();
//   int b = 5;
//   B(b);
// }
  .method public hidebysig static void Main() cil managed
  {
    .entrypoint
    .maxstack 1
    .locals init (
        [0] int32 num)
    L_0000: call int32 TestCase::A()
    L_0005: pop
    L_0006: ldc.i4.5
    L_0007: stloc.0
    L_0008: ldloc.0
    L_0009: call int32 TestCase::B([mscorlib]System.Int32)
    L_000e: pop
    L_000f: ret
  }

  .method public hidebysig static [mscorlib]System.Int32 B([mscorlib]System.Int32 b) cil managed
  {
    .maxstack 8
    L_0000: ldarg.0
    L_0001: ldc.i4.1
    L_0002: add
    L_0003: ret
  }
  .method public hidebysig static [mscorlib]System.Int32 A() cil managed
  {
    .maxstack 8
    L_0000: ldc.i4.7
    L_0001: ret
  }
}