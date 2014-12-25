System.Void TestCase::Main() {
  L0: T_0 = new System.Void TestCase::.ctor()
  L1: V_0 = T_0
  L2: T_1 = addressof System.Int32 TestCase::M(System.Int32,System.Int32)
  L3: pushparam V_0
  L4: pushparam T_1
  L5: T_2 = new System.Void TestCase/PerformCalculation::.ctor(System.Object,System.IntPtr)
  L6: V_1 = T_2
  L7: pushparam V_1
  L8: pushparam 1
  L9: pushparam 2
  L10: T_3 = callvirt System.Int32 TestCase/PerformCalculation::Invoke(System.Int32,System.Int32)
  L11: pushparam T_3
  L12: call System.Void System.Console::WriteLine(System.Int32)
  L13: T_4 = addressof System.Int32 TestCase::M(System.Int32,System.Int32)
  L14: pushparam V_0
  L15: pushparam T_4
  L16: T_5 = new System.Void TestCase/PerformCalculation::.ctor(System.Object,System.IntPtr)
  L17: pushparam V_0
  L18: pushparam T_5
  L19: callvirt System.Void TestCase::add_OnCalc(TestCase/PerformCalculation)
  L20: T_6 = addressof System.Int32 TestCase::sM(System.Int32,System.Int32)
  L21: pushparam null
  L22: pushparam T_6
  L23: T_7 = new System.Void TestCase/PerformCalculation::.ctor(System.Object,System.IntPtr)
  L24: pushparam V_0
  L25: pushparam T_7
  L26: callvirt System.Void TestCase::add_OnCalc(TestCase/PerformCalculation)
  L27: T_8 = V_0
  L28: T_9 = addressof T_8.System.Int32 TestCase::vM(System.Int32,System.Int32)
  L29: pushparam T_8
  L30: pushparam T_9
  L31: T_10 = new System.Void TestCase/PerformCalculation::.ctor(System.Object,System.IntPtr)
  L32: pushparam V_0
  L33: pushparam T_10
  L34: callvirt System.Void TestCase::add_OnCalc(TestCase/PerformCalculation)
  L35: T_11 = V_0.OnCalc
  L36: iffalse T_11 goto L44
  L37: T_12 = V_0.OnCalc
  L38: pushparam T_12
  L39: pushparam 3
  L40: pushparam 4
  L41: T_13 = callvirt System.Int32 TestCase/PerformCalculation::Invoke(System.Int32,System.Int32)
  L42: pushparam T_13
  L43: call System.Void System.Console::WriteLine(System.Int32)
  L44: return
}

