.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static string Main(int a) {
  //   switch (a) {
  //     case 0: return "Zero";
  //     case 1: return "One";
  //     case 2: return "Two";
  //   }
  //   return "Other";
  // }
  .method public hidebysig static string  Main(int32 a) cil managed
  {
    .maxstack  1
    IL_0000:  ldarg.0
    IL_0001:  switch     (IL_0014, IL_001a, IL_0020)
    IL_0012:  br.s       IL_0026
    IL_0014:  ldstr      "Zero"
    IL_0019:  ret
    IL_001a:  ldstr      "One"
    IL_001f:  ret
    IL_0020:  ldstr      "Two"
    IL_0025:  ret
    IL_0026:  ldstr      "Other"
    IL_002b:  ret
  }  
}
