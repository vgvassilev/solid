Main
L0: T_0 = a > b
L1: ifFalse T_0 goto L4
L2: T_1 = a + c
L3: return T_1
L4: T_2 = b + c
L5: return T_2
//XFAIL: