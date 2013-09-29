// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   int[] a = new int[32];
  //   a[1] = 5;
  //   a[2] = a[1];
  //   m(ref a[2]);
  //
  //   int[,] aobj = new int[32,32];
  //   aobj[1,1] = 5;
  //   aobj[2,2] = aobj[1,1];
  //   m(ref aobj[2,2]);
  // }
  //
  // public static void m(ref int x) {
  //   System.Console.WriteLine(x);
  // }

  .method public static hidebysig default void Main() cil managed
  {
    .entrypoint
    .maxstack 24
    .locals init (int32[] V_0, int32[,] V_1)

    IL_0000:  ldc.i4.s 0x20
    IL_0002:  newarr [mscorlib]System.Int32
    IL_0007:  stloc.0
    IL_0008:  ldloc.0
    IL_0009:  ldc.i4.1
    IL_000a:  ldc.i4.5
    IL_000b:  stelem.i4
    IL_000c:  ldloc.0
    IL_000d:  ldc.i4.2
    IL_000e:  ldloc.0
    IL_000f:  ldc.i4.1
    IL_0010:  ldelem.i4
    IL_0011:  stelem.i4
    IL_0012:  ldloc.0
    IL_0013:  ldc.i4.2
    IL_0014:  ldelema [mscorlib]System.Int32
    IL_0019:  call void class TestCase::m([out] int32&)

    IL_001e:  ldc.i4.s 0x20
    IL_0020:  ldc.i4.s 0x20
    IL_0022:  newobj instance void int32[,]::'.ctor'(int32, int32)
    IL_0027:  stloc.1
    IL_0028:  ldloc.1
    IL_0029:  ldc.i4.1
    IL_002a:  ldc.i4.1
    IL_002b:  ldc.i4.5
    IL_002c:  call instance void int32[,]::Set(int32, int32, int32)
    IL_0031:  ldloc.1
    IL_0032:  ldc.i4.2
    IL_0033:  ldc.i4.2
    IL_0034:  ldloc.1
    IL_0035:  ldc.i4.1
    IL_0036:  ldc.i4.1
    IL_0037:  call instance int32 int32[,]::Get(int32, int32)
    IL_003c:  call instance void int32[,]::Set(int32, int32, int32)
    IL_0041:  ldloc.1
    IL_0042:  ldc.i4.2
    IL_0043:  ldc.i4.2
    IL_0044:  call instance int32&  int32[,]::Address(int32, int32)
    IL_0049:  call void class TestCase::m([out] int32&)
    IL_004e:  ret
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
