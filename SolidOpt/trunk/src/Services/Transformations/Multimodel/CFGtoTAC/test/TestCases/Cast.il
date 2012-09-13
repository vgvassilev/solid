.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static short Main() {
  //   int i = -5;
  //   short s = (short)i;
  //   return s;
  // }
  .method public hidebysig static int16  Main() cil managed
  {
    .maxstack  1
    .locals init (int32 V_0, int16 V_1)
    
    IL_0000:  ldc.i4.s   -5
    IL_0002:  stloc.0
    IL_0003:  ldloc.0
    IL_0004:  conv.i2
    IL_0005:  stloc.1
    IL_0006:  ldloc.1
    IL_0007:  ret
  }
}
