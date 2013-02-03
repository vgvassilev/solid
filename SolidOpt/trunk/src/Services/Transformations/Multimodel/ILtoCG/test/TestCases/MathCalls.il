// RUN: "@ILASM@" /DLL @TEST_CASE@
// XFAIL:
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   Math.Abs(-1);
  // }
 .method public hidebysig static void Main() cil managed
 {
   .locals init (bool local0)
   IL_0000: ldc.i4.m1
   IL_0001: call int16 [mscorlib]System.Math::Abs(int16)
   IL_0006: pop
   IL_0007: ret
 }
}

