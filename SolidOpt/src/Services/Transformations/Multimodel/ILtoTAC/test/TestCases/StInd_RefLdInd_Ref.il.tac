System.Void TestCase::Main() {
  L0: T_0 = (System.Object) 0
  L1: V_0 = T_0
  L2: T_1 = addressof V_0
  L3: pushparam T_1
  L4: call System.Void TestCase::ByRef(System.Object&)
  L5: pushparam V_0
  L6: call System.Void System.Console::WriteLine(System.Object)
  L7: return
}

System.Void TestCase::ByRef(System.Object& x) {
  L0: T_0 = deref x
  L1: pushparam T_0
  L2: call System.Void System.Console::WriteLine(System.Object)
  L3: T_1 = (System.Object) 0
  L4: deref x = T_1
  L5: return
}
