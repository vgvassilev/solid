System.Void TestCase::Main() {
  L0: pushparam 0
  L1: pushparam "Value:"
  L2: call System.Void TestCase::sM1(System.Int32,System.String)
  L3: T_0 = addressof System.Void TestCase::sM1(System.Int32,System.String)
  L4: pushparam 1
  L5: pushparam "Value:"
  L6: call deref T_0
  L7: T_1 = 2 + 3
  L8: T_2 = 2 * 3
  L9: T_3 = addressof System.Void TestCase::sM3(System.Int32,System.String,System.Int32)
  L10: pushparam T_1
  L11: pushparam "Value:"
  L12: pushparam T_2
  L13: call deref T_3
  L14: T_4 = addressof System.Int32 TestCase::sM2(System.Int32)
  L15: pushparam 4
  L16: T_5 = call deref T_4
  L17: pushparam T_5
  L18: call System.Void System.Console::WriteLine(System.Int32)
  L19: T_6 = new System.Void TestCase::.ctor()
  L20: V_0 = T_6
  L21: T_7 = addressof System.Int32 TestCase::M1(System.Int32)
  L22: pushparam V_0
  L23: pushparam 5
  L24: T_8 = call deref T_7
  L25: pushparam T_8
  L26: call System.Void System.Console::WriteLine(System.Int32)
  L27: T_9 = addressof V_0.System.Int32 TestCase::vM1(System.Int32)
  L28: pushparam V_0
  L29: pushparam 6
  L30: T_10 = call deref T_9
  L31: pushparam T_10
  L32: call System.Void System.Console::WriteLine(System.Int32)
  L33: return
}
