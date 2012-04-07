.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
 // public static void Main(string[] args) {
 //   int x = 5;
 //   do {
 //     Console.WriteLine(x.ToString());
 //     x--;
 //   } while (x > 0);
 // }
 .method public hidebysig static bool Main(int32 x, int32 y) cil managed
 {
    .entrypoint
    .maxstack 2
    .locals init (
        [0] int32 x,
        [1] bool CS$4$0000)
    IL_0000: nop
    IL_0001: ldc.i4.5
    IL_0002: stloc.0
    IL_0003: nop
    IL_0004: ldloca.s x
    IL_0006: call instance string [mscorlib]System.Int32::ToString()
    IL_000b: call void [mscorlib]System.Console::WriteLine(string)
    IL_0010: nop 
    IL_0011: ldloc.0
    IL_0012: ldc.i4.1
    IL_0013: sub.ovf 
    IL_0014: stloc.0 
    IL_0015: nop 
    IL_0016: ldloc.0 
    IL_0017: ldc.i4.0 
    IL_0018: cgt 
    IL_001a: stloc.1 
    IL_001b: ldloc.1 
    IL_001c: brtrue.s IL_0003
    IL_001e: ret
 }
}