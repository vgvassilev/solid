// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main(int x) {
  //   if (x > 0)
  //     Console.WriteLine("x > 0");
  //   return x < 0
  // }
	.method public hidebysig static bool Main(int32 x) cil managed
	{
		.locals init (bool local0)
		IL_0000: ldarg.0
		IL_0001: ldc.i4.0
		IL_0002: ble.s IL_000e
		IL_0004: ldstr "x > 0"
		IL_0009: call void [mscorlib]System.Console::WriteLine(System.String)
		IL_000e: ldarg.0
		IL_000f: ldc.i4.0
		IL_0010: clt
		IL_0012: stloc.0
		IL_0013: br.s IL_0015
		IL_0015: ldloc.0
		IL_0016: ret
	}
}
