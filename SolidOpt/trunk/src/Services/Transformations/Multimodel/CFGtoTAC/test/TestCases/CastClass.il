// RUN: "@ILASM@" /DLL @TEST_CASE@
// XFAIL:*
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   object m = new String('a',1);
  //   Console.WriteLine((String)m);
  // }
  .method public hidebysig static void  Main() cil managed
  {
    .entrypoint
    // Code size 21 (0x15)
    .maxstack 3
    .locals init (object  V_0)
    IL_0000:  ldc.i4.s 0x61
    IL_0002:  ldc.i4.1
    IL_0003:  newobj instance void string::'.ctor'(char, int32)
    IL_0008:  stloc.0
    IL_0009:  ldloc.0
    IL_000a:  castclass [mscorlib]System.String
    IL_000f:  call void class [mscorlib]System.Console::WriteLine(string)
    IL_0014:  ret
  }
}