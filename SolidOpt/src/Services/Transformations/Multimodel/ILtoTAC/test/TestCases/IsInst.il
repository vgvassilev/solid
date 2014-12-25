// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{
  // public static void Main() {
  // object m = new String('a',1);
  //   if (m is String)
  //     Console.WriteLine(m as String);
  // }
  .method public hidebysig static void  Main() cil managed
  {
    .maxstack 3
    .locals init (object  V_0)
    IL_0000:  ldc.i4.s 0x61
    IL_0002:  ldc.i4.1
    IL_0003:  newobj instance void string::'.ctor'(char, int32)
    IL_0008:  stloc.0
    IL_0009:  ldloc.0
    IL_000a:  isinst [mscorlib]System.String
    IL_000f:  brfalse IL_001f

    IL_0014:  ldloc.0
    IL_0015:  isinst [mscorlib]System.String
    IL_001a:  call void class [mscorlib]System.Console::WriteLine(string)
    IL_001f:  ret
  }
}

