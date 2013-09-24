System.Void TestCase::Main() {
  L0: T_0 = 'a'
  L1: T_1 = 1
  L2: pushparam T_0
  L3: pushparam T_1
  L4: T_2 = new System.String(System.Char, System.Int32)
  L5: T_3 = T_2 as System.String
  L6: T_4 = T_3 == null
  L7: iftrue T_4 goto L8
  L6: pushparam T_3
  L7: T_5 = call Console.WriteLine(System.String)
  L8:
}