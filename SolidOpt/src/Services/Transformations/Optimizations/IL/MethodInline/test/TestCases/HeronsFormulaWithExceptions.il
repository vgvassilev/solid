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
    L_0001: ldc.r8 3
    L_000a: ldc.r8 3
    L_0013: ldc.r8 3
    L_001c: call float64 TestCase::CalculateAreaHeron(float64, float64, float64)
    L_0021: pop 
    L_0022: ret 
  }
  
  .method public hidebysig static float64 CalculateAreaHeron(float64 a, float64 b, float64 c) cil managed
  {
    .custom instance void [SolidOpt.Services.Optimizations.Annotations]SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.InlineableAttribute::.ctor() = (
      01 00 00 00
    )
    .try { 
      IL_0000 : ldarg.0
      IL_0001 : ldc.r8 0
      IL_000a : beq IL_002d 
      IL_000f : ldarg.1
      IL_0010 : ldc.r8 0
      IL_0019 : beq IL_002d 
      IL_001e : ldarg.2
      IL_001f : ldc.r8 0
      IL_0028 : bne.un IL_003c 
      IL_002d : ldc.r8 0
      IL_0036 : stloc.0
      IL_0037 : leave IL_007e 
      IL_003c : ldarg.0
      IL_003d : ldarg.1
      IL_003e : add
      IL_003f : ldarg.2
      IL_0040 : add
      IL_0041 : ldc.r8 2
      IL_004a : div
      IL_004b : stloc.1
      IL_004c : ldloc.1
      IL_004d : ldloc.1
      IL_004e : ldarg.0
      IL_004f : sub
      IL_0050 : mul
      IL_0051 : ldloc.1
      IL_0052 : ldarg.1
      IL_0053 : sub
      IL_0054 : mul
      IL_0055 : ldloc.1
      IL_0056 : ldarg.2
      IL_0057 : sub
      IL_0058 : mul
      IL_0059 : call float64 [mscorlib]System.Math::Sqrt(float64)
      IL_005e : stloc.0
      IL_005f : leave IL_007e 
      IL_0064 : leave IL_007e 
    } 
    catch [mscorlib]System.Exception {
      IL_0069 : pop
      IL_006a : ldc.r8 -1
      IL_0073 : stloc.0
      IL_0074 : leave IL_007e 
      IL_0079 : leave IL_007e 
    } 
    IL_007e : ldloc.0
    IL_007f : ret
  }
}
