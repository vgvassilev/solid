Main
L0: flag = 0;
L1: PushParam "A"
L2: tmp0 = call s.StartsWith();
L3: ifFalse tmp0 goto L5;
L4: flag = s.Length >= 3;
L5: flag = false;
L6: ret flag;
//XFAIL: 