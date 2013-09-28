// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  .method public static hidebysig default void Main () cil managed
  {
    .entrypoint
    .maxstack 3
    .locals init (class [mscorlib]System.Type V_0)

    IL_0000:  ldtoken method void class TestCase::M(object)
    IL_0005:  call class [mscorlib]System.Reflection.MethodBase [mscorlib]System.Reflection.MethodBase::GetMethodFromHandle(valuetype [mscorlib]System.RuntimeMethodHandle)
    IL_000a:  call void class [mscorlib]System.Console::WriteLine(object)
    IL_000f:  ret
  }

  .method public static hidebysig default void M (object obj) cil managed
  {
    IL_0000:  ldarg.0
    IL_0001:  call void class [mscorlib]System.Console::WriteLine(object)
    IL_0006:  ret
  }

}
