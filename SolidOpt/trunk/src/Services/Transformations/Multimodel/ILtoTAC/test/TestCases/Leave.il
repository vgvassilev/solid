// RUN: "@ILASM@" /DLL @TEST_CASE@
// XFAIL:*
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  //   try {
  //     int i = 1;
  //   }
  //   catch {
  //     Console.WriteLine ("Exception.");
  //   }
  // }
  .method public hidebysig static void  Main(string[] args) cil managed
  {
    .maxstack 1
    .locals init (int32 V_0)
    IL_0000:  nop
    .try { // 0
      IL_0001:  nop
      IL_0002:  ldc.i4.1
      IL_0003:  stloc.0
      IL_0004:  nop
      IL_0005:  leave IL_001c

    } // end .try 0
    catch class [mscorlib]System.Object { // 0
      IL_000a:  pop
      IL_000b:  nop
      IL_000c:  ldstr "Exception."
      IL_0011:  call void class [mscorlib]System.Console::WriteLine(string)
      IL_0016:  nop
      IL_0017:  leave IL_001c

    } // end handler 0
    IL_001c:  ret 
  }
}
