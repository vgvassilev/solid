// RUN: "@ILASM@" /DLL @TEST_CASE@
// XFAIL:*
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   Random r = new Random();
  //   Console.WriteLine(r.Next().ToString());
  // }
  .method public hidebysig static void Main() cil managed
  {
    .maxstack 6
    .locals init (class [mscorlib]System.Random V_0, int32 V_1)
    IL_0000:  newobj instance void class [mscorlib]System.Random::'.ctor'()
    IL_0005:  stloc.0
    IL_0006:  ldloc.0
    IL_0007:  callvirt instance int32 class [mscorlib]System.Random::Next()
    IL_000c:  stloc.1
    IL_000d:  ldloca.s 1
    IL_000f:  call instance string int32::ToString()
    IL_0014:  call void class [mscorlib]System.Console::WriteLine(string)
    IL_0019:  ret
  } // end of method TestCase::Main
} // end of class TestCase

