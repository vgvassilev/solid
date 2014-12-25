// RUN: "@ILASM@" /DLL @TEST_CASE@
// XFAIL:*
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //  // __refvalue and __makeref are C# language extensions, which are
  //  // not standardized, whereas the generated CIL instructions are
  //  // very well standardized in ECMA 335
  //   __refvalue(__makeref(args[0]), int) = 3;
  // }
  .method public hidebysig static void  Main(string[] args) cil managed
  {
    .maxstack 8
    IL_0000:  nop
    IL_0001:  ldarg.0
    IL_0002:  ldc.i4.0
    IL_0003:  ldelema [mscorlib]System.String
    IL_0008:  mkrefany [mscorlib]System.String
    IL_000d:  refanyval [mscorlib]System.Int32
    IL_0012:  ldc.i4.3
    IL_0013:  stind.i4
    IL_0014:  ret
  }
}