System.Void TestCase::Main() {
  L0: V_0 = 1
  L1: T_0 = (System.Object) V_0
  L2: V_2 = T_0
  L3: T_1 = (System.Int32) V_2
  L4: T_2 = T_1 + 1
  L5: V_1 = T_2
  L6: T_3 = (System.Object) V_1
  L7: pushparam T_3
  L8: call System.Void TestCase::Box(System.Object)
  L9: pushparam V_2
  L10: T_4 = call System.Int32 TestCase::UnBox(System.Object)
  L11: T_5 = (System.Object) T_4
  L12: pushparam T_5
  L13: call System.Void TestCase::Box(System.Object)
  L14: T_6 = addressof V_1
  L15: pushparam T_6
  L16: call System.Void TestCase::UnBox1(System.Int32&)
  L17: pushparam V_1
  L18: call System.Void System.Console::WriteLine(System.Int32)
  L19: return
}
