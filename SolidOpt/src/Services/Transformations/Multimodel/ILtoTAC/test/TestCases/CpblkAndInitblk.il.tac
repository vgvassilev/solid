System.Void TestCase::Main(System.String[] args) {
  L0: nop
  L1: pushparam 2
  L2: T_0 = call System.IntPtr System.Runtime.InteropServices.Marshal::AllocHGlobal(System.Int32)
  L3: V_0 = T_0
  L4: pushparam 2
  L5: T_1 = call System.IntPtr System.Runtime.InteropServices.Marshal::AllocHGlobal(System.Int32)
  L6: V_1 = T_1
  L7: pushparam V_0
  L8: pushparam 0
  L9: pushparam 2
  L10: call System.Void TestCase::Init(System.IntPtr,System.Byte,System.Int32)
  L11: pushparam V_0
  L12: pushparam V_1
  L13: pushparam 2
  L14: call System.Void TestCase::Copy(System.IntPtr,System.IntPtr,System.UInt32)
  L15: return
}
