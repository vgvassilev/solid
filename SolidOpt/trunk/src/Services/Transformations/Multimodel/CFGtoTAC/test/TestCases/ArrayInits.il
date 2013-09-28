// RUN: "@ILASM@" /DLL @TEST_CASE@
//TODO: We should have a look how array inits are implemented in .NET too.
.assembly TestCase {
  .custom instance void class [mscorlib]System.Reflection.AssemblyTitleAttribute::'.ctor'(string) =  (01 00 05 54 65 73 74 73 00 00 )
  .custom instance void class [mscorlib]System.Reflection.AssemblyDescriptionAttribute::'.ctor'(string) =  (01 00 00 00 00 )
  .custom instance void class [mscorlib]System.Reflection.AssemblyConfigurationAttribute::'.ctor'(string) =  (01 00 00 00 00 )
  .custom instance void class [mscorlib]System.Reflection.AssemblyCompanyAttribute::'.ctor'(string) =  (01 00 00 00 00 )
  .custom instance void class [mscorlib]System.Reflection.AssemblyProductAttribute::'.ctor'(string) =  (01 00 00 00 00 )
  .custom instance void class [mscorlib]System.Reflection.AssemblyCopyrightAttribute::'.ctor'(string) =  (01 00 09 76 76 61 73 73 69 6C 65 76 00 00 )
  .custom instance void class [mscorlib]System.Reflection.AssemblyTrademarkAttribute::'.ctor'(string) =  (01 00 00 00 00 )

  .custom instance void class [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::'.ctor'() =  (
    01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
    63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01       ) // ceptionThrows.

  .hash algorithm 0x00008004
  .ver  1:0:5016:32142
}

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


  // public static void Main() {
  //   int[] simpleArray = new int[2] {1,2};
  //   int[][] arr = new int[2][] {new int[] {1,2,3}, new int[] {4,5,6}};
  //   int[,] m = { {1, 2}, {3, 4} };
  // }
  .method public hidebysig static void  Main() cil managed
  {
    .maxstack 9
    .locals init (int32[] V_0, int32[][] V_1, int32[,]  V_2)
    IL_0000:  ldc.i4.2
    IL_0001:  newarr [mscorlib]System.Int32
    IL_0006:  dup
    IL_0007:  ldc.i4.0
    IL_0008:  ldc.i4.1
    IL_0009:  stelem.i4
    IL_000a:  dup
    IL_000b:  ldc.i4.1
    IL_000c:  ldc.i4.2
    IL_000d:  stelem.i4
    IL_000e:  stloc.0
    IL_000f:  ldc.i4.2
    IL_0010:  newarr int32[]
    IL_0015:  dup
    IL_0016:  ldc.i4.0
    IL_0017:  ldc.i4.3
    IL_0018:  newarr [mscorlib]System.Int32
    IL_001d:  dup
    IL_001e:  ldtoken field valuetype '<PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}'/'$ArrayType=12' '<PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}'::'$field-0'
    IL_0023:  call void class [mscorlib]System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(class [mscorlib]System.Array, valuetype [mscorlib]System.RuntimeFieldHandle)
    IL_0028:  stelem.ref
    IL_0029:  dup
    IL_002a:  ldc.i4.1
    IL_002b:  ldc.i4.3
    IL_002c:  newarr [mscorlib]System.Int32
    IL_0031:  dup
    IL_0032:  ldtoken field valuetype '<PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}'/'$ArrayType=12' '<PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}'::'$field-1'
    IL_0037:  call void class [mscorlib]System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(class [mscorlib]System.Array, valuetype [mscorlib]System.RuntimeFieldHandle)
    IL_003c:  stelem.ref
    IL_003d:  stloc.1
    IL_003e:  ldc.i4.2
    IL_003f:  ldc.i4.2
    IL_0040:  newobj instance void int32[,]::'.ctor'(int32, int32)
    IL_0045:  dup
    IL_0046:  ldtoken field valuetype '<PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}'/'$ArrayType=16' '<PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}'::'$field-2'
    IL_004b:  call void class [mscorlib]System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(class [mscorlib]System.Array, valuetype [mscorlib]System.RuntimeFieldHandle)
    IL_0050:  stloc.2
    IL_0051:  ret
  }
}

.class private auto ansi abstract sealed beforefieldinit '<PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}'
    extends [mscorlib]System.Object
{
  .custom instance void class [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::'.ctor'() =  (01 00 00 00 ) // ....

  .field  assembly static  valuetype '<PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}'/'$ArrayType=12' '$field-0' at D_00002154
  .field  assembly static  valuetype '<PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}'/'$ArrayType=12' '$field-1' at D_00002160
  .field  assembly static  valuetype '<PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}'/'$ArrayType=16' '$field-2' at D_0000216c

  .class nested private explicit ansi sealed beforefieldinit '$ArrayType=12'
    extends [mscorlib]System.ValueType
  {
    .pack 1
    .size 12
  } // end of class $ArrayType=12

  .class nested private explicit ansi sealed beforefieldinit '$ArrayType=16'
    extends [mscorlib]System.ValueType
  {
    .pack 1
    .size 16

  } // end of class $ArrayType=16

  } // end of class <PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}

.data D_00002154 = bytearray (01 00 00 00 02 00 00 00 03 00 00 00) // size: 12
.data D_00002160 = bytearray (04 00 00 00 05 00 00 00 06 00 00 00) // size: 12
.data D_0000216c = bytearray (01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00) // size: 16
