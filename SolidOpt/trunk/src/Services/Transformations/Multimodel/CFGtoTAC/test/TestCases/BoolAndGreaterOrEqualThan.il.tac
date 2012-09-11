Main
L0: pushparam s
L1: pushparam "A"
L2: T_0 = callvirt System.Boolean System.String::StartsWith(System.String)
L3: iffalse T_0 goto L10
L4: pushparam s
L5: T_1 = callvirt System.Int32 System.String::get_Length()
L6: T_2 = T_1 < 3
L7: T_3 = T_2 == 0
L8: V_0 = T_3
L9: goto L11
L10: V_0 = 0
L11: return V_0
