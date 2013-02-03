// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  .method public hidebysig static int32  Main() cil managed
  {
    .locals init (
             int32 V_0,
             int32 V_1,
             int32 V_2,
             int32 V_3)
    
    ldc.i4.1
    stloc.0
    
    nop
    
    ldloc.0
    dup
    stloc.1
    stloc.2
    
    nop
    
    ldc.i4.5
    dup
    stloc.1
    stloc.2
    
    nop
    
    ldloc.0
    dup
    dup
    stloc.1
    stloc.2
    stloc.3
    
    ret
  }  
}
