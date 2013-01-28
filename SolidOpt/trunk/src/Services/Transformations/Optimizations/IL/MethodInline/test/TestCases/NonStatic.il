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
    .locals init (TestThis V_0, int32 V_1)
    IL_0000 : ldstr "test"
    IL_0005 : call void [mscorlib]System.Console::WriteLine([mscorlib]System.String)
    IL_000a : newobj System.Void TestThis::.ctor()
    IL_000f : stloc.0
    IL_0010 : ldloc.0
    IL_0011 : callvirt int32 TestThis::GetNumber()
    IL_0016 : stloc.1
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
//    public int GetNumber()
//    {
//      Console.WriteLine("This is this");
//      return number;
//    }
// }
.class private auto ansi beforefieldinit TestThis
  extends [mscorlib]System.Object
{
  // Fields
  .field private static int32 number

  // Methods  
  .method public hidebysig instance int32 GetNumber () cil managed 
  {
    .custom instance void [SolidOpt.Services.Optimizations.Annotations]SolidOpt.Services.Transformations.Optimizations.IL.MethodInline.InlineableAttribute::.ctor() = (
      01 00 00 00
    )
    IL_0000 : ldstr "This is this"
    IL_0005 : call void [mscorlib]System.Console::WriteLine([mscorlib]System.String)
    IL_000a : ldsfld int32 TestThis::number
    IL_000f : ret
  }
  .method public hidebysig specialname rtspecialname instance void .ctor () cil managed 
  {
    IL_0000: ldarg.0
    IL_0001: call instance void [mscorlib]System.Object::.ctor()
    IL_0006: ret
  }

  .method private specialname rtspecialname static void .cctor () cil managed 
  {
    IL_0000: ret
  }
}
