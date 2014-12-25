System.Void TestCase::Main() {
  L0: nop
  L1: V_0 = 4
  L2: V_1 = 5
  L3: T_0 = V_0 > V_1
  L4: iffalse T_0 goto L6
  L5: V_0 = 1
  L6: T_1 = V_0 < V_1
  L7: iffalse T_1 goto L9
  L8: V_1 = 1
  L9: T_2 = V_0 == V_1
  L10: iffalse T_2 goto L12
  L11: V_0 = 0
  L12: T_3 = V_0 == V_1
  L13: iftrue T_3 goto L15
  L14: V_1 = 0
  L15: T_4 = V_0 < V_1
  L16: iftrue T_4 goto L18
  L17: V_0 = 1
  L18: T_5 = V_0 > V_1
  L19: iftrue T_5 goto L21
  L20: V_1 = 1
  L21: return
}
