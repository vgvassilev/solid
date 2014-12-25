// RUN: "@ILASM@" /DLL @TEST_CASE@
// XFAIL:*
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main (string[] args) {
  //   IntPtr pointer1 = System.Runtime.InteropServices.Marshal.AllocHGlobal(2);
  //   IntPtr pointer2 = System.Runtime.InteropServices.Marshal.AllocHGlobal(2);
  //   Init(pointer1, pointer2, 2);
  //   Copy (pointer1, pointer2, 2);
  // }
  .method public hidebysig static void Main(string[] args) cil managed
  {
  .entrypoint
  .maxstack 3
  .locals init (native int  V_0, native int  V_1)
  IL_0000:  nop 
  IL_0001:  ldc.i4.2 
  IL_0002:  call native int class [mscorlib]System.Runtime.InteropServices.Marshal::AllocHGlobal(int32)
  IL_0007:  stloc.0 
  IL_0008:  ldc.i4.2 
  IL_0009:  call native int class [mscorlib]System.Runtime.InteropServices.Marshal::AllocHGlobal(int32)
  IL_000e:  stloc.1 
  IL_000f:  ldloc.0 
  IL_0010:  ldc.i4.0 
  IL_0011:  ldc.i4.2 
  IL_0012:  call void class TestCase::Init(native int, unsigned int8, int32)
  IL_0017:  ldloc.0 
  IL_0018:  ldloc.1 
  IL_0019:  ldc.i4.2 
  IL_001a:  call void class TestCase::Copy(native int, native int, unsigned int32)
  IL_001f:  ret 
  } // end of method TestCase::Main
  
  // The compiler doesn't produce cpblk instruction for C#  
  .method public hidebysig static void Copy (native int destination, native int source, unsigned int32 size) cil managed 
  { 
    .maxstack 3
    ldarg.0
    ldarg.1
    ldarg.2
    cpblk
    ret  
  } // end of method TestCase::Copy


  // The compiler doesn't produce cpblk instruction for C#
  .method public hidebysig static void Init (native int address, unsigned int8 val, int32 size) cil managed 
  { 
    .maxstack 3
    ldarg.0
    ldarg.1
    ldarg.2
    initblk
    ret
  } // end of method TestCase::Init
}
