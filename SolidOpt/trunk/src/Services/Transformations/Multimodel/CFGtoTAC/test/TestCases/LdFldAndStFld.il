// RUN: "@ILASM@" /DLL @TEST_CASE@
// XFAIL:*
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
  //   Console.WriteLine(my.NonStaticFld);
  // }
  .method public hidebysig static void Main() cil managed
  {
    .maxstack 2
    .locals init (class TestCase/MyClass V_0)
    IL_0000:  newobj instance void class TestCase/MyClass::'.ctor'()
    IL_0005:  stloc.0
    IL_0006:  ldloc.0
    IL_0007:  ldfld int32 TestCase/MyClass::NonStaticFld
    IL_000c:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_0011:  ret
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

