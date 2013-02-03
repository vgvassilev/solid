// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  // }
  .method public hidebysig static void  Main() cil managed
  {
    IL_0000:  nop
    IL_0001:  ret
  }  
}
