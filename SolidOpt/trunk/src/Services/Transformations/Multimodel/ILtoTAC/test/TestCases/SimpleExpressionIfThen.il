// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static int Main(int a, int b, int c) {
  //   if (a + b > c) return c;
  //   return a;
  // }
  .method public hidebysig static int32  Main(int32 a, int32 b, int32 c) cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  add
    IL_0003:  ldarg.2
    IL_0004:  ble.s IL_0008
    IL_0006:  ldarg.2
    IL_0007:  ret
    IL_0008:  ldarg.0
    IL_0009:  ret
  }  
}
