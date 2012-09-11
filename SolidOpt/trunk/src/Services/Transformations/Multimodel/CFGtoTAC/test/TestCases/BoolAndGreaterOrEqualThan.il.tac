Main
L0: pushparam "A"
L1: T_0 = callvirt System.Boolean System.String::StartsWith(System.String) s
L2: iffalse T_0 goto L8
L3: T_1 = callvirt System.Int32 System.String::get_Length() s
L4: T_2 = T_1 < 3
L5: T_3 = T_2 == 0
L6: V_0 = T_3
L7: goto L9
L8: V_0 = 0
L9: return V_0
