System.Void TestCase::Main() {
  L0: pushparam 32
  L1: pushparam 32
  L2: T_0 = new System.Void System.Int32[,]::.ctor(System.Int32,System.Int32)
  L3: V_0 = T_0
  L4: pushparam V_0
  L5: pushparam 1
  L6: pushparam 1
  L7: pushparam 5
  L8: call System.Void System.Int32[,]::Set(System.Int32,System.Int32,System.Int32)
  L9: pushparam V_0
  L10: pushparam 1
  L11: pushparam 1
  L12: T_1 = call System.Int32 System.Int32[,]::Get(System.Int32,System.Int32)
  L13: pushparam V_0
  L14: pushparam 2
  L15: pushparam 2
  L16: pushparam T_1
  L17: call System.Void System.Int32[,]::Set(System.Int32,System.Int32,System.Int32)
  L18: pushparam V_0
  L19: pushparam 2
  L20: pushparam 2
  L21: T_2 = call System.Int32& System.Int32[,]::Address(System.Int32,System.Int32)
  L22: pushparam T_2
  L23: call System.Void TestCase::m(System.Int32&)
  L24: return
}
