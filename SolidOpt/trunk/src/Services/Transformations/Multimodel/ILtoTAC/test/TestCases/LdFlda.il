// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public class MyClass {
  //   public int i;
  //   public float f;
  // }
  //
  // public static void Main() {
  //   System.Console.WriteLine(new MyClass().i.ToString());
  // }

  .method public static hidebysig default void Main() cil managed
  {
    .entrypoint
    .maxstack 8

    IL_0000:  newobj instance void class TestCase/MyClass::'.ctor'()
    IL_0005:  ldflda int32 TestCase/MyClass::i
    IL_000a:  call instance string int32::ToString()
    IL_000f:  call void class [mscorlib]System.Console::WriteLine(string)
    IL_0014:  ret
  }

  .class nested public auto ansi beforefieldinit MyClass
         extends [mscorlib]System.Object
  {
    .field  public  int32 i
    .field  public  float32 f

    .method public hidebysig specialname rtspecialname instance default void '.ctor'() cil managed
    {
      IL_0000:  ldarg.0
      IL_0001:  call instance void object::'.ctor'()
      IL_0006:  ret
    }
  }

}

