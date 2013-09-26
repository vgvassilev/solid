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
  //   public static string StaticFld = "a";
  // }
  // public static void Main() {
  //   MyClass.StaticFld = "b";
  //   Console.WriteLine(MyClass.StaticFld);
  // }
  .method public hidebysig static void Main() cil managed
  {
    .maxstack 8
    IL_0000:  ldstr "b"
    IL_0005:  stsfld string TestCase/MyClass::StaticFld
    IL_000a:  ldsfld string TestCase/MyClass::StaticFld
    IL_000f:  call void class [mscorlib]System.Console::WriteLine(string)
    IL_0014:  ret
  }
  
  .class nested private auto ansi beforefieldinit MyClass
    extends [mscorlib]System.Object
  {
    .field  public static string StaticFld

    .method public hidebysig specialname rtspecialname 
           instance default void '.ctor' ()  cil managed 
    {
      .maxstack 8
      IL_0000:  ldarg.0
      IL_0001:  call instance void object::'.ctor'()
      IL_0006:  ret
    } // end of method MyClass::.ctor

    .method private static specialname rtspecialname 
           default void '.cctor' ()  cil managed 
    {
      .maxstack 8
      IL_0000:  ldstr "a"
      IL_0005:  stsfld string TestCase/MyClass::StaticFld
      IL_000a:  ret
    } // end of method MyClass::.cctor
  } // end of class MyClass
} // end of class TestCase
