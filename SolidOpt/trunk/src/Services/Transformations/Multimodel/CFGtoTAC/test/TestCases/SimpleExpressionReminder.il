.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static int Main(int a, int b) {
  //   if (a == b) a = 1000;
  //   return a % b;
  // }
  .method public hidebysig static int32  Main(int32 a, int32 b) cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  ceq
    IL_0003:  brfalse.s  IL_000b
    IL_0005:  ldc.i4     1000
    IL_000a:  starg.s    a
    IL_000c:  ldarg.0
    IL_000d:  ldarg.1
    IL_000e:  rem
    IL_000f:  ret
  }  
}

