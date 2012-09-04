Main
L0: tmp0 = arg0 + arg1
L1: tmp1 = tmp0 > arg2
L2: ifFalse tmp1 goto L4
L3: return arg2
L4: return arg0
//XFAIL: