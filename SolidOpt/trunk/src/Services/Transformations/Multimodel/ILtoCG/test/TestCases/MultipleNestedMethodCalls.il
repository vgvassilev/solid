/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  //  public static int One() {
  //    return 1;
  //  }
  //
  //  public static int Two() {
  //    return One() + One();
  //  }
  //
  //  public static int Three() {
  //    return One() + Two();
  //  }
  //
  //  public static int Four() {
  //    return Two() + Three() - One();
  //  }
  //
  //  public static int Five() {
  //    return One() + Four();
  //  }
  //
  //  public static void Main() {
  //    Five();
  //  }
  
  .method public hidebysig static int32 One() cil managed
  {
    .maxstack 1
    .locals init ([0] int32 CS$1$0000)
    L_0000: nop 
    L_0001: ldc.i4.1 
    L_0002: stloc.0 
    L_0003: br.s L_0005
    L_0005: ldloc.0 
    L_0006: ret 
  }

  .method public hidebysig static int32 Two() cil managed
  {
    .maxstack 2
    .locals init ([0] int32 CS$1$0000)
    L_0000: nop 
    L_0001: call int32 TestCase::One()
    L_0006: call int32 TestCase::One()
    L_000b: add.ovf 
    L_000c: stloc.0 
    L_000d: br.s L_000f
    L_000f: ldloc.0 
    L_0010: ret 
  }
  
  .method public hidebysig static int32 Three() cil managed
  {
    .maxstack 2
    .locals init ([0] int32 CS$1$0000)
    L_0000: nop 
    L_0001: call int32 TestCase::One()
    L_0006: call int32 TestCase::Two()
    L_000b: add.ovf 
    L_000c: stloc.0 
    L_000d: br.s L_000f
    L_000f: ldloc.0 
    L_0010: ret 
  }
  
  .method public hidebysig static int32 Four() cil managed
  {
    .maxstack 2
    .locals init ([0] int32 CS$1$0000)
    L_0000: nop 
    L_0001: call int32 TestCase::Two()
    L_0006: call int32 TestCase::Three()
    L_000b: add.ovf 
    L_000c: call int32 TestCase::One()
    L_0011: sub.ovf 
    L_0012: stloc.0 
    L_0013: br.s L_0015
    L_0015: ldloc.0 
    L_0016: ret 
  }
  
  .method public hidebysig static int32 Five() cil managed
  {
    .maxstack 2
    .locals init ([0] int32 CS$1$0000)
    L_0000: nop 
    L_0001: call int32 TestCase::One()
    L_0006: call int32 TestCase::Four()
    L_000b: add.ovf 
    L_000c: stloc.0 
    L_000d: br.s L_000f
    L_000f: ldloc.0 
    L_0010: ret 
  }

  .method public hidebysig static void Main() cil managed
  {
    .entrypoint
    .maxstack 8
    L_0000: nop
    L_0001: call int32 TestCase::Five()
    L_0006: pop
    L_0007: ret
  }
}

