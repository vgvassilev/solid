// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.custom instance void class [mscorlib]System.Security.UnverifiableCodeAttribute::'.ctor'() =  (01 00 00 00)

.class public auto ansi abstract sealed beforefieldinit TestCase
       extends [mscorlib]System.Object
{

  // public struct MyClass {
  //   int i;
  //   float f;
  // }
  //
  // public static void Main() {
  //   int size = sizeof(int); // compile time calculated
  //   System.Console.WriteLine(size);
  //   size = sizeof(MyClass); // runtime calculated
  //   System.Console.WriteLine(size);
  // }

  .method public static hidebysig default void Main () cil managed
  {
    .entrypoint
    .maxstack 3
    .locals init (int32 V_0)

    IL_0000:  ldc.i4.4
    IL_0001:  stloc.0
    IL_0002:  ldloc.0
    IL_0003:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_0008:  sizeof TestCase/MyClass
    IL_000e:  stloc.0
    IL_000f:  ldloc.0
    IL_0010:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_0015:  ret
  }

  .class nested public sequential ansi sealed beforefieldinit MyClass
         extends [mscorlib]System.ValueType
  {
    .field  private  int32 i
    .field  private  float32 f
  }

}
