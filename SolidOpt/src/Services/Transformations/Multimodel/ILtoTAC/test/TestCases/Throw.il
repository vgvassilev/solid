// RUN: "@ILASM@" /DLL @TEST_CASE@
// XFAIL:*
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   throw new Exception ();
  // }
  .method public hidebysig static void  Main(string[] args) cil managed
  {
    .maxstack 8
    IL_0000:  nop 
    IL_0001:  newobj instance void class [mscorlib]System.Exception::'.ctor'()
    IL_0006:  throw 
  } // end of method TestCase::Main
}

