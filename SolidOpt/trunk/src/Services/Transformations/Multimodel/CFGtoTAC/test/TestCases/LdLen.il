// RUN: "@ILASM@" /DLL @TEST_CASE@
// XFAIL:*
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  .method public hidebysig specialname rtspecialname 
          instance default void '.ctor' ()  cil managed 
   {
     .maxstack 8
     IL_0000:  ldarg.0 
     IL_0001:  call instance void object::'.ctor'()
     IL_0006:  ret 
   } // end of method TestCase::.ctor


  // public static void Main() {
  //   int[] simpleArray = new int[2];
  //   Console.WriteLine(simpleArray.Length);
  // }
  .method public hidebysig static void  Main() cil managed
  {
    .maxstack 2
    .locals init (int32[] V_0)
    IL_0000:  ldc.i4.2
    IL_0001:  newarr [mscorlib]System.Int32
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  ldlen
    IL_0009:  conv.i4 // Because the ret value of ldlen is a native unsigned int
    IL_000a:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_000f:  ret
  }
}

