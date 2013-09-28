// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   int[,] a = new int[32,32];
  //   a[1,1] = 5;
  //   a[2,2] = a[1,1];
  //   m(ref a[2,2]);
  // }
  //
  // public static void m(ref int x) {
  //   System.Console.WriteLine(x);
  // }

  .method public static hidebysig default void Main() cil managed
  {
    .entrypoint
    .maxstack 23
    .locals init (int32[,] V_0)

    IL_0000:  ldc.i4.s 0x20
    IL_0002:  ldc.i4.s 0x20
    IL_0004:  newobj instance void int32[,]::'.ctor'(int32, int32)
    IL_0009:  stloc.0
    IL_000a:  ldloc.0
    IL_000b:  ldc.i4.1
    IL_000c:  ldc.i4.1
    IL_000d:  ldc.i4.5
    IL_000e:  call instance void int32[,]::Set(int32, int32, int32)
    IL_0013:  ldloc.0
    IL_0014:  ldc.i4.2
    IL_0015:  ldc.i4.2
    IL_0016:  ldloc.0
    IL_0017:  ldc.i4.1
    IL_0018:  ldc.i4.1
    IL_0019:  call instance int32 int32[,]::Get(int32, int32)
    IL_001e:  call instance void int32[,]::Set(int32, int32, int32)
    IL_0023:  ldloc.0
    IL_0024:  ldc.i4.2
    IL_0025:  ldc.i4.2
    IL_0026:  call instance int32&  int32[,]::Address(int32, int32)
    IL_002b:  call void class TestCase::m([out] int32&)
    IL_0030:  ret
  }

  .method public static hidebysig default void m(int32& x) cil managed
  {
    .maxstack 8

    IL_0000:  ldarg.0
    IL_0001:  ldind.i4
    IL_0002:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_0007:  ret
  }

}
