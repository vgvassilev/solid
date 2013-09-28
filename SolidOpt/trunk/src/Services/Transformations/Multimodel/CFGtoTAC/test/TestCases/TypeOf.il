// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   System.Type t = typeof(System.Object);
  //   System.Console.WriteLine(t);
  // }

  .method public static hidebysig default void Main() cil managed
  {
    .entrypoint
    .maxstack 3
    .locals init (class [mscorlib]System.Type V_0)

    IL_0000:  ldtoken [mscorlib]System.Object
    IL_0005:  call class [mscorlib]System.Type class [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
    IL_000a:  stloc.0
    IL_000b:  ldloc.0
    IL_000c:  call void class [mscorlib]System.Console::WriteLine(object)
    IL_0011:  ret
  }

}
