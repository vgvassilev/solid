// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static int M0() {
  //   int i = Main(0);
  //   System.Console.WriteLine(i);
  //   return i;
  // }
  //
  // public int Main(int a) {
  //   return M1(a);
  // }
  //
  // public string M1(int a) {
  //   return "abc";
  // }

  .method public static hidebysig void  М0() cil managed
  {
    .entrypoint
    .maxstack  8
    .locals init (int32 V_0)
    IL_0000:  ldc.i4.0
    IL_0001:  call       int32 TestCase::Main(int32)
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  call       void [mscorlib]System.Console::WriteLine(int32)
    IL_000d:  ret
  }

  .method public static hidebysig int32  Main(int32 a) cil managed
  {
    .maxstack  1
    //IL_0000:  ldarg.0
    //IL_0001:  call  string TestCase::M1(int32)
    //IL_0006:  ret
    IL_0000:  jmp       string TestCase::M1(int32)
  }
  
  .method public static hidebysig string  M1(int32 a) cil managed
  {
    .maxstack  1
    IL_0000:  ldstr "abc"
    IL_0005:  ret
  }

}
