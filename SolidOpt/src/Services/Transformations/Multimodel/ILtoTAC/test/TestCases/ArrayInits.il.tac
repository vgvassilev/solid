System.Void TestCase::Main() {
  L0: T_0 = new System.Int32[2]
  L1: T_1 = T_0
  L2: T_1[0] = 1
  L3: T_2 = T_1
  L4: T_2[1] = 2
  L5: V_0 = T_2
  L6: T_3 = new System.Int32[2][]
  L7: T_4 = T_3
  L8: T_5 = new System.Int32[3]
  L9: T_6 = T_5
  L10: pushparam T_6
  L11: pushparam <PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}.$field-0
  L12: call System.Void System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(System.Array,System.RuntimeFieldHandle)
  L13: T_4[0] = T_6
  L14: T_7 = T_4
  L15: T_8 = new System.Int32[3]
  L16: T_9 = T_8
  L17: pushparam T_9
  L18: pushparam <PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}.$field-1
  L19: call System.Void System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(System.Array,System.RuntimeFieldHandle)
  L20: T_7[1] = T_9
  L21: V_1 = T_7
  L22: pushparam 2
  L23: pushparam 2
  L24: T_10 = new System.Void System.Int32[,]::.ctor(System.Int32,System.Int32)
  L25: T_11 = T_10
  L26: pushparam T_11
  L27: pushparam <PrivateImplementationDetails>{df3f263a-9b18-43ec-b2da-1941d8c8ffb9}.$field-2
  L28: call System.Void System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(System.Array,System.RuntimeFieldHandle)
  L29: V_2 = T_11
  L30: return
}
