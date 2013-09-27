System.Void TestCase::Main() {
  L0: pushparam 97
  L1: pushparam 1
  L2: T_0 = new System.String(System.Char,System.Int32)
  L3: V_0 = T_0
  L4: T_1 = V_0 as System.String
  L5: iffalse T_1 goto L9
  L6: T_2 = V_0 as System.String
  L7: pushparam T_2
  L8: call System.Void System.Console::WriteLine(System.String)
  L9: return
}
