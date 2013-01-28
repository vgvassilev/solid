// XFAIL:
.assembly extern System.Exception {}
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
  extends [mscorlib]System.Object
{
  //  public static void Main(string[] args) {
  //    Console.WriteLine("test");
  //    TestThis.Inc();
  //    int i = TestThis.Inc(1);
  //    Console.ReadKey(true);
  //  }

  .method public hidebysig static void Main(string[] args) cil managed
  {
    IL_0000 : ldstr "test"
    IL_0005 : call void [mscorlib]System.Console::WriteLine([mscorlib]System.String)
    IL_000a : call int32 TestThis::Inc()
    IL_000f : pop
    IL_0010 : ldc.i4.1
    IL_0011 : call int32 TestThis::Inc(int32)
    IL_0016 : stloc.0
    IL_0017 : ldc.i4.1
    IL_0018 : call [mscorlib]System.ConsoleKeyInfo [mscorlib]System.Console::ReadKey(bool)
    IL_001d : pop
    IL_001e : ret
  }
}

//  class TestThis
//  {
//    private static int number = 0;
//    
//    [Inlineable]
//    public static int Inc ()
//    {
//      return number + 1;
//    }
//    
//    [Inlineable]
//    public static int Inc (int number)
//    {
//      return (TestThis.number + number + 1) * (number + TestThis.number + 1);
//    }
//  }
.class private auto ansi beforefieldinit TestThis
  extends [mscorlib]System.Object
{
  // Fields
  .field private static int32 number

  // Methods
  .method public hidebysig specialname rtspecialname 
    instance void .ctor () cil managed 
  {
    IL_0000: ldarg.0
    IL_0001: call instance void [mscorlib]System.Object::.ctor()
    IL_0006: ret
  }

  .method private specialname rtspecialname static 
    void .cctor () cil managed 
  {
    IL_0000: ret
  }

  .method public hidebysig static int32 Inc () cil managed 
  {
    .custom instance void [SolidOpt.Services.Optimizations.Annotations]SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.InlineableAttribute::.ctor() = (
      01 00 00 00
    )
    IL_0000: ldsfld int32 TestThis::number
    IL_0005: ldc.i4.1
    IL_0006: add
    IL_0007: ret
  }

    
  .method public hidebysig static int32 Inc (int32 number) cil managed 
  {
    .custom instance void [SolidOpt.Services.Optimizations.Annotations]SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.InlineableAttribute::.ctor() = (
      01 00 00 00
    )
    IL_0000: ldsfld int32 TestThis::number
    IL_0005: ldarg.0
    IL_0006: add
    IL_0007: ldc.i4.1
    IL_0008: add
    IL_0009: ldarg.0
    IL_000a: ldsfld int32 TestThis::number
    IL_000f: add
    IL_0010: ldc.i4.1
    IL_0011: add
    IL_0012: mul
    IL_0013: ret
  }
}
