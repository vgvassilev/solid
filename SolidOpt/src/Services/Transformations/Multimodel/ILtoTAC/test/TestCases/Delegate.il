// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  //  public delegate int PerformCalculation(int x, int y);
  //
  //  public int M(int x, int y) {
  //    return x + y;
  //  }
  //
  //  public static int sM(int x, int y) {
  //    return x - y;
  //  }
  //
  //  public virtual int vM(int x, int y) {
  //    return x * y;
  //  }
  //
  //  public event PerformCalculation OnCalc;
  //
  //  public static void Main() {
  //    TestCase test = new TestCase();
  //
  //    PerformCalculation calc = test.M;
  //    System.Console.WriteLine(calc(1,2));
  //
  //    test.OnCalc += test.M;
  //    test.OnCalc += sM;
  //    test.OnCalc += test.vM;
  //    if (test.OnCalc != null)
  //      System.Console.WriteLine(test.OnCalc(3,4));
  //  }

  .field  private  class TestCase/PerformCalculation OnCalc

  .method public hidebysig specialname rtspecialname instance default void '.ctor'() cil managed
  {
    IL_0000:  ldarg.0
    IL_0001:  call instance void object::'.ctor'()
    IL_0006:  ret
  }

  .method public hidebysig specialname instance default void add_OnCalc(class TestCase/PerformCalculation 'value') cil managed
  {
    .locals init (class TestCase/PerformCalculation V_0, class TestCase/PerformCalculation V_1)

    IL_0000:  ldarg.0
    IL_0001:  ldfld class TestCase/PerformCalculation TestCase::OnCalc
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldarg.0
    IL_000a:  ldflda class TestCase/PerformCalculation TestCase::OnCalc
    IL_000f:  ldloc.1
    IL_0010:  ldarg.1
    IL_0011:  call class [mscorlib]System.Delegate class [mscorlib]System.Delegate::Combine(class [mscorlib]System.Delegate, class [mscorlib]System.Delegate)
    IL_0016:  castclass TestCase/PerformCalculation
    IL_001b:  ldloc.0
    IL_001c:  call !!0 class [mscorlib]System.Threading.Interlocked::CompareExchange<class TestCase/PerformCalculation> ([out] !!0&, !!0, !!0)
    IL_0021:  stloc.0
    IL_0022:  ldloc.0
    IL_0023:  ldloc.1
    IL_0024:  call bool class [mscorlib]System.Delegate::op_Inequality(class [mscorlib]System.Delegate, class [mscorlib]System.Delegate)
    IL_0029:  brtrue IL_0007
    IL_002e:  ret
  }

  .method public hidebysig specialname instance default void remove_OnCalc(class TestCase/PerformCalculation 'value') cil managed
  {
    .locals init (class TestCase/PerformCalculation V_0, class TestCase/PerformCalculation V_1)
    IL_0000:  ldarg.0
    IL_0001:  ldfld class TestCase/PerformCalculation TestCase::OnCalc
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  stloc.1
    IL_0009:  ldarg.0
    IL_000a:  ldflda class TestCase/PerformCalculation TestCase::OnCalc
    IL_000f:  ldloc.1
    IL_0010:  ldarg.1
    IL_0011:  call class [mscorlib]System.Delegate class [mscorlib]System.Delegate::Remove(class [mscorlib]System.Delegate, class [mscorlib]System.Delegate)
    IL_0016:  castclass TestCase/PerformCalculation
    IL_001b:  ldloc.0
    IL_001c:  call !!0 class [mscorlib]System.Threading.Interlocked::CompareExchange<class TestCase/PerformCalculation> ([out] !!0&, !!0, !!0)
    IL_0021:  stloc.0
    IL_0022:  ldloc.0
    IL_0023:  ldloc.1
    IL_0024:  call bool class [mscorlib]System.Delegate::op_Inequality(class [mscorlib]System.Delegate, class [mscorlib]System.Delegate)
    IL_0029:  brtrue IL_0007
    IL_002e:  ret
  }

  .method public hidebysig instance default int32 M(int32 x, int32 y) cil managed
  {
    IL_0000:  ldarg.1
    IL_0001:  ldarg.2
    IL_0002:  add
    IL_0003:  ret
  }

  .method public static hidebysig default int32 sM(int32 x, int32 y) cil managed
  {
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  sub
    IL_0003:  ret
  }

  .method public virtual hidebysig newslot instance default int32 vM(int32 x, int32 y) cil managed
  {
    IL_0000:  ldarg.1
    IL_0001:  ldarg.2
    IL_0002:  mul
    IL_0003:  ret
  }

  .method public static hidebysig default void Main() cil managed
  {
    .entrypoint
    .maxstack 17
    .locals init (class TestCase V_0, class TestCase/PerformCalculation V_1)

    IL_0000:  newobj instance void class TestCase::'.ctor'()
    IL_0005:  stloc.0
    IL_0006:  ldloc.0
    IL_0007:  ldftn instance int32 class TestCase::M(int32, int32)
    IL_000d:  newobj instance void class TestCase/PerformCalculation::'.ctor'(object, native int)
    IL_0012:  stloc.1
    IL_0013:  ldloc.1
    IL_0014:  ldc.i4.1
    IL_0015:  ldc.i4.2
    IL_0016:  callvirt instance int32 class TestCase/PerformCalculation::Invoke(int32, int32)
    IL_001b:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_0020:  ldloc.0
    IL_0021:  ldloc.0
    IL_0022:  ldftn instance int32 class TestCase::M(int32, int32)
    IL_0028:  newobj instance void class TestCase/PerformCalculation::'.ctor'(object, native int)
    IL_002d:  callvirt instance void class TestCase::add_OnCalc(class TestCase/PerformCalculation)
    IL_0032:  ldloc.0
    IL_0033:  ldnull
    IL_0034:  ldftn int32 class TestCase::sM(int32, int32)
    IL_003a:  newobj instance void class TestCase/PerformCalculation::'.ctor'(object, native int)
    IL_003f:  callvirt instance void class TestCase::add_OnCalc(class TestCase/PerformCalculation)
    IL_0044:  ldloc.0
    IL_0045:  ldloc.0
    IL_0046:  dup
    IL_0047:  ldvirtftn instance int32 class TestCase::vM(int32, int32)
    IL_004d:  newobj instance void class TestCase/PerformCalculation::'.ctor'(object, native int)
    IL_0052:  callvirt instance void class TestCase::add_OnCalc(class TestCase/PerformCalculation)
    IL_0057:  ldloc.0
    IL_0058:  ldfld class TestCase/PerformCalculation TestCase::OnCalc
    IL_005d:  brfalse IL_0074
    IL_0062:  ldloc.0
    IL_0063:  ldfld class TestCase/PerformCalculation TestCase::OnCalc
    IL_0068:  ldc.i4.3
    IL_0069:  ldc.i4.4
    IL_006a:  callvirt instance int32 class TestCase/PerformCalculation::Invoke(int32, int32)
    IL_006f:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_0074:  ret
  }

  .event TestCase/PerformCalculation OnCalc
  {
    .addon instance default void TestCase::add_OnCalc(class TestCase/PerformCalculation 'value')
    .removeon instance default void TestCase::remove_OnCalc(class TestCase/PerformCalculation 'value')
  }

  .class nested public auto ansi sealed PerformCalculation extends [mscorlib]System.MulticastDelegate
  {
    .method public hidebysig specialname rtspecialname instance default void '.ctor'(object 'object', native int 'method') runtime managed
    {
      // Disassembly of native methods is not supported
    }

    .method public virtual hidebysig newslot instance default int32 Invoke(int32 x, int32 y) runtime managed
    {
      // Disassembly of native methods is not supported
    }

    .method public virtual hidebysig newslot instance default class [mscorlib]System.IAsyncResult BeginInvoke(int32 x, int32 y, class [mscorlib]System.AsyncCallback callback, object 'object') runtime managed
    {
      // Disassembly of native methods is not supported
    }

    .method public virtual hidebysig newslot instance default int32 EndInvoke(class [mscorlib]System.IAsyncResult result) runtime managed
    {
      // Disassembly of native methods is not supported
    }

  }

}
