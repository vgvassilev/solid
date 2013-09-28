// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}
.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  .method public hidebysig specialname rtspecialname 
          instance default void '.ctor' ()  cil managed 
   {
     .maxstack 8
     IL_0000:  ldarg.0 
     IL_0001:  call instance void object::'.ctor'()
     IL_0006:  ret 
   } // end of method TestCase::.ctor

  // class MyClass {
  //   public int NonStaticFld = 0;
  // }
  // public static void Main() {
  //   MyClass my = new MyClass();
  //   Console.WriteLine(my.NonStaticFld++);
  // }
  .method public hidebysig static void Main() cil managed
  {
    .maxstack 3
    .locals init (class TestCase/MyClass V_0, int32 V_1)
    IL_0000:  nop
    IL_0001:  newobj instance void class TestCase/MyClass::'.ctor'()
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  dup
    IL_0009:  ldfld int32 TestCase/MyClass::NonStaticFld
    IL_000e:  ldc.i4.1
    IL_000f:  add
    IL_0010:  dup
    IL_0011:  stloc.1
    IL_0012:  stfld int32 TestCase/MyClass::NonStaticFld
    IL_0017:  ldloc.1
    IL_0018:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_001d:  ret
  }
  
  .class nested private auto ansi beforefieldinit MyClass
    extends [mscorlib]System.Object
  {
    .field  public static  string StaticFld

    .method public hidebysig specialname rtspecialname 
           instance default void '.ctor' () cil managed 
    {
      .maxstack 8
      IL_0000:  ldarg.0
      IL_0001:  ldc.i4.0
      IL_0002:  stfld int32 TestCase/MyClass::NonStaticFld
      IL_0007:  ldarg.0
      IL_0008:  call instance void object::'.ctor'()
      IL_000d:  ret
    } // end of method MyClass::.ctor
  } // end of class MyClass
} // end of class TestCase
