System.Void TestCase::Main() {
  L0: T_0[] = new int[2]
  // FIXME: In that case this is weird syntax, because V_0[] exports visual info about the type of the rhs
  // and the lhs does not. Maybe we should change the array variable to not have [] as mandatory notation.
  L1: V_0[] = T_0
  L2: T_2 = sizeof(V_0)
  L3: pushparam T_2
  L4: call System.Void System.Console::WriteLine(System.Int32)
  L5: return
}
