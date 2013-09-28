// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   object m = new Object();
  //   Console.WriteLine(m.ToString());
  // }
  .method public hidebysig static void  Main() cil managed
  {
    .maxstack  1
    .locals init (object  V_0)
    IL_0000:  newobj instance void object::'.ctor'()
    IL_0005:  stloc.0
    IL_0006:  ldloc.0
    IL_0007:  callvirt instance string object::ToString()
    IL_000c:  call void class [mscorlib]System.Console::WriteLine(string)
    IL_0011:  ret
  }
}
