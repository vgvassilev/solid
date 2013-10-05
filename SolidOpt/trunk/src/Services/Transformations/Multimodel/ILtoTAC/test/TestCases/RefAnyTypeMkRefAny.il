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
  //   var i = __reftype(__makeref(args[0]));
  // }
  .method public hidebysig static void  Main(string[] args) cil managed
  {
    .maxstack 2
    .locals init (class [mscorlib]System.Type V_0)
    IL_0000:  nop 
    IL_0001:  ldarg.0 
    IL_0002:  ldc.i4.0 
    IL_0003:  ldelema [mscorlib]System.String
    IL_0008:  mkrefany [mscorlib]System.String
    IL_000d:  refanytype 
    IL_000f:  call class [mscorlib]System.Type class [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
    IL_0014:  stloc.0 
    IL_0015:  ret 
  }
}
