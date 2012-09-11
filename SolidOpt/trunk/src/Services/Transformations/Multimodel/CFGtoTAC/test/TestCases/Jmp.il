.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public int Main(int a) {
  //   int i = M1(a);
  //   return i;
  // }
  //
  // public int M1(int a) {
  //   return a;
  // }

  .method public hidebysig instance void  Main() cil managed
  {
    .maxstack  2
    .locals init (int32 V_0)
    IL_0000:  ldarg.0
    IL_0001:  jmp       instance int32 TestCase::M1(int32)
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  ret
  }

  .method public hidebysig instance int32  M1(int32 a) cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.1
    IL_0001:  ret
  }
}

