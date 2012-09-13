.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static int Main(int a) {
  //   flaot i = M1(a);
  //   return i;
  // }
  //
  // public float M1(int a) {
  //   return 1;
  // }

  .method public static hidebysig int32  Main(int32 a) cil managed
  {
    .entrypoint
    .maxstack  8
    .locals init (int32 V_0)
    //IL_0000:  ldarg.0
    IL_0001:  jmp       float32 TestCase::M1(int32)
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  ret
  }

  .method public static hidebysig float32  M1(int32 a) cil managed
  {
    .maxstack  1
    IL_0000:  ldc.i4.1
    IL_0001:  conv.r4
    IL_0002:  ret
  }

}
