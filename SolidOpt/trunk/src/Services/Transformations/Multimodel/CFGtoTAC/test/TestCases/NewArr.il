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
   } // end of method MainClass::.ctor


  // public static void Main() {
  //   int[] simpleArray;
  //   simpleArray = new int[10];
  //   Object[] o;
  //   int[][] jaggedArray = new int[2][];
  //   int[,] matrixArray = new int[3,4];
  //   int[][][][][] bigJaggedArray = new int[2][][][][];
  //   int[,,,,,,] bigMatrixArray = new int[3,5,2,4,5,4,6];
  // }
  .method public hidebysig static void  Main() cil managed
  {
    .maxstack 11
    .locals init (int32[] V_0, object[]  V_1, int32[][] V_2, int32[,]  V_3, int32[][][][][] V_4, int32[,,,,,,] V_5)
    IL_0000:  ldc.i4.s 0x0a
    IL_0002:  newarr [mscorlib]System.Int32
    IL_0007:  stloc.0
    IL_0008:  ldc.i4.2
    IL_0009:  newarr int32[]
    IL_000e:  stloc.2
    IL_000f:  ldc.i4.3
    IL_0010:  ldc.i4.4
    IL_0011:  newobj instance void int32[,]::'.ctor'(int32, int32)
    IL_0016:  stloc.3
    IL_0017:  ldc.i4.2
    IL_0018:  newarr int32[][][][]
    IL_001d:  stloc.s 4
    IL_001f:  ldc.i4.3
    IL_0020:  ldc.i4.5
    IL_0021:  ldc.i4.2
    IL_0022:  ldc.i4.4
    IL_0023:  ldc.i4.5
    IL_0024:  ldc.i4.4
    IL_0025:  ldc.i4.6
    IL_0026:  newobj instance void int32[,,,,,,]::'.ctor'(int32, int32, int32, int32, int32, int32, int32)
    IL_002b:  stloc.s 5
    IL_002d:  ret
  }
}
