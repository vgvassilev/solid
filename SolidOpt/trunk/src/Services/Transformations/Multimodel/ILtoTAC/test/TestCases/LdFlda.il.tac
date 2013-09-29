System.Void TestCase::Main() {
  L0: T_0 = new System.Void TestCase/MyClass::.ctor()
  L1: T_1 = addressof T_0.i
  L2: pushparam T_1
  L3: T_2 = call System.String System.Int32::ToString()
  L4: pushparam T_2
  L5: call System.Void System.Console::WriteLine(System.String)
  L6: return
}
