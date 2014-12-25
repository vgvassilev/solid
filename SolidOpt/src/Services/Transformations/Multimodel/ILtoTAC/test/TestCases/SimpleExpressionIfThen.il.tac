System.Int32 TestCase::Main(System.Int32 a, System.Int32 b, System.Int32 c) {
  L0: T_0 = a + b
  L1: T_1 = T_0 > c
  L2: iffalse T_1 goto L4
  L3: return c
  L4: return a
}
