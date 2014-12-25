// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static int i;
  //
  // public static void Main() {
  //   System.Console.WriteLine(i.ToString());
  // }

  .field  public static  int32 i

  .method public static hidebysig default void Main() cil managed
  {
    .entrypoint

    IL_0000:  ldsflda int32 TestCase::i
    IL_0005:  call instance string int32::ToString()
    IL_000a:  call void class [mscorlib]System.Console::WriteLine(string)
    IL_000f:  ret
  }
}

