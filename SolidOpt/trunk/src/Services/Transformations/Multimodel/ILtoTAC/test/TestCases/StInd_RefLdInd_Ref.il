// RUN: "@ILASM@" /DLL @TEST_CASE@
// XFAIL:*
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   object x = new Int32();
  //   ByRef(ref x);
  //   Console.WriteLine(x);
  // }
  .method public hidebysig static void Main() cil managed
  {
    .maxstack 3
    .locals init (object  V_0)
    IL_0000:  ldc.i4.0 
    IL_0001:  box [mscorlib]System.Int32
    IL_0006:  stloc.0 
    IL_0007:  ldloca.s 0
    IL_0009:  call void class TestCase::ByRef([out] object&)
    IL_000e:  ldloc.0 
    IL_000f:  call void class [mscorlib]System.Console::WriteLine(object)
    IL_0014:  ret 
  } // end of method TestCase::Main
  
  .method private static hidebysig default void ByRef (object& x)  cil managed 
  {
    .maxstack 8
    IL_0000:  ldarg.0 
    IL_0001:  ldind.ref 
    IL_0002:  call void class [mscorlib]System.Console::WriteLine(object)
    IL_0007:  ldarg.0 
    IL_0008:  ldc.i4.0 
    IL_0009:  box [mscorlib]System.Int32
    IL_000e:  stind.ref 
    IL_000f:  ret 
  } // end of method TestCase::ByRef
} // end of class TestCase

