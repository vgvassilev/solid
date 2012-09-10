.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  .method public hidebysig static int32  Main() cil managed
  {
    .locals init (
             int32 V_0,
             int32 V_1,
             int32 V_2)
    
    ldc.i4.1
    stloc.0
    ldc.i4.2
    stloc.1
    ldc.i4.3
    stloc.2
    
    ldloc.0
    dup
    ldloc.1
    dup
    ldloc.2
    dup
    
    add
    add
    add
    add
    add
    
    ret
  }  
}
