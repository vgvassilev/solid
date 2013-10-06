// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   int a = 2;
  //   a = a >> 2;
  //   uint b = 4;
  //   b = b >> 4;

  //   int c = 0;
  //   c = c << 1;
  //   uint d = 0;
  //   d = d << 3;
  // }
  .method public hidebysig static void  Main(string[] args) cil managed
  {
    .maxstack 2
    .locals init (int32 V_0, unsigned int32  V_1, int32 V_2, unsigned int32  V_3)
    IL_0000:  nop
    IL_0001:  ldc.i4.2
    IL_0002:  stloc.0
    IL_0003:  ldloc.0
    IL_0004:  ldc.i4.2
    IL_0005:  shr
    IL_0006:  stloc.0
    IL_0007:  ldc.i4.4
    IL_0008:  stloc.1
    IL_0009:  ldloc.1
    IL_000a:  ldc.i4.4
    IL_000b:  shr.un
    IL_000c:  stloc.1
    IL_000d:  ldc.i4.0
    IL_000e:  stloc.2
    IL_000f:  ldloc.2
    IL_0010:  ldc.i4.1
    IL_0011:  shl
    IL_0012:  stloc.2
    IL_0013:  ldc.i4.0
    IL_0014:  stloc.3
    IL_0015:  ldloc.3
    IL_0016:  ldc.i4.3
    IL_0017:  shl
    IL_0018:  stloc.3
    IL_0019:  ret
  }
}
