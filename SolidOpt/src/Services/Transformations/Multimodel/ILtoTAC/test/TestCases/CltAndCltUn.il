// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   uint a = 1, b = 2;
  //   bool c = a < b;
  //   int d = 3, e = 4;
  //   bool f = d < e;
  // }
  .method public hidebysig static void  Main(string[] args) cil managed
  {
    .maxstack 2
    .locals init (unsigned int32  V_0, unsigned int32  V_1, bool  V_2, int32 V_3, int32 V_4, bool  V_5)
    IL_0000:  nop
    IL_0001:  ldc.i4.1
    IL_0002:  stloc.0
    IL_0003:  ldc.i4.2
    IL_0004:  stloc.1
    IL_0005:  ldloc.0
    IL_0006:  ldloc.1
    IL_0007:  clt.un
    IL_0009:  stloc.2
    IL_000a:  ldc.i4.3
    IL_000b:  stloc.3
    IL_000c:  ldc.i4.4
    IL_000d:  stloc.s 4
    IL_000f:  ldloc.3
    IL_0010:  ldloc.s 4
    IL_0012:  clt
    IL_0014:  stloc.s 5
    IL_0016:  ret
  } // end of method TestCase::Main
}
