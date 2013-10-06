// RUN: "@ILASM@" /DLL @TEST_CASE@
.assembly TestCase {}

.class public auto ansi beforefieldinit TestCase
       extends [mscorlib]System.Object
{

  //  public static void Main() {
  //    SByte i8 = 8;
  //    Int16 i16 = 16;
  //    Int32 i32 = 32;
  //    Int64 i64 = 64;
  //    Byte u8 = 8;
  //    UInt16 u16 = 16;
  //    UInt32 u32 = 32;
  //    UInt64 u64 = 64;
  //    IntPtr intptr = new IntPtr();
  //    Single s = 1.1f;
  //    Double d = 2.2;
  //    DateTime dt = new DateTime();
  //    dt = new DateTime(2013, 10, 4);
  //    Object obj = new Object();
  //    Random rnd = new Random();
  //    String str = "test";
  //
  //    ByRef(ref i8, ref i16, ref i32, ref i64, ref u8, ref u16, ref u32, ref u64, ref intptr, ref s, ref d, ref dt, ref obj, ref rnd, ref str);
  //    ByRef(ref i8, ref i16, ref i32, ref i64, ref u8, ref u16, ref u32, ref u64, ref intptr, ref s, ref d, ref dt, ref obj, ref rnd, ref str);
  //  }
  //
  //  public static void ByRef(ref SByte i8, ref Int16 i16, ref Int32 i32, ref Int64 i64,
  //                           ref Byte u8, ref UInt16 u16, ref UInt32 u32, ref UInt64 u64,
  //                           ref IntPtr intptr, ref Single s, ref Double d,
  //                           ref DateTime dt, ref Object obj, ref Random rnd, ref String str) {
  //    System.Console.WriteLine(i8);
  //    System.Console.WriteLine(i16);
  //    System.Console.WriteLine(i32);
  //    System.Console.WriteLine(i64);
  //    System.Console.WriteLine(u8);
  //    System.Console.WriteLine(u16);
  //    System.Console.WriteLine(u32);
  //    System.Console.WriteLine(u64);
  //    System.Console.WriteLine(intptr);
  //    System.Console.WriteLine(s);
  //    System.Console.WriteLine(d);
  //    System.Console.WriteLine(dt);
  //    System.Console.WriteLine(obj);
  //    System.Console.WriteLine(rnd);
  //    System.Console.WriteLine(str);
  //
  //    i8 = -8;
  //    i16 = -16;
  //    i32 = -32;
  //    i64 = -64;
  //    u8 = 8;
  //    u16 = 16;
  //    u32 = 32;
  //    u64 = 64;
  //    intptr = new IntPtr();
  //    s = -1.1f;
  //    d = -2.2;
  //    dt = new DateTime();
  //    obj = new Object();
  //    rnd = new Random();
  //    str = "best";
  //  }

  .method public static hidebysig default void Main() cil managed
  {
    .entrypoint
    .maxstack 19
    .locals init (int8 V_0, int16 V_1, int32 V_2, int64 V_3, unsigned int8 V_4,
        unsigned int16 V_5, unsigned int32 V_6, unsigned int64 V_7, native int V_8,
        float32 V_9, float64 V_10, valuetype [mscorlib]System.DateTime V_11,
        object V_12, class [mscorlib]System.Random V_13, string V_14)

    IL_0000:  ldc.i4.8
    IL_0001:  stloc.0
    IL_0002:  ldc.i4.s 0x10
    IL_0004:  stloc.1
    IL_0005:  ldc.i4.s 0x20
    IL_0007:  stloc.2
    IL_0008:  ldc.i4.s 0x40
    IL_000a:  conv.i8
    IL_000b:  stloc.3
    IL_000c:  ldc.i4.8
    IL_000d:  stloc.s 4
    IL_000f:  ldc.i4.s 0x10
    IL_0011:  stloc.s 5
    IL_0013:  ldc.i4.s 0x20
    IL_0015:  stloc.s 6
    IL_0017:  ldc.i4.s 0x40
    IL_0019:  conv.i8
    IL_001a:  stloc.s 7
    IL_001c:  ldloca.s 8
    IL_001e:  initobj [mscorlib]System.IntPtr
    IL_0024:  ldc.r4 1.1000000238418579
    IL_0029:  stloc.s 9
    IL_002b:  ldc.r8 2.2000000000000002
    IL_0034:  stloc.s 10
    IL_0036:  ldloca.s 11
    IL_0038:  initobj [mscorlib]System.DateTime
    IL_003e:  ldloca.s 11
    IL_0040:  ldc.i4 2013
    IL_0045:  ldc.i4.s 0x0a
    IL_0047:  ldc.i4.4
    IL_0048:  call instance void valuetype [mscorlib]System.DateTime::'.ctor'(int32, int32, int32)
    IL_004d:  newobj instance void object::'.ctor'()
    IL_0052:  stloc.s 12
    IL_0054:  newobj instance void class [mscorlib]System.Random::'.ctor'()
    IL_0059:  stloc.s 13
    IL_005b:  ldstr "test"
    IL_0060:  stloc.s 14
    IL_0062:  ldloca.s 0
    IL_0064:  ldloca.s 1
    IL_0066:  ldloca.s 2
    IL_0068:  ldloca.s 3
    IL_006a:  ldloca.s 4
    IL_006c:  ldloca.s 5
    IL_006e:  ldloca.s 6
    IL_0070:  ldloca.s 7
    IL_0072:  ldloca.s 8
    IL_0074:  ldloca.s 9
    IL_0076:  ldloca.s 10
    IL_0078:  ldloca.s 11
    IL_007a:  ldloca.s 12
    IL_007c:  ldloca.s 13
    IL_007e:  ldloca.s 14
    IL_0080:  call void class TestCase::ByRef([out] int8&, [out] int16&, [out] int32&, [out] int64&, [out] unsigned int8&, [out] unsigned int16&, [out] unsigned int32&, [out] unsigned int64&, [out] native int&, [out] float32&, [out] float64&, [out] valuetype [mscorlib]System.DateTime&, [out] object&, [out] class [mscorlib]System.Random&, [out] string&)
    IL_0085:  ldloca.s 0
    IL_0087:  ldloca.s 1
    IL_0089:  ldloca.s 2
    IL_008b:  ldloca.s 3
    IL_008d:  ldloca.s 4
    IL_008f:  ldloca.s 5
    IL_0091:  ldloca.s 6
    IL_0093:  ldloca.s 7
    IL_0095:  ldloca.s 8
    IL_0097:  ldloca.s 9
    IL_0099:  ldloca.s 10
    IL_009b:  ldloca.s 11
    IL_009d:  ldloca.s 12
    IL_009f:  ldloca.s 13
    IL_00a1:  ldloca.s 14
    IL_00a3:  call void class TestCase::ByRef([out] int8&, [out] int16&, [out] int32&, [out] int64&, [out] unsigned int8&, [out] unsigned int16&, [out] unsigned int32&, [out] unsigned int64&, [out] native int&, [out] float32&, [out] float64&, [out] valuetype [mscorlib]System.DateTime&, [out] object&, [out] class [mscorlib]System.Random&, [out] string&)
    IL_00a8:  ret
  }

  .method public static hidebysig default void ByRef (int8& i8, int16& i16, int32& i32, int64& i64, unsigned int8& u8, unsigned int16& u16, unsigned int32& u32, unsigned int64& u64, native int& intptr, float32& s, float64& d, valuetype [mscorlib]System.DateTime& dt, object& obj, class [mscorlib]System.Random& rnd, string& str) cil managed
  {
    .maxstack 18

    IL_0000:  ldarg.0
    IL_0001:  ldind.i1
    IL_0002:  conv.i4
    IL_0003:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_0008:  ldarg.1
    IL_0009:  ldind.i2
    IL_000a:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_000f:  ldarg.2
    IL_0010:  ldind.i4
    IL_0011:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_0016:  ldarg.3
    IL_0017:  ldind.i8
    IL_0018:  call void class [mscorlib]System.Console::WriteLine(int64)
    IL_001d:  ldarg.s 4
    IL_001f:  ldind.u1
    IL_0020:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_0025:  ldarg.s 5
    IL_0027:  ldind.u2
    IL_0028:  call void class [mscorlib]System.Console::WriteLine(int32)
    IL_002d:  ldarg.s 6
    IL_002f:  ldind.u4
    IL_0030:  call void class [mscorlib]System.Console::WriteLine(unsigned int32)
    IL_0035:  ldarg.s 7
    IL_0037:  ldind.i8
    IL_0038:  call void class [mscorlib]System.Console::WriteLine(unsigned int64)
    IL_003d:  ldarg.s 8
    IL_003f:  ldind.i
    IL_0040:  box [mscorlib]System.IntPtr
    IL_0045:  call void class [mscorlib]System.Console::WriteLine(object)
    IL_004a:  ldarg.s 9
    IL_004c:  ldind.r4
    IL_004d:  call void class [mscorlib]System.Console::WriteLine(float32)
    IL_0052:  ldarg.s 10
    IL_0054:  ldind.r8
    IL_0055:  call void class [mscorlib]System.Console::WriteLine(float64)
    IL_005a:  ldarg.s 11
    IL_005c:  ldobj [mscorlib]System.DateTime
    IL_0061:  box [mscorlib]System.DateTime
    IL_0066:  call void class [mscorlib]System.Console::WriteLine(object)
    IL_006b:  ldarg.s 12
    IL_006d:  ldind.ref
    IL_006e:  call void class [mscorlib]System.Console::WriteLine(object)
    IL_0073:  ldarg.s 13
    IL_0075:  ldind.ref
    IL_0076:  call void class [mscorlib]System.Console::WriteLine(object)
    IL_007b:  ldarg.s 14
    IL_007d:  ldind.ref
    IL_007e:  call void class [mscorlib]System.Console::WriteLine(string)
    IL_0083:  ldarg.0
    IL_0084:  ldc.i4.s 0xfffffff8
    IL_0086:  stind.i1
    IL_0087:  ldarg.1
    IL_0088:  ldc.i4.s 0xfffffff0
    IL_008a:  stind.i2
    IL_008b:  ldarg.2
    IL_008c:  ldc.i4.s 0xffffffe0
    IL_008e:  stind.i4
    IL_008f:  ldarg.3
    IL_0090:  ldc.i4.s 0xffffffc0
    IL_0092:  conv.i8
    IL_0093:  stind.i8
    IL_0094:  ldarg.s 4
    IL_0096:  ldc.i4.8
    IL_0097:  stind.i1
    IL_0098:  ldarg.s 5
    IL_009a:  ldc.i4.s 0x10
    IL_009c:  stind.i2
    IL_009d:  ldarg.s 6
    IL_009f:  ldc.i4.s 0x20
    IL_00a1:  stind.i4
    IL_00a2:  ldarg.s 7
    IL_00a4:  ldc.i4.s 0x40
    IL_00a6:  conv.i8
    IL_00a7:  stind.i8
    IL_00a8:  ldarg.s 8
    IL_00aa:  initobj [mscorlib]System.IntPtr
    IL_00b0:  ldarg.s 9
    IL_00b2:  ldc.r4 -1.1000000238418579
    IL_00b7:  stind.r4
    IL_00b8:  ldarg.s 10
    IL_00ba:  ldc.r8 -2.2000000000000002
    IL_00c3:  stind.r8
    IL_00c4:  ldarg.s 11
    IL_00c6:  initobj [mscorlib]System.DateTime
    IL_00cc:  ldarg.s 12
    IL_00ce:  newobj instance void object::'.ctor'()
    IL_00d3:  stind.ref
    IL_00d4:  ldarg.s 13
    IL_00d6:  newobj instance void class [mscorlib]System.Random::'.ctor'()
    IL_00db:  stind.ref
    IL_00dc:  ldarg.s 14
    IL_00de:  ldstr "best"
    IL_00e3:  stind.ref
    IL_00e4:  ret
  }

}

