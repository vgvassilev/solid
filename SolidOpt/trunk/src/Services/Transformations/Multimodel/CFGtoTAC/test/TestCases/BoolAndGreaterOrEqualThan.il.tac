Main
L0: V_0 = 0;
L1: PushParam "A"
L2: T_0 = callvirt s string::StartsWith(string)
L3: iffalse T_0 goto L8
L4: T_1 = callvirt s string::get_Length()
L5: T_2 = T_1 >= 3
L6: V_0 = T_2
L7: goto L9
L8: V_0 = false
L9: return V_0
//XFAIL: 