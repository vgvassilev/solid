// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
	.field int32 v
  //  private int v
  //  public void Main(int n) {
  //    v += n;
  //  }
	.method public hidebysig void Main(int32 n) cil managed
	{
    IL_0000: ldarg.0
    IL_0001: dup
    IL_0002: ldfld System.Int32 TestCase::v
    IL_0007: ldarg.1
    IL_0008: add
    IL_0009: stfld System.Int32 TestCase::v
    IL_000e: ret
	}
}
