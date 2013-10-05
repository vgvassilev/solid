// RUN: "@ILASM@" /DLL @TEST_CASE@
// XFAIL:*
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   try {
  //   }
  //   catch {
  //     throw;
  //   }
  // }
  .method public hidebysig static void  Main(string[] args) cil managed
  {
    .maxstack 1
    IL_0000:  nop 
    .try { // 0
      IL_0001:  nop 
      IL_0002:  nop 
      IL_0003:  leave IL_000c

    } // end .try 0
    catch class [mscorlib]System.Object { // 0
      IL_0008:  pop 
      IL_0009:  nop 
      IL_000a:  rethrow 
    } // end handler 0
    IL_000c:  ret 
  }
}
