Main
L0: tmp0 = arg0 + arg1
L1: ifFalse tmp0 goto L4
L2: tmp1 = arg0 + arg2
L3: return tmp1
L4: tmp2 = arg1 + arg2
L5: return tmp2
//XFAIL: