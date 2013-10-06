// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}
.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main () {
  //   uint a = 4;
  //   uint b = 5;
  //   if (a > b)
  //    a = 1;
  //   if (a < b)
  //     b = 1;
  //   if (a == b)
  //     a = 0;
  //   if (a != b)
  //     b = 0;
  //   if (a >= b)
  //     a = 1;
  //   if (a <= b)
  //    b = 1;
  // }
  .method public static hidebysig default void Main ()  cil managed 
  {
    .maxstack 2
    .locals init (unsigned int32  V_0, unsigned int32  V_1)
    IL_0000:  nop 
    IL_0001:  ldc.i4.4 
    IL_0002:  stloc.0 
    IL_0003:  ldc.i4.5 
    IL_0004:  stloc.1 
    IL_0005:  ldloc.0 
    IL_0006:  ldloc.1 
    IL_0007:  ble.un IL_000e

    IL_000c:  ldc.i4.1 
    IL_000d:  stloc.0 
    IL_000e:  ldloc.0 
    IL_000f:  ldloc.1 
    IL_0010:  bge.un IL_0017

    IL_0015:  ldc.i4.1 
    IL_0016:  stloc.1 
    IL_0017:  ldloc.0 
    IL_0018:  ldloc.1 
    IL_0019:  bne.un IL_0020

    IL_001e:  ldc.i4.0 
    IL_001f:  stloc.0 
    IL_0020:  ldloc.0 
    IL_0021:  ldloc.1 
    IL_0022:  beq IL_0029

    IL_0027:  ldc.i4.0 
    IL_0028:  stloc.1 
    IL_0029:  ldloc.0 
    IL_002a:  ldloc.1 
    IL_002b:  blt.un IL_0032

    IL_0030:  ldc.i4.1 
    IL_0031:  stloc.0 
    IL_0032:  ldloc.0 
    IL_0033:  ldloc.1 
    IL_0034:  bgt.un IL_003b

    IL_0039:  ldc.i4.1 
    IL_003a:  stloc.1 
    IL_003b:  ret
  } // end of method MainClass::Main
}


