// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{

  // public static void Main() {
  //   int i;
  //   int j;
  //   object obj;
  //
  //   i = 1;
  //   obj = i; // box
  //
  //   j = (int)obj + 1; // unbox.any
  //
  //   Box(j); // implicit box
  //
  //   Box(UnBox(obj)); // implicit box
  //
  //   UnBox1(ref j);
  //   System.Console.WriteLine(j);
  // }
  //
  // public static void Box(object obj) {
  //   System.Console.WriteLine(obj);
  // }
  //
  // public static int UnBox(object obj) {
  //   return (int)obj;
  // }
  //
  // public static void UnBox1(ref int i) {
  //   object j = 5;
  //   i = (int)j;
  // }

  .method public static hidebysig default void Main () cil managed
  {
    .entrypoint
    .maxstack 6
    .locals init (int32 V_0, int32 V_1, object V_2)

    IL_0000:  ldc.i4.1
    IL_0001:  stloc.0
    IL_0002:  ldloc.0
    IL_0003:  box [mscorlib]System.Int32
    IL_0008:  stloc.2
    IL_0009:  ldloc.2
    IL_000a:  unbox.any [mscorlib]System.Int32
    IL_000f:  ldc.i4.1
    IL_0010:  add
    IL_0011:  stloc.1
    IL_0012:  ldloc.1
    IL_0013:  box [mscorlib]System.Int32
    IL_0018:  call void class TestCase::Box(object)
    IL_001d:  ldloc.2
    IL_001e:  call int32 class TestCase::UnBox(object)
    IL_0023:  box [mscorlib]System.Int32
    IL_0028:  call void class TestCase::Box(object)
    IL_002d:  ldloca.s 1
    IL_002f:  call void class TestCase::UnBox1([out] int32&)
    IL_0034:  ldloc.1
    IL_0035:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_003a:  ret
  }

  .method public static hidebysig default void Box(object obj) cil managed
  {
    .maxstack 8
    IL_0000:  ldarg.0
    IL_0001:  call void class [mscorlib]System.Console::WriteLine(object)
    IL_0006:  ret
  }

  .method public static hidebysig default int32 UnBox(object obj) cil managed
  {
    .maxstack 8
    IL_0000:  ldarg.0
    IL_0001:  unbox.any [mscorlib]System.Int32
    IL_0006:  ret
  }

  .method public static hidebysig default void UnBox1(int32& i) cil managed
  {
    .maxstack 3
    .locals init (object V_0)

    IL_0000:  ldc.i4.5
    IL_0001:  box [mscorlib]System.Int32
    IL_0006:  stloc.0
    IL_0007:  ldarg.0
    IL_0008:  ldloc.0
    IL_0009:  unbox.any [mscorlib]System.Int32
    IL_000e:  stind.i4
    IL_000f:  ret
  }

}
