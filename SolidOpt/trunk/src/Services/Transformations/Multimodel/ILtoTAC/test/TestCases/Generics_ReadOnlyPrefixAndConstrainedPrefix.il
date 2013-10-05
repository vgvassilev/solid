// RUN: "@ILASM@" /DLL @TEST_CASE@
// XFAIL:*
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static int Main() {
  // }
  .method public hidebysig static int32  Main() cil managed
  {
    .maxstack  2    
    IL_0000:  nop
    IL_0001:  ret
  } // end of method TestCase::Main
  
  // public void ReadOnlyAndConstrained<T>(T[] array) {
  //   array[0].ToString ();
  // }
  .method public hidebysig instance default void ReadOnlyAndConstrained<T> (!!T[] 'array')  cil managed 
  {
    .maxstack 8
    IL_0000:  nop
    IL_0001:  ldarg.1
    IL_0002:  ldc.i4.0
    IL_0003:  readonly.
    IL_0005:  ldelema !!0
    IL_000a:  constrained. !!0
    IL_0010:  callvirt instance string object::ToString()
    IL_0015:  pop
    IL_0016:  ret
  } // end of method TestCase::ReadOnlyAndConstrained
}
