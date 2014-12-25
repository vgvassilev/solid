// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main(int a) {
  //   Console.WriteLine(a.ToString());
  // }
  .method public hidebysig static void Main(int32 a) cil managed
  {
    .entrypoint

    IL_000d:  ldarga.s 0
    IL_000f:  call instance string int32::ToString()
    IL_0014:  call void class [mscorlib]System.Console::WriteLine(string)
    IL_0019:  ret
  }
}

