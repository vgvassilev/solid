// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{

  .method public hidebysig specialname rtspecialname instance default void '.ctor'() cil managed
  {
    ldarg.0
    call instance void object::'.ctor'()
    ret
  }

  .method public hidebysig static void sM1(int32 a, string b) cil managed
  {
    ldarg.1
    call void [mscorlib]System.Console::Write(string)
    ldarg.0
    call void [mscorlib]System.Console::WriteLine(int32)
    ret
  }

  .method public hidebysig static int32 sM2(int32 a) cil managed
  {
    ldarg.0
    ret
  }

  .method public hidebysig static void sM3(int32 a, string b, int32 c) cil managed
  {
    ldarg.1
    call void [mscorlib]System.Console::Write(string)
    ldarg.0
    ldarg.2
    add
    call void [mscorlib]System.Console::WriteLine(int32)
    ret
  }

  .method public hidebysig instance int32 M1(int32 a) cil managed
  {
    ldarg.1
    ret
  }

  .method public virtual hidebysig newslot instance default int32 vM1(int32 a) cil managed
  {
    IL_0000:  ldarg.1
    IL_0003:  ret
  }


  .method public hidebysig static void Main() cil managed
  {
    .entrypoint
    .locals init (class TestCase V_0)

    // Normal static method call
    ldc.i4.0
    ldstr "Value:"
    call void TestCase::sM1(int32,string)

    // Indirect static method call - simple params
    ldc.i4.1
    ldstr "Value:"
    ldftn void TestCase::sM1(int32,string)
    calli void (int32,string)

    // Indirect static method call
    ldc.i4.2
    ldc.i4.3
    add
    ldstr "Value:"
    ldc.i4.2
    ldc.i4.3
    mul
    ldftn void TestCase::sM3(int32,string,int32)
    calli void (int32,string,int32)

    // Indirect static method call with result
    ldc.i4.4
    ldftn int32 TestCase::sM2(int32)
    calli int32 (int32)
    call void [mscorlib]System.Console::WriteLine(int32)

    // Create new instance
    newobj instance void class TestCase::'.ctor'()
    stloc.0

    // Indirect instance method call with result
    ldloc.0
    ldc.i4.5
    ldftn int32 TestCase::M1(int32)
    calli instance int32 (int32)
    call void [mscorlib]System.Console::WriteLine(int32)

    // Indirect virtual method call with result
    ldloc.0
    ldc.i4.6
    ldloc.0
    ldvirtftn instance int32 TestCase::vM1(int32)
    calli instance int32 (int32)
    call void [mscorlib]System.Console::WriteLine(int32)

    ret
  }

}
