.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
	.field int32 _field
  // public int field;
  // public static int Main() {
  //   int loc0 = field;
  //   return loc0;
  // }
	.method public hidebysig int32 Main() cil managed
	{
		.locals init (int32 local0)
		IL_0000: ldarg.0
		IL_0001: ldfld int32 TestCase::_field
		IL_0006: stloc.0
		IL_0007: br.s IL_0009
		IL_0009: ldloc.0
		IL_000a: ret
	}
}
