.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static string Main() {
  //  int v1, v2, v3;
  //  float v4, v5;
  //  string v6;
  //
  //  v1 = 1;
  //  v2 = v1 + 1;
  //  v3 = v2 + 1;
  //  v4 = 4f;
  //  v5 = 5f + v4;
  //  v6 = "abc";
  //
  //  return v6;
  // }
  .method public hidebysig static string  Main() cil managed
  {
    .maxstack  2
    .locals init (int32 V_0,
           int32 V_1,
           int32 V_2,
           float32 V_3,
           float32 V_4,
           string V_5)
    IL_0000:  ldc.i4.1
    IL_0001:  stloc.0
    IL_0002:  ldloc.0
    IL_0003:  ldc.i4.1
    IL_0004:  add
    IL_0005:  stloc.1
    IL_0006:  ldloc.1
    IL_0007:  ldc.i4.1
    IL_0008:  add
    IL_0009:  stloc.2
    IL_000a:  ldc.r4     4.
    IL_000f:  stloc.3
    IL_0010:  ldc.r4     5.
    IL_0015:  ldloc.3
    IL_0016:  add
    IL_0017:  stloc.s    V_4
    IL_0019:  ldstr      "abc"
    IL_001e:  stloc.s    V_5
    IL_0020:  ldloc.s    V_5
    IL_0022:  ret
  }
}
