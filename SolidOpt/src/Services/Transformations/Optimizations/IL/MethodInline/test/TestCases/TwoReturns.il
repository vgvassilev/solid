// RUN: "@ILASM@" /DLL @TEST_CASE@
// XFAIL:
.assembly extern System.Exception {}
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
  extends [mscorlib]System.Object
{
  //  public static void Main(string[] args) {
  //    CalculateAreaHeron(3, 3, 3);
  //  }
  //  [Inlineable]
  //  public static double CalculateAreaHeron(double a, double b, double c)
  //  {
  //    try {
  //      if (a==0 || b==0 || c==0) return 0;
  //      
  //      double p = (a + b + c) / 2 ;
  //      return Math.Sqrt(p * (p-a) * (p-b) * (p-c));
  //    }
  //    catch {
  //      return -1;
  //    }
  //  }
  .method public hidebysig static void Main(string[] args) cil managed
  {
    .entrypoint
    .maxstack 8
    L_0000: nop 
    L_0001: ldc.i4 1
    L_000a: ldc.i4 1
    L_0013: ldc.r8 3
    L_001c: call float64 TestCase::CalculateAreaHeron(float64, float64, float64)
    L_0021: pop 
    L_0022: ret 
  }
  
  .method public hidebysig static int32 Inlinee(int32 p, int32 q) cil managed
  {
    .custom instance void [SolidOpt.Services.Optimizations.Annotations]SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.InlineableAttribute::.ctor() = (
      01 00 00 00
    )
    .locals init ([mscorlib]System.Byte V_0, int32 V_1)
    IL_0000 : ldc.i4.5
    IL_0001 : stloc.0
    IL_0002 : ldc.i4.2
    IL_0003 : stloc.1
    IL_0004 : ldc.i4.5
    IL_0005 : starg.s p
    IL_0007 : ldc.i4.6
    IL_0008 : starg.s q
    IL_000a : ldloc.0
    IL_000b : ldc.i4.6
    IL_000c : bne.un IL_001b 
    IL_0011 : ldloc.0
    IL_0012 : call void [mscorlib]System.Console::WriteLine(int32)
    IL_0017 : ldarg.0
    IL_0018 : ldarg.1
    IL_0019 : add
    IL_001a : ret
    IL_001b : ldarg.0
    IL_001c : ldarg.1
    IL_001d : add
    IL_001e : call void [mscorlib]System.Console::WriteLine(int32)
    IL_0023 : ldc.i4.s 10
    IL_0025 : ret
  }
}
