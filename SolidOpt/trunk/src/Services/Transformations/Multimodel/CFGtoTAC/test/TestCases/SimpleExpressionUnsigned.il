// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static int Main(int a, int b) {
  //   if (a == b) a = 1000;
  //   return a % b;
  // }
  .method public hidebysig static uint32  Main(uint32 a, int32 b) cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  div.un
    IL_0003:  ret
  }  
}

