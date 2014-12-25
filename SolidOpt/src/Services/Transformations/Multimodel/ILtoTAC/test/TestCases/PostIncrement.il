// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static int Main() {
  //   int i = 0;
  //   int j;
  //   j = i++;
  //   return j;
  // }
  .method public hidebysig static int32  Main() cil managed
  {
    .maxstack  3
    .locals init (int32 V_0,
             int32 V_1)
    IL_0000:  ldc.i4.0
    IL_0001:  stloc.0
    IL_0002:  ldloc.0
    IL_0003:  dup
    IL_0004:  ldc.i4.1
    IL_0005:  add
    IL_0006:  stloc.0
    IL_0007:  stloc.1
    IL_0008:  ldloc.1
    IL_0009:  ret
  }  
}
