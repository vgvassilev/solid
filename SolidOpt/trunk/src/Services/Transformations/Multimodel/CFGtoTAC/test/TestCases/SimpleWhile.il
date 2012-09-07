.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static int Main(int a, int b) {
  //   while (a > b) a = a - b;
  //   return a;
  // }
  .method public hidebysig static int32  Main(int32 a, int32 b) cil managed
  {
    .maxstack  8
    IL_0000:  br.s       IL_0007
    IL_0002:  ldarg.0
    IL_0003:  ldarg.1
    IL_0004:  sub
    IL_0005:  starg.s    a
    IL_0007:  ldarg.0
    IL_0008:  ldarg.1
    IL_0009:  bgt.s      IL_0002
    IL_000b:  ldarg.0
    IL_000c:  ret
  }  
}
