System.Void TestCase::Main() {
  L0: T_0 = new System.Random()
  L1: V_0 = T_0
  L2: pushparam V_0
  L3: T_1 = callvirt System.Int32 System.Random::Next()
  L4: V_1 = T_1
  L5: T_2 = &V_1
  L6: pushparam T_2
  L7: call System.Void System.Console::WriteLine(System.Int32)
  L8: return
}

