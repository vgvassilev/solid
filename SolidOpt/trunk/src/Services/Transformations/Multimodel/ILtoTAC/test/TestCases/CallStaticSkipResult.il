// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   M1(1);
  // }
  // 
  // public static int M1(int a) {
  //   return a;
  // }
  
  .method public hidebysig static void  Main() cil managed
  {
    .maxstack  8
    IL_0000:  ldc.i4.1
    IL_0001:  call       int32 TestCase::M1(int32)
    IL_0006:  pop
    IL_0007:  ret
  }  

  .method public hidebysig static int32  M1(int32 a) cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ret
  }  
}
