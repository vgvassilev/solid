.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static string Main(int a) {
  //   string result = "";
  //   do {
  //     switch (a--) {
  //       case 0: result = "Zero"; break;
  //       case 1: result = "One"; continue;
  //       case 2: result = "Two"; break;
  //       case 6: result = "Six"; break;
  //       case 7: result = "Seven"; break;
  //     }
  //     result = "!";
  //   } while (a > 0);
  //   return result;
  // }
  .method public hidebysig static string  Main(int32 a) cil managed
  {
    .maxstack  3
    .locals init (
            string V_0,
            int32 V_1)
    IL_0000:  ldstr      ""
    IL_0005:  stloc.0
    IL_0006:  ldarg.0
    IL_0007:  dup
    IL_0008:  ldc.i4.1
    IL_0009:  sub
    IL_000a:  starg.s    a
    IL_000c:  stloc.1
    IL_000d:  ldloc.1
    IL_000e:  switch (IL_0035, IL_003d, IL_0045, IL_005b, IL_005b, IL_005b, IL_004d, IL_0055)
    IL_0033:  br.s       IL_005b
    IL_0035:  ldstr      "Zero"
    IL_003a:  stloc.0
    IL_003b:  br.s       IL_005b
    IL_003d:  ldstr      "One"
    IL_0042:  stloc.0
    IL_0043:  br.s       IL_0061
    IL_0045:  ldstr      "Two"
    IL_004a:  stloc.0
    IL_004b:  br.s       IL_005b
    IL_004d:  ldstr      "Six"
    IL_0052:  stloc.0
    IL_0053:  br.s       IL_005b
    IL_0055:  ldstr      "Seven"
    IL_005a:  stloc.0
    IL_005b:  ldstr      "!"
    IL_0060:  stloc.0
    IL_0061:  ldarg.0
    IL_0062:  ldc.i4.0
    IL_0063:  bgt.s      IL_0006
    IL_0065:  ldloc.0
    IL_0066:  ret
  }  
}
