System.Int32 TestCase::Main() {
  L0: V_0 = 32767
  L1: V_1 = 32767
  L2: T_0 = V_0 + V_1
  L3: checkoverflow
  L4: T_1 = (System.Int16) T_0
  L5: checkoverflow
  L6: V_2 = T_1
  L7: return V_2
}
