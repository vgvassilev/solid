// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  //  static int field;
  //  public static int Main() {
  //    bool flag;
  //    field = 42;
  //    return flag = field;
  //  }
	.method public hidebysig static int32 Main() cil managed
	{
		.locals init (bool local0)
		IL_0000: nop
		IL_0001: ldc.i4.s 42
		IL_0003: stsfld int32 TestCase::_field
		IL_0008: ldsfld int32 TestCase::_field
		IL_000d: stloc.0
		IL_000e: br.s IL_0010
		IL_0010: ldloc.0
		IL_0011: ret
	}
}
