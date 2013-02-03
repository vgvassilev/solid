// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static bool Main(string s) {
  //   s.StartsWith("A")
  //   s.Length;
  // }
 .method public hidebysig static void Main(string s) cil managed
 {
   .locals init (bool local0)
   IL_0000: ldarg.0
   IL_0001: ldstr "A"
   IL_0006: callvirt bool string::StartsWith(string)
   IL_000b: ldarg.0
   IL_000c: callvirt int32 string::get_Length()
   IL_0011: pop
   IL_0012: ret
 }
}
