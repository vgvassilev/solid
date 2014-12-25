// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static int Main() {
  //   short x = 32767;
  //   short y = 32767;
  //   int z = checked((short)(x + y));
  //   return z;
  // }
  .method public hidebysig static int32  Main() cil managed
  {
    .maxstack  2
    .locals init (
           int16 V_0,
           int16 V_1,
           int32 V_2)
    
    IL_0000:  ldc.i4     0x7fff
    IL_0005:  stloc.0
    IL_0006:  ldc.i4     0x7fff
    IL_000b:  stloc.1
    IL_000c:  ldloc.0
    IL_000d:  ldloc.1
    IL_000e:  add.ovf
    IL_000f:  conv.ovf.i2
    IL_0010:  stloc.2
    IL_0011:  ldloc.2
    IL_0012:  ret
  }
}
