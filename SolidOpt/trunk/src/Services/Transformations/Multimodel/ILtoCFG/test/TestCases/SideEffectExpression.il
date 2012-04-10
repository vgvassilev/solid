.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  //  public static void Main() {
  //    int local0, local1;
  //    local0 = 0;
  //    local0 = 1;
  //    local1 = 1 + 0 + 3 + 2;
  //  }
	.method public hidebysig static void Main() cil managed
	{
    .locals init (int32 V_0, int32 V_1)
		IL_0000: ldc.i4.0
		IL_0001: stloc.0
		IL_0002: ldc.i4.3
		IL_0003: ldloc.0
		IL_0004: ldc.i4.1
		IL_0005: add
		IL_0006: dup
		IL_0007: stloc.0
		IL_0008: add
		IL_0009: ldc.i4.2
		IL_000a: add
		IL_000b: stloc.1
		IL_000c: ret
	}
}
