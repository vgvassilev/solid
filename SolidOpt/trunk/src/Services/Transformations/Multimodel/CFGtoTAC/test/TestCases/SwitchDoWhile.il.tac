System.String TestCase::Main(System.Int32 a) {
  L0: V_0 = ""
  L1: T_0 = a
  L2: T_1 = T_0 - 1
  L3: a = T_1
  L4: V_1 = T_0
  L5: switch V_1 goto L7, L9, L11, L16, L16, L16, L13, L15
  L6: goto L16
  L7: V_0 = "Zero"
  L8: goto L16
  L9: V_0 = "One"
  L10: goto L17
  L11: V_0 = "Two"
  L12: goto L16
  L13: V_0 = "Six"
  L14: goto L16
  L15: V_0 = "Seven"
  L16: V_0 = "!"
  L17: T_2 = a > 0
  L18: iftrue T_2 goto L1
  L19: return V_0 
}
