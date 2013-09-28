System.Void TestCase::Main() {
  L0: nop
  L1: T_0 = new System.Void TestCase/MyClass::.ctor()
  L2: V_0 = T_0
  L3: T_1 = V_0
  L4: T_2 = T_1.NonStaticFld
  L5: T_3 = T_2 + 1
  L6: T_4 = T_3
  L7: V_1 = T_4
  L8: T_1.NonStaticFld = T_4
  L9: pushparam V_1
  L10: call System.Void System.Console::WriteLine(System.Int32)
  L11: return
}
