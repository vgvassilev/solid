System.Int32 TestCase::Main(System.Int32 a, System.Int32 b) {
  L0: T_0 = a * 2
  L1: a = T_0
  L2: T_1 = a < b
  L3: iftrue T_1 goto L0
  L4: return a
}
