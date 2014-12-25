System.Void TestCase::Main() {
  L0: T_0 = new System.Int32[32]
  L1: V_0 = T_0
  L2: V_0[1] = 5
  L3: T_1 = V_0[1]
  L4: V_0[2] = T_1
  L5: T_2 = addressof V_0[2]
  L6: pushparam T_2
  L7: call System.Void TestCase::m(System.Int32&)
  L8: pushparam 32
  L9: pushparam 32
  L10: T_3 = new System.Void System.Int32[,]::.ctor(System.Int32,System.Int32)
  L11: V_1 = T_3
  L12: pushparam V_1
  L13: pushparam 1
  L14: pushparam 1
  L15: pushparam 5
  L16: call System.Void System.Int32[,]::Set(System.Int32,System.Int32,System.Int32)
  L17: pushparam V_1
  L18: pushparam 1
  L19: pushparam 1
  L20: T_4 = call System.Int32 System.Int32[,]::Get(System.Int32,System.Int32)
  L21: pushparam V_1
  L22: pushparam 2
  L23: pushparam 2
  L24: pushparam T_4
  L25: call System.Void System.Int32[,]::Set(System.Int32,System.Int32,System.Int32)
  L26: pushparam V_1
  L27: pushparam 2
  L28: pushparam 2
  L29: T_5 = call System.Int32& System.Int32[,]::Address(System.Int32,System.Int32)
  L30: pushparam T_5
  L31: call System.Void TestCase::m(System.Int32&)
  L32: return
}
