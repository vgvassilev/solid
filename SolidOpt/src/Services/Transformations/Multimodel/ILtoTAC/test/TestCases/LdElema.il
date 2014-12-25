// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static int[] i = new int[10];
  // public static void Main() {
  //   System.Console.WriteLine(i[1].ToString());
  // }

  .field  public static  int32[] i

  .method private static specialname rtspecialname default void '.cctor'() cil managed
  {
    IL_0000:  ldc.i4.s 0x0a
    IL_0002:  newarr [mscorlib]System.Int32
    IL_0007:  stsfld int32[] TestCase::i
    IL_000c:  ret
  }

  .method public static hidebysig default void Main() cil managed
  {
    .entrypoint

    IL_0000:  ldsfld int32[] TestCase::i
    IL_0005:  ldc.i4.1
    IL_0006:  ldelema [mscorlib]System.Int32
    IL_000b:  call instance string int32::ToString()
    IL_0010:  call void class [mscorlib]System.Console::WriteLine(string)
    IL_0015:  ret
  }

}

