System.Void TestCase::Main() {
  L0: V_0 = 8
  L1: V_1 = 16
  L2: V_2 = 32
  L3: T_0 = (System.Int64) 64
  L4: V_3 = T_0
  L5: V_4 = 8
  L6: V_5 = 16
  L7: V_6 = 32
  L8: T_1 = (System.Int64) 64
  L9: V_7 = T_1
  L10: T_2 = addressof V_8
  L11: dafaultinit deref T_2 System.IntPtr
  L12: V_9 = 1.1
  L13: V_10 = 2.2
  L14: T_3 = addressof V_11
  L15: dafaultinit deref T_3 System.DateTime
  L16: T_4 = addressof V_11
  L17: pushparam T_4
  L18: pushparam 2013
  L19: pushparam 10
  L20: pushparam 4
  L21: call System.Void System.DateTime::.ctor(System.Int32,System.Int32,System.Int32)
  L22: T_5 = new System.Void System.Object::.ctor()
  L23: V_12 = T_5
  L24: T_6 = new System.Void System.Random::.ctor()
  L25: V_13 = T_6
  L26: V_14 = "test"
  L27: T_7 = addressof V_0
  L28: T_8 = addressof V_1
  L29: T_9 = addressof V_2
  L30: T_10 = addressof V_3
  L31: T_11 = addressof V_4
  L32: T_12 = addressof V_5
  L33: T_13 = addressof V_6
  L34: T_14 = addressof V_7
  L35: T_15 = addressof V_8
  L36: T_16 = addressof V_9
  L37: T_17 = addressof V_10
  L38: T_18 = addressof V_11
  L39: T_19 = addressof V_12
  L40: T_20 = addressof V_13
  L41: T_21 = addressof V_14
  L42: pushparam T_7
  L43: pushparam T_8
  L44: pushparam T_9
  L45: pushparam T_10
  L46: pushparam T_11
  L47: pushparam T_12
  L48: pushparam T_13
  L49: pushparam T_14
  L50: pushparam T_15
  L51: pushparam T_16
  L52: pushparam T_17
  L53: pushparam T_18
  L54: pushparam T_19
  L55: pushparam T_20
  L56: pushparam T_21
  L57: call System.Void TestCase::ByRef(System.SByte&,System.Int16&,System.Int32&,System.Int64&,System.Byte&,System.UInt16&,System.UInt32&,System.UInt64&,System.IntPtr&,System.Single&,System.Double&,System.DateTime&,System.Object&,System.Random&,System.String&)
  L58: T_22 = addressof V_0
  L59: T_23 = addressof V_1
  L60: T_24 = addressof V_2
  L61: T_25 = addressof V_3
  L62: T_26 = addressof V_4
  L63: T_27 = addressof V_5
  L64: T_28 = addressof V_6
  L65: T_29 = addressof V_7
  L66: T_30 = addressof V_8
  L67: T_31 = addressof V_9
  L68: T_32 = addressof V_10
  L69: T_33 = addressof V_11
  L70: T_34 = addressof V_12
  L71: T_35 = addressof V_13
  L72: T_36 = addressof V_14
  L73: pushparam T_22
  L74: pushparam T_23
  L75: pushparam T_24
  L76: pushparam T_25
  L77: pushparam T_26
  L78: pushparam T_27
  L79: pushparam T_28
  L80: pushparam T_29
  L81: pushparam T_30
  L82: pushparam T_31
  L83: pushparam T_32
  L84: pushparam T_33
  L85: pushparam T_34
  L86: pushparam T_35
  L87: pushparam T_36
  L88: call System.Void TestCase::ByRef(System.SByte&,System.Int16&,System.Int32&,System.Int64&,System.Byte&,System.UInt16&,System.UInt32&,System.UInt64&,System.IntPtr&,System.Single&,System.Double&,System.DateTime&,System.Object&,System.Random&,System.String&)
  L89: return
}

System.Void TestCase::ByRef(System.SByte& i8, System.Int16& i16, System.Int32& i32, System.Int64& i64, System.Byte& u8, System.UInt16& u16, System.UInt32& u32, System.UInt64& u64, System.IntPtr& intptr, System.Single& s, System.Double& d, System.DateTime& dt, System.Object& obj, System.Random& rnd, System.String& str) {
  L0: T_0 = deref i8
  L1: T_1 = (System.Int32) T_0
  L2: pushparam T_1
  L3: call System.Void System.Console::WriteLine(System.Int32)
  L4: T_2 = deref i16
  L5: pushparam T_2
  L6: call System.Void System.Console::WriteLine(System.Int32)
  L7: T_3 = deref i32
  L8: pushparam T_3
  L9: call System.Void System.Console::WriteLine(System.Int32)
  L10: T_4 = deref i64
  L11: pushparam T_4
  L12: call System.Void System.Console::WriteLine(System.Int64)
  L13: T_5 = deref u8
  L14: pushparam T_5
  L15: call System.Void System.Console::WriteLine(System.Int32)
  L16: T_6 = deref u16
  L17: pushparam T_6
  L18: call System.Void System.Console::WriteLine(System.Int32)
  L19: T_7 = deref u32
  L20: pushparam T_7
  L21: call System.Void System.Console::WriteLine(System.UInt32)
  L22: T_8 = deref u64
  L23: pushparam T_8
  L24: call System.Void System.Console::WriteLine(System.UInt64)
  L25: T_9 = deref intptr
  L26: T_10 = (System.Object) T_9
  L27: pushparam T_10
  L28: call System.Void System.Console::WriteLine(System.Object)
  L29: T_11 = deref s
  L30: pushparam T_11
  L31: call System.Void System.Console::WriteLine(System.Single)
  L32: T_12 = deref d
  L33: pushparam T_12
  L34: call System.Void System.Console::WriteLine(System.Double)
  L35: T_13 = deref dt
  L36: T_14 = (System.Object) T_13
  L37: pushparam T_14
  L38: call System.Void System.Console::WriteLine(System.Object)
  L39: T_15 = deref obj
  L40: pushparam T_15
  L41: call System.Void System.Console::WriteLine(System.Object)
  L42: T_16 = deref rnd
  L43: pushparam T_16
  L44: call System.Void System.Console::WriteLine(System.Object)
  L45: T_17 = deref str
  L46: pushparam T_17
  L47: call System.Void System.Console::WriteLine(System.String)
  L48: deref i8 = -8
  L49: deref i16 = -16
  L50: deref i32 = -32
  L51: T_18 = (System.Int64) -64
  L52: deref i64 = T_18
  L53: deref u8 = 8
  L54: deref u16 = 16
  L55: deref u32 = 32
  L56: T_19 = (System.Int64) 64
  L57: deref u64 = T_19
  L58: dafaultinit deref intptr System.IntPtr
  L59: deref s = -1.1
  L60: deref d = -2.2
  L61: dafaultinit deref dt System.DateTime
  L62: T_20 = new System.Void System.Object::.ctor()
  L63: deref obj = T_20
  L64: T_21 = new System.Void System.Random::.ctor()
  L65: deref rnd = T_21
  L66: deref str = "best"
  L67: return
}
