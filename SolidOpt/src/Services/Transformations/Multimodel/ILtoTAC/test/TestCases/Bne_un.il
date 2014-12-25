// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}
.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main () {
  //   uint a = 1;
  //   if (a == 2)
  //     Console.Write("1");
  // }
  .method public static hidebysig default void Main ()  cil managed 
  {
    .maxstack 2
    .locals init (unsigned int32  V_0)
    IL_0000:  ldc.i4.1 
    IL_0001:  stloc.0 
    IL_0002:  ldloc.0 
    IL_0003:  ldc.i4.2 
    IL_0004:  bne.un IL_0013

    IL_0009:  ldstr "1"
    IL_000e:  call void class [mscorlib]System.Console::Write(string)
    IL_0013:  ret 
  } // end of method MainClass::Main
}

