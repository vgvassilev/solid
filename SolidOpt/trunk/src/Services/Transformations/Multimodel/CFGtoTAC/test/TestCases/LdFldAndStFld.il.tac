System.Void TestCase::Main() {
  L0: T_0 = new TestCase/MyClass::ctor // NOTE: Is this the 'right' notation for nested classes?
  L1: V_0 = T_0
  L2: T_1 = T_0.NonStaticFld
  L3: pushparam T_1
  L4: call System.Void System.Console::WriteLine(System.Int32)
  L5: return
}
