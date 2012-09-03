.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static int M(int a, int b, int c) {
  //   return a+b+c;
  // }
  .method public hidebysig static int32  M(int32 a, int32 b, int32 c) cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  add
    IL_0003:  ldarg.2
    IL_0004:  add
    IL_0005:  ret
  }  
}
