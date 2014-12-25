// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  //  public static bool Main(int x) {
  //    if (x != 1) {
  //    }
  //    return true;
  //  }
  .method public hidebysig static bool Main(int32 x) cil managed
  {
    .locals init (bool local0)
		IL_0000: ldarg.0
		IL_0001: ldc.i4.1
		IL_0002: beq.s IL_0004
		IL_0004: ldc.i4.1
		IL_0005: ret
	}
}
