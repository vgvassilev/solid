// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static int Main(int a, int b) {
  //   do {
  //     a = a * 2;
  //   } while (a < b);
  //   return a;
  // }
  .method public hidebysig static int32  Main(int32 a, int32 b) cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldc.i4.2
    IL_0002:  mul
    IL_0003:  starg.s    a
    IL_0005:  ldarg.0
    IL_0006:  ldarg.1
    IL_0007:  blt.s      IL_0000
    IL_0009:  ldarg.0
    IL_000a:  ret
  }  
}
