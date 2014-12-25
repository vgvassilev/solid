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


  // public static void Main() {
  //   int[] simpleArray;
  //   simpleArray = new int[10];
  //   Object[] o = new Object[5];
  //   int[][] jaggedArray = new int[2][];
  //   int[,] matrixArray = new int[3,4];
  //   int[][][][][] bigJaggedArray = new int[2][][][][];
  //   int[,,,,,,] bigMatrixArray = new int[3,5,2,4,5,4,6];
  // }
  
//  L0: T_0[] = new int[10]
//  L1: T_1[] // FIXME: Should we keep it like that or we ignore, because it was unused.
//  L2: T_2[][] = new int[2][]
//  L3: T_3[,] = new int[3,4]
//  L4: T_4[][][][][] = new int[2][][][][]
//  L5: T_5[,,,,,,] = new int[3,5,2,4,5,4,6]
//  return

  .method public hidebysig static void  Main() cil managed
  {
    .maxstack 11
    .locals init (int32[] V_0, object[]  V_1, int32[][] V_2, int32[,]  V_3, int32[][][][][] V_4, int32[,,,,,,] V_5)
    IL_0000:  ldc.i4.s 0x0a
    IL_0002:  newarr [mscorlib]System.Int32
    IL_0007:  stloc.0
    IL_0008:  ldc.i4.s 0x05
    IL_000a:  newarr [mscorlib]System.Object
    IL_000f:  stloc.1
    IL_0010:  ldc.i4.2
    IL_0011:  newarr int32[]
    IL_0016:  stloc.2
    IL_0017:  ldc.i4.3
    IL_0018:  ldc.i4.4
    IL_0019:  newobj instance void int32[,]::'.ctor'(int32, int32)
    IL_001e:  stloc.3
    IL_001f:  ldc.i4.2
    IL_0020:  newarr int32[][][][]
    IL_0025:  stloc.s 4
    IL_0027:  ldc.i4.3
    IL_0028:  ldc.i4.5
    IL_0029:  ldc.i4.2
    IL_002a:  ldc.i4.4
    IL_002b:  ldc.i4.5
    IL_002c:  ldc.i4.4
    IL_002d:  ldc.i4.6
    IL_002e:  newobj instance void int32[,,,,,,]::'.ctor'(int32, int32, int32, int32, int32, int32, int32)
    IL_0033:  stloc.s 5
    IL_0035:  ret
  }
}
