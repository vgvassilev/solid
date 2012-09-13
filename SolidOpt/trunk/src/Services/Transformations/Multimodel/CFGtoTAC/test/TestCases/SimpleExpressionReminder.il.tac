System.Int32 TestCase::Main(System.Int32 a, System.Int32 b) {
  L0: T_0 = a == b
  L1: iffalse T_0 goto L3
  L2: a = 1000
  L3: T_1 = a % b
  L4: return T_1
}
