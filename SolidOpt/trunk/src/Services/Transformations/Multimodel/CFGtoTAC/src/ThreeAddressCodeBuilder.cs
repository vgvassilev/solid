/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Mono.Cecil;
using Mono.Cecil.Cil;

using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;
using SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode;

namespace SolidOpt.Services.Transformations.Multimodel.CFGtoTAC
{
  public class ThreeAddressCodeBuilder
  {
    private const string InvalidILExceptionString = "TAC builder: Invalid IL!";
    private readonly TypeReference Int32TypeReference;
    private readonly Triplet FixupTriplet;

    private ControlFlowGraph cfg = null;
    
    public ThreeAddressCodeBuilder(ControlFlowGraph cfg) {
      this.cfg = cfg;
      Int32TypeReference = new TypeReference("System", "Int32", null, true);
      FixupTriplet = new Triplet(-2, TripletOpCode.Nop);
    }
    
    private static VariableReference GenerateTempVar(List<VariableDefinition> inVarList,
                                                          TypeReference varTypeRef)
    {
      VariableDefinition result = new VariableDefinition("T_" + inVarList.Count, varTypeRef);
      inVarList.Add(result);
      return result;
    }
    
    private Dictionary<Triplet, Instruction> ForwardBranchTriplets = new Dictionary<Triplet, Instruction>();
    private Dictionary<Instruction, Triplet> TripletStarts = new Dictionary<Instruction, Triplet>();

    private Triplet GetLabeledTripletByIL(Instruction target)
    {
      Triplet result;
      if (TripletStarts.TryGetValue(target, out result)) return result;
      return FixupTriplet;
    }
    
    private object FixupObject(object obj, object target)
    {
      if (obj == FixupTriplet) return GetLabeledTripletByIL((Instruction)target);
      if (obj is Triplet[]) {
        for (int i = 0; i < (target as Instruction[]).Length; i++) {
          if ((obj as Triplet[])[i] == FixupTriplet)
            (obj as Triplet[])[i] = GetLabeledTripletByIL((target as Instruction[])[i]);
        }
      }
      return obj;
    }
    
    private void FixupForwardBranchTriplets()
    {
      foreach (KeyValuePair<Triplet, Instruction> pair in ForwardBranchTriplets) {
        pair.Key.Result = FixupObject(pair.Key.Result, pair.Value.Operand);
        pair.Key.Operand1 = FixupObject(pair.Key.Operand1, pair.Value.Operand);
        pair.Key.Operand2 = FixupObject(pair.Key.Operand2, pair.Value.Operand);
      }
    }
    
    public ThreeAdressCode Create() {
      List<Triplet> triplets = new List<Triplet>();
      Stack<object> simulationStack = new Stack<object>();
      List<VariableDefinition> tempVariables = new List<VariableDefinition>();
      Instruction instr = cfg.Root.First;

      VariableReference tmpVarRef;
      object obj1, obj2;
      Triplet triplet;

      int tripletIndex = triplets.Count;
      Instruction start = instr;
      int paramOffset = cfg.Method.HasThis ? 1 : 0;
      while (instr != null) {
          switch (instr.OpCode.Code) {
              case Code.Nop:
                  triplets.Add(new Triplet(TripletOpCode.Nop));
                  break;
              case Code.Break:
                  // Nothing to do
                  break;
              case Code.Ldarg_0:
                  if (cfg.Method.HasThis)
                    simulationStack.Push(cfg.Method.Body.ThisParameter);
                  else
                    simulationStack.Push(cfg.Method.Parameters[0]);
                  break;
              case Code.Ldarg_1:
                  simulationStack.Push(cfg.Method.Parameters[1 - paramOffset]);
                  break;
              case Code.Ldarg_2:
                  simulationStack.Push(cfg.Method.Parameters[2 - paramOffset]);
                  break;
              case Code.Ldarg_3:
                  simulationStack.Push(cfg.Method.Parameters[3 - paramOffset]);
                  break;
              case Code.Ldloc_0:
                  simulationStack.Push(cfg.Method.Body.Variables[0]);
                  break;
              case Code.Ldloc_1:
                  simulationStack.Push(cfg.Method.Body.Variables[1]);
                  break;
              case Code.Ldloc_2:
                  simulationStack.Push(cfg.Method.Body.Variables[2]);
                  break;
              case Code.Ldloc_3:
                  simulationStack.Push(cfg.Method.Body.Variables[3]);
                  break;
              case Code.Stloc_0:
                  triplets.Add(new Triplet(TripletOpCode.Assignment, cfg.Method.Body.Variables[0], simulationStack.Pop()));
                  break;
              case Code.Stloc_1:
                  triplets.Add(new Triplet(TripletOpCode.Assignment, cfg.Method.Body.Variables[1], simulationStack.Pop()));
                  break;
              case Code.Stloc_2:
                  triplets.Add(new Triplet(TripletOpCode.Assignment, cfg.Method.Body.Variables[2], simulationStack.Pop()));
                  break;
              case Code.Stloc_3:
                  triplets.Add(new Triplet(TripletOpCode.Assignment, cfg.Method.Body.Variables[3], simulationStack.Pop()));
                  break;
              case Code.Ldarg_S:
                  if (cfg.Method.HasThis && ((ParameterReference)instr.Operand).Index == 0)
                    simulationStack.Push(cfg.Method.Body.ThisParameter);
                  else
                    simulationStack.Push(cfg.Method.Parameters[((ParameterReference)instr.Operand).Index - paramOffset]);
                  break;
//            case Code.Ldarga_S:
              case Code.Starg_S:
                  if (cfg.Method.HasThis && ((ParameterReference)instr.Operand).Index == 0)
                    triplets.Add(new Triplet(TripletOpCode.Assignment, cfg.Method.Body.ThisParameter, simulationStack.Pop()));
                  else
                    triplets.Add(new Triplet(TripletOpCode.Assignment, cfg.Method.Parameters[((ParameterReference)instr.Operand).Index - paramOffset], simulationStack.Pop()));
                  break;
              case Code.Ldloc_S:
                  simulationStack.Push(instr.Operand);
                  break;
//            case Code.Ldloca_S:
              case Code.Stloc_S:
                  triplets.Add(new Triplet(TripletOpCode.Assignment, instr.Operand, simulationStack.Pop()));
                  break;
              case Code.Ldnull:
                  simulationStack.Push(null);
                  break;
              case Code.Ldc_I4_M1:
                  simulationStack.Push(-1);
                  break;
              case Code.Ldc_I4_0:
                  simulationStack.Push(0);
                  break;
              case Code.Ldc_I4_1:
                  simulationStack.Push(1);
                  break;
              case Code.Ldc_I4_2:
                  simulationStack.Push(2);
                  break;
              case Code.Ldc_I4_3:
                  simulationStack.Push(3);
                  break;
              case Code.Ldc_I4_4:
                  simulationStack.Push(4);
                  break;
              case Code.Ldc_I4_5:
                  simulationStack.Push(5);
                  break;
              case Code.Ldc_I4_6:
                  simulationStack.Push(6);
                  break;
              case Code.Ldc_I4_7:
                  simulationStack.Push(7);
                  break;
              case Code.Ldc_I4_8:
                  simulationStack.Push(8);
                  break;
              case Code.Ldc_I4_S:
                  simulationStack.Push(instr.Operand);
                  break;
              case Code.Ldc_I4:
                  simulationStack.Push(instr.Operand);
                  break;
              case Code.Ldc_I8:
                  simulationStack.Push(instr.Operand);
                  break;
              case Code.Ldc_R4:
                  simulationStack.Push(instr.Operand);
                  break;
              case Code.Ldc_R8:
                  simulationStack.Push(instr.Operand);
                  break;
              case Code.Dup:
                  obj1 = simulationStack.Pop();
                  TypeReference typeRef = Helper.GetOperandType(obj1);
                  tmpVarRef = GenerateTempVar(tempVariables, typeRef);
                  triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef, obj1));
                  simulationStack.Push(tmpVarRef);
                  simulationStack.Push(tmpVarRef);
                  break;
              case Code.Pop:
                  simulationStack.Pop();
                  break;
              case Code.Jmp:
                  MethodReference jmpMethod = instr.Operand as MethodReference;
                  if (jmpMethod.HasThis) {
                    // Construct the this parameter.
                    ParameterReference thisRef = new ParameterDefinition("0",
                                                                         ParameterAttributes.None,
                                                                         jmpMethod.DeclaringType);
                    Debug.Assert(cfg.Method.HasThis, "Current method doesn't have this ptr.");
                    triplets.Add(new Triplet(TripletOpCode.PushParam, null, thisRef));
                  }

                  ParameterDefinition paramDef = null;
                  for (int i = jmpMethod.Parameters.Count - 1; i >= 0; i--) {
                    paramDef = jmpMethod.Parameters[i];
                    Debug.Assert(paramDef == cfg.Method.Parameters[i], "Args differ");
                    triplets.Add(new Triplet(TripletOpCode.PushParam, null, paramDef));
                  }

                  if (jmpMethod.ReturnType.FullName == "System.Void") {
                      //???    || ((instr.Next != null) && (instr.Next.OpCode.Code == Code.Pop))) {
                      triplets.Add(new Triplet(TripletOpCode.Call, null, jmpMethod));
                  } else {
                      tmpVarRef = GenerateTempVar(tempVariables, jmpMethod.ReturnType);
                      triplets.Add(new Triplet(TripletOpCode.Call, tmpVarRef, jmpMethod));
                      simulationStack.Push(tmpVarRef);
                  }
                  break;
              case Code.Call:
                  Stack<object> callReverseStack = new Stack<object>();
                  MethodReference callMethod = instr.Operand as MethodReference;
                  int callMethodHasThis = callMethod.HasThis ? 1 : 0;
                  
                  for (int i = callMethod.Parameters.Count + callMethodHasThis; i > 0; i--)
                      callReverseStack.Push(simulationStack.Pop());
                  for (int i = callMethod.Parameters.Count + callMethodHasThis; i > 0; i--) {
                      obj1 = callReverseStack.Pop();
                      triplets.Add(new Triplet(TripletOpCode.PushParam, null, obj1));
                  }
                  if (callMethod.ReturnType.FullName == "System.Void") {
                      //???    || ((instr.Next != null) && (instr.Next.OpCode.Code == Code.Pop))) {
                      triplets.Add(new Triplet(TripletOpCode.CallVirt, null, instr.Operand));
                  } else {
                      tmpVarRef = GenerateTempVar(tempVariables, callMethod.ReturnType);
                      triplets.Add(new Triplet(TripletOpCode.Call, tmpVarRef, instr.Operand));
                      simulationStack.Push(tmpVarRef);
                  }
                  break;
//              case Code.Calli:
              case Code.Callvirt:
                  Stack<object> callVirtReverseStack = new Stack<object>();
                  MethodReference callVirtMethod = instr.Operand as MethodReference;
                  int callVirtMethodHasThis = callVirtMethod.HasThis ? 1 : 0;
                  
                  for (int i = callVirtMethod.Parameters.Count + callVirtMethodHasThis; i > 0; i--)
                      callVirtReverseStack.Push(simulationStack.Pop());
                  for (int i = callVirtMethod.Parameters.Count + callVirtMethodHasThis; i > 0; i--) {
                      obj1 = callVirtReverseStack.Pop();
                      triplets.Add(new Triplet(TripletOpCode.PushParam, null, obj1));
                  }
                  if (callVirtMethod.ReturnType.FullName == "System.Void") {
                      //???    || ((instr.Next != null) && (instr.Next.OpCode.Code == Code.Pop))) {
                      triplets.Add(new Triplet(TripletOpCode.CallVirt, null, instr.Operand));
                  } else {
                      tmpVarRef = GenerateTempVar(tempVariables, callVirtMethod.ReturnType);
                      triplets.Add(new Triplet(TripletOpCode.CallVirt, tmpVarRef, instr.Operand));
                      simulationStack.Push(tmpVarRef);
                  }
                  break;
              case Code.Ret:
                  if (simulationStack.Count > 0)
                      triplets.Add(new Triplet(TripletOpCode.Return, null, simulationStack.Pop()));
                  else
                      triplets.Add(new Triplet(TripletOpCode.Return));
                  break;
              case Code.Br_S:
                  triplet = new Triplet(TripletOpCode.Goto, null, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand1 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Brfalse_S:
                  obj1 = simulationStack.Pop();
                  //???if (!Helper.???(obj1)) throw new Exception(InvalidILExceptionString);
                  triplet = new Triplet(TripletOpCode.IfFalse, null, obj1, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Brtrue_S:
                  obj1 = simulationStack.Pop();
                  //???if (!Helper.???(obj1)) throw new Exception(InvalidILExceptionString);
                  triplet = new Triplet(TripletOpCode.IfTrue, null, obj1, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Beq_S:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
                      throw new Exception(InvalidILExceptionString);
                  tmpVarRef = GenerateTempVar(tempVariables, Int32TypeReference);
                  triplets.Add(new Triplet(TripletOpCode.Equal, tmpVarRef, obj1, obj2));
                  triplet = new Triplet(TripletOpCode.IfTrue, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Bge_S:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
                      throw new Exception(InvalidILExceptionString);
                  tmpVarRef = GenerateTempVar(tempVariables, Int32TypeReference);
                  triplets.Add(new Triplet(TripletOpCode.Less, tmpVarRef, obj1, obj2));
                  triplet = new Triplet(TripletOpCode.IfFalse, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Bgt_S:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
                      throw new Exception(InvalidILExceptionString);
                  tmpVarRef = GenerateTempVar(tempVariables, Int32TypeReference);
                  triplets.Add(new Triplet(TripletOpCode.Great, tmpVarRef, obj1, obj2));
                  triplet = new Triplet(TripletOpCode.IfTrue, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Ble_S:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
                      throw new Exception(InvalidILExceptionString);
                  tmpVarRef = GenerateTempVar(tempVariables, Int32TypeReference);
                  triplets.Add(new Triplet(TripletOpCode.Great, tmpVarRef, obj1, obj2));
                  triplet = new Triplet(TripletOpCode.IfFalse, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Blt_S:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
                      throw new Exception(InvalidILExceptionString);
                  tmpVarRef = GenerateTempVar(tempVariables, Int32TypeReference);
                  triplets.Add(new Triplet(TripletOpCode.Less, tmpVarRef, obj1, obj2));
                  triplet = new Triplet(TripletOpCode.IfTrue, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
//            case Code.Bne_Un_S:
//            case Code.Bge_Un_S:
//            case Code.Bgt_Un_S:
//            case Code.Ble_Un_S:
//            case Code.Blt_Un_S:
              case Code.Br:
                  triplet = new Triplet(TripletOpCode.Goto, null, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand1 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Brfalse:
                  obj1 = simulationStack.Pop();
                  //???if (!Helper.???(obj1)) throw new Exception(InvalidILExceptionString);
                  triplet = new Triplet(TripletOpCode.IfFalse, null, obj1, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Brtrue:
                  obj1 = simulationStack.Pop();
                  //???if (!Helper.???(obj1)) throw new Exception(InvalidILExceptionString);
                  triplet = new Triplet(TripletOpCode.IfTrue, null, obj1, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Beq:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
                      throw new Exception(InvalidILExceptionString);
                  tmpVarRef = GenerateTempVar(tempVariables, Int32TypeReference);
                  triplets.Add(new Triplet(TripletOpCode.Equal, tmpVarRef, obj1, obj2));
                  triplet = new Triplet(TripletOpCode.IfTrue, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Bge:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
                      throw new Exception(InvalidILExceptionString);
                  tmpVarRef = GenerateTempVar(tempVariables, Int32TypeReference);
                  triplets.Add(new Triplet(TripletOpCode.Less, tmpVarRef, obj1, obj2));
                  triplet = new Triplet(TripletOpCode.IfFalse, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Bgt:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
                      throw new Exception(InvalidILExceptionString);
                  tmpVarRef = GenerateTempVar(tempVariables, Int32TypeReference);
                  triplets.Add(new Triplet(TripletOpCode.Great, tmpVarRef, obj1, obj2));
                  triplet = new Triplet(TripletOpCode.IfTrue, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Ble:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
                      throw new Exception(InvalidILExceptionString);
                  tmpVarRef = GenerateTempVar(tempVariables, Int32TypeReference);
                  triplets.Add(new Triplet(TripletOpCode.Great, tmpVarRef, obj1, obj2));
                  triplet = new Triplet(TripletOpCode.IfFalse, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
              case Code.Blt:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
                      throw new Exception(InvalidILExceptionString);
                  tmpVarRef = GenerateTempVar(tempVariables, Int32TypeReference);
                  triplets.Add(new Triplet(TripletOpCode.Less, tmpVarRef, obj1, obj2));
                  triplet = new Triplet(TripletOpCode.IfTrue, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
                  if (triplet.Operand2 == FixupTriplet)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
//            case Code.Bne_Un:
//            case Code.Bge_Un:
//            case Code.Bgt_Un:
//            case Code.Ble_Un:
//            case Code.Blt_Un:
              case Code.Switch:
                  obj1 = simulationStack.Pop();
                  Triplet[] labels = new Triplet[((Instruction[])instr.Operand).Length];
                  bool needFixup = false;
                  int i = 0;
                  foreach (Instruction ins in (Instruction[])instr.Operand) {
                      triplet = GetLabeledTripletByIL(ins);
                      labels[i++] = triplet;
                      if (triplet == FixupTriplet)
                          needFixup = true;
                  }
                  triplet = new Triplet(TripletOpCode.Switch, null, obj1, labels);
                  if (needFixup)
                      ForwardBranchTriplets[triplet] = instr;
                  triplets.Add(triplet);
                  break;
//            case Code.Ldind_I1:
//            case Code.Ldind_U1:
//            case Code.Ldind_I2:
//            case Code.Ldind_U2:
//            case Code.Ldind_I4:
//            case Code.Ldind_U4:
//            case Code.Ldind_I8:
//            case Code.Ldind_I:
//            case Code.Ldind_R4:
//            case Code.Ldind_R8:
//            case Code.Ldind_Ref:
//            case Code.Stind_Ref:
//            case Code.Stind_I1:
//            case Code.Stind_I2:
//            case Code.Stind_I4:
//            case Code.Stind_I8:
//            case Code.Stind_R4:
//            case Code.Stind_R8:
              case Code.Add:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  tmpVarRef = GenerateTempVar(tempVariables, Helper.BinaryNumericOperations(obj1, obj2));
                  triplets.Add(new Triplet(TripletOpCode.Addition, tmpVarRef, obj1, obj2));
                  simulationStack.Push(tmpVarRef);
                  break;
              case Code.Sub:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  tmpVarRef = GenerateTempVar(tempVariables, Helper.BinaryNumericOperations(obj1, obj2));
                  triplets.Add(new Triplet(TripletOpCode.Substraction, tmpVarRef, obj1, obj2));
                  simulationStack.Push(tmpVarRef);
                  break;
              case Code.Mul:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  tmpVarRef = GenerateTempVar(tempVariables, Helper.BinaryNumericOperations(obj1, obj2));
                  triplets.Add(new Triplet(TripletOpCode.Multiplication, tmpVarRef, obj1, obj2));
                  simulationStack.Push(tmpVarRef);
                  break;
              case Code.Div:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  tmpVarRef = GenerateTempVar(tempVariables, Helper.BinaryNumericOperations(obj1, obj2));
                  triplets.Add(new Triplet(TripletOpCode.Division, tmpVarRef, obj1, obj2));
                  simulationStack.Push(tmpVarRef);
                  break;
//            case Code.Div_Un:
              case Code.Rem:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  tmpVarRef = GenerateTempVar(tempVariables, Helper.BinaryNumericOperations(obj1, obj2));
                  triplets.Add(new Triplet(TripletOpCode.Reminder, tmpVarRef, obj1, obj2));
                  simulationStack.Push(tmpVarRef);
                  break;
//            case Code.Rem_Un:
              case Code.And:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  tmpVarRef = GenerateTempVar(tempVariables, Helper.IntegerOperations(obj1, obj2));
                  triplets.Add(new Triplet(TripletOpCode.And, tmpVarRef, obj1, obj2));
                  simulationStack.Push(tmpVarRef);
                  break;
              case Code.Or:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  tmpVarRef = GenerateTempVar(tempVariables, Helper.IntegerOperations(obj1, obj2));
                  triplets.Add(new Triplet(TripletOpCode.Or, tmpVarRef, obj1, obj2));
                  simulationStack.Push(tmpVarRef);
                  break;
              case Code.Xor:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  tmpVarRef = GenerateTempVar(tempVariables, Helper.IntegerOperations(obj1, obj2));
                  triplets.Add(new Triplet(TripletOpCode.Xor, tmpVarRef, obj1, obj2));
                  simulationStack.Push(tmpVarRef);
                  break;
              case Code.Shl:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  tmpVarRef = GenerateTempVar(tempVariables, Helper.ShiftOperations(obj1, obj2));
                  triplets.Add(new Triplet(TripletOpCode.ShiftLeft, tmpVarRef, obj1, obj2));
                  simulationStack.Push(tmpVarRef);
                  break;
              case Code.Shr:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  tmpVarRef = GenerateTempVar(tempVariables, Helper.ShiftOperations(obj1, obj2));
                  triplets.Add(new Triplet(TripletOpCode.ShiftRight, tmpVarRef, obj1, obj2));
                  simulationStack.Push(tmpVarRef);
                  break;
//            case Code.Shr_Un:
              case Code.Neg:
                  obj1 = simulationStack.Pop();
                  tmpVarRef = GenerateTempVar(tempVariables, Helper.UnaryNumericOperations(obj1));
                  triplets.Add(new Triplet(TripletOpCode.Negate, tmpVarRef, obj1));
                  simulationStack.Push(tmpVarRef);
                  break;
              case Code.Not:
                  obj1 = simulationStack.Pop();
                  tmpVarRef = GenerateTempVar(tempVariables, Helper.IntegerOperations(obj1, obj1)); //TODO: Strange specification description. Read again.
                  triplets.Add(new Triplet(TripletOpCode.Not, tmpVarRef, obj1));
                  simulationStack.Push(tmpVarRef);
                  break;
//            case Code.Conv_I1:
//            case Code.Conv_I2:
//            case Code.Conv_I4:
//            case Code.Conv_I8:
//            case Code.Conv_R4:
//            case Code.Conv_R8:
//            case Code.Conv_U4:
//            case Code.Conv_U8:
//            case Code.Callvirt:
//            case Code.Cpobj:
//            case Code.Ldobj:
              case Code.Ldstr:
                  simulationStack.Push(instr.Operand);
                  break;
//            case Code.Newobj:
//            case Code.Castclass:
//            case Code.Isinst:
//            case Code.Conv_R_Un:
//            case Code.Unbox:
//            case Code.Throw:
//            case Code.Ldfld:
//            case Code.Ldflda:
//            case Code.Stfld:
//            case Code.Ldsfld:
//            case Code.Ldsflda:
//            case Code.Stsfld:
//            case Code.Stobj:
//            case Code.Conv_Ovf_I1_Un:
//            case Code.Conv_Ovf_I2_Un:
//            case Code.Conv_Ovf_I4_Un:
//            case Code.Conv_Ovf_I8_Un:
//            case Code.Conv_Ovf_U1_Un:
//            case Code.Conv_Ovf_U2_Un:
//            case Code.Conv_Ovf_U4_Un:
//            case Code.Conv_Ovf_U8_Un:
//            case Code.Conv_Ovf_I_Un:
//            case Code.Conv_Ovf_U_Un:
//            case Code.Box:
//            case Code.Newarr:
//            case Code.Ldlen:
//            case Code.Ldelema:
//            case Code.Ldelem_I1:
//            case Code.Ldelem_U1:
//            case Code.Ldelem_I2:
//            case Code.Ldelem_U2:
//            case Code.Ldelem_I4:
//            case Code.Ldelem_U4:
//            case Code.Ldelem_I8:
//            case Code.Ldelem_I:
//            case Code.Ldelem_R4:
//            case Code.Ldelem_R8:
//            case Code.Ldelem_Ref:
//            case Code.Stelem_I:
//            case Code.Stelem_I1:
//            case Code.Stelem_I2:
//            case Code.Stelem_I4:
//            case Code.Stelem_I8:
//            case Code.Stelem_R4:
//            case Code.Stelem_R8:
//            case Code.Stelem_Ref:
//            case Code.Ldelem_Any:
//            case Code.Stelem_Any:
//            case Code.Unbox_Any:
//            case Code.Conv_Ovf_I1:
//            case Code.Conv_Ovf_U1:
//            case Code.Conv_Ovf_I2:
//            case Code.Conv_Ovf_U2:
//            case Code.Conv_Ovf_I4:
//            case Code.Conv_Ovf_U4:
//            case Code.Conv_Ovf_I8:
//            case Code.Conv_Ovf_U8:
//            case Code.Refanyval:
//            case Code.Ckfinite:
//            case Code.Mkrefany:
//            case Code.Ldtoken:
//            case Code.Conv_U2:
//            case Code.Conv_U1:
//            case Code.Conv_I:
//            case Code.Conv_Ovf_I:
//            case Code.Conv_Ovf_U:
//            case Code.Add_Ovf:
//            case Code.Add_Ovf_Un:
//            case Code.Mul_Ovf:
//            case Code.Mul_Ovf_Un:
//            case Code.Sub_Ovf:
//            case Code.Sub_Ovf_Un:
//            case Code.Endfinally:
//            case Code.Leave:
//            case Code.Leave_S:
//            case Code.Stind_I:
//            case Code.Conv_U:
//            case Code.Arglist:
              case Code.Ceq:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
                      throw new Exception(InvalidILExceptionString);
                  tmpVarRef = GenerateTempVar(tempVariables, Int32TypeReference);
                  triplets.Add(new Triplet(TripletOpCode.Equal, tmpVarRef, obj1, obj2));
                  simulationStack.Push(tmpVarRef);
                  break;
              case Code.Cgt:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
                      throw new Exception(InvalidILExceptionString);
                  tmpVarRef = GenerateTempVar(tempVariables, Int32TypeReference);
                  triplets.Add(new Triplet(TripletOpCode.Great, tmpVarRef, obj1, obj2));
                  simulationStack.Push(tmpVarRef);
                  break;
//            case Code.Cgt_Un:
              case Code.Clt:
                  obj2 = simulationStack.Pop();
                  obj1 = simulationStack.Pop();
                  if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
                      throw new Exception(InvalidILExceptionString);
                  tmpVarRef = GenerateTempVar(tempVariables, Int32TypeReference);
                  triplets.Add(new Triplet(TripletOpCode.Less, tmpVarRef, obj1, obj2));
                  simulationStack.Push(tmpVarRef);
                  break;
//            case Code.Clt_Un:
//            case Code.Ldftn:
//            case Code.Ldvirtftn:
              case Code.Ldarg:
                  if (cfg.Method.HasThis && ((ParameterReference)instr.Operand).Index == 0)
                    simulationStack.Push(cfg.Method.Body.ThisParameter);
                  else
                    simulationStack.Push(cfg.Method.Parameters[((ParameterReference)instr.Operand).Index - paramOffset]);
                  break;
//            case Code.Ldarga:
              case Code.Starg:
                  if (cfg.Method.HasThis && ((ParameterReference)instr.Operand).Index == 0)
                    triplets.Add(new Triplet(TripletOpCode.Assignment, cfg.Method.Body.ThisParameter, simulationStack.Pop()));
                  else
                    triplets.Add(new Triplet(TripletOpCode.Assignment, cfg.Method.Parameters[((ParameterReference)instr.Operand).Index - paramOffset], simulationStack.Pop()));
                  break;
              case Code.Ldloc:
                  simulationStack.Push(instr.Operand);
                  break;
//            case Code.Ldloca:
              case Code.Stloc:
                  triplets.Add(new Triplet(TripletOpCode.Assignment, instr.Operand, simulationStack.Pop()));
                  break;
//            case Code.Localloc:
//            case Code.Endfilter:
//            case Code.Unaligned:
//            case Code.Volatile:
//            case Code.Tail:
//            case Code.Initobj:
//            case Code.Constrained:
//            case Code.Cpblk:
//            case Code.Initblk:
//            case Code.No:
//            case Code.Rethrow:
//            case Code.Sizeof:
//            case Code.Refanytype:
//            case Code.Readonly:

              default:
                  throw new NotImplementedException(instr.OpCode.ToString());
          }

          if (tripletIndex != triplets.Count) {
              TripletStarts.Add(start, triplets[tripletIndex]);
              tripletIndex = triplets.Count;
              start = instr.Next;
          }

          instr = instr.Next;
      }

      FixupForwardBranchTriplets();

      for (int i = 0; i < triplets.Count; i++) {
          triplets[i].offset = i;
          if (i > 0) triplets[i].Previous = triplets[i-1];
          if (i < triplets.Count-1) triplets[i].Next = triplets[i+1];
      }
      
      return new ThreeAdressCode(cfg.Method, triplets[0], triplets, tempVariables);
    }
  }

  public static class Helper
  {
        private const string InvalidILExceptionString = "TAC builder: Invalid IL!";
        private const string UnsupportedTypeExceptionString = "TAC builder: Unsupported type!";

        public static TypeReference GetOperandType(object op)
        {
            Type t = op.GetType();
            if (op is VariableReference) return (op as VariableReference).VariableType;
            if (op is ParameterReference) return (op as ParameterReference).ParameterType;
            return new TypeReference(t.Namespace, t.Name, null, t.IsValueType);
        }

        public static int GetTypeKind(TypeReference tr)
        {
            switch (tr.FullName) {
                case "System.Int32": return 0;
                case "System.Int64": return 1;
                case "System.IntPtr": return 2;
                case "System.Single": return 3;
                case "System.Double": return 4;
            }
            if (tr.IsPointer) return 5;
            if (!tr.IsValueType) return 6;
            throw new Exception(UnsupportedTypeExceptionString);
        }

        // Binary numeric operations

        public readonly static Type[,] BinaryNumericOperationsResultTypes =
        {
        //              Int32           Int64           IntPtr                Single          Double          &                     Obj
        /* Int32  */  { typeof(Int32),  null,           typeof(IntPtr),       null,           null,           typeof(PointerType),  null },
        /* Int64  */  { null,           typeof(Int64),  null,                 null,           null,           null,                 null },
        /* IntPtr */  { typeof(IntPtr), null,           typeof(IntPtr),       null,           null,           typeof(PointerType),  null },
        /* Single */  { null,           null,           null,                 typeof(Single), null,           null,                 null },
        /* Double */  { null,           null,           null,                 null,           typeof(Double), null,                 null },
        /* &      */  { typeof(PointerType), null,      typeof(PointerType),  null,           null,           typeof(IntPtr),       null },
        /* Obj    */  { null,           null,           null,                 null,           null,           null,                 null }
        };

        public static TypeReference BinaryNumericOperations(object op1, object op2)
        {
            Type resultType = BinaryNumericOperationsResultTypes[GetTypeKind(GetOperandType(op1)), GetTypeKind(GetOperandType(op2))];

            //TODO: PointerType result are valid only with Add/Sub IL instructions. See CIL specification Table III.2
            if (resultType == null) throw new Exception(InvalidILExceptionString);

            return new TypeReference(resultType.Namespace, resultType.Name, null, resultType.IsValueType);
        }

        // The unary numeric operations

        public readonly static Type[] UnaryNumericOperationsResultTypes =
          // Int32          Int64          IntPtr          Single          Double          &     Obj
          {  typeof(Int32), typeof(Int32), typeof(IntPtr), typeof(Single), typeof(Double), null, null };

        public static TypeReference UnaryNumericOperations(object op)
        {
            Type resultType = UnaryNumericOperationsResultTypes[GetTypeKind(GetOperandType(op))];

            if (resultType == null) throw new Exception(InvalidILExceptionString);
            
            return new TypeReference(resultType.Namespace, resultType.Name, null, resultType.IsValueType);
        }

        // The binary comparison or branch operations

        public readonly static bool[,] BinaryComparisonOrBranchOperationsResultTypes =
        {
            //              Int32   Int64   IntPtr  Single  Double  &       Obj
            /* Int32  */  { true,   false,  true,   false,  false,  false,  false },
            /* Int64  */  { false,  true,   false,  false,  false,  false,  false },
            /* IntPtr */  { true,   false,  true,   false,  false,  true,   false },
            /* Single */  { false,  false,  false,  true,   false,  false,  false },
            /* Double */  { false,  false,  false,  false,  true,   false,  false },
            /* &      */  { false,  false,  true,   false,  false,  true,   false },
            /* Obj    */  { false,  false,  false,  false,  false,  false,  true  }
        };

        public static bool BinaryComparisonOrBranchOperations(object op1, object op2)
        {
            bool result = BinaryComparisonOrBranchOperationsResultTypes[GetTypeKind(GetOperandType(op1)), GetTypeKind(GetOperandType(op2))];

            //TODO: PointerType and Object/Object result are valid only with beq[.s], bne.un[.s] and ceq IL instructions. See CIL specification Table III.4

            return result;
        }

        // Integer operations
        
        public readonly static Type[,] IntegerOperationsResultTypes =
        {
            //              Int32           Int64           IntPtr          Single  Double  &       Obj
            /* Int32  */  { typeof(Int32),  null,           typeof(IntPtr), null,   null,   null,   null },
            /* Int64  */  { null,           typeof(Int64),  null,           null,   null,   null,   null },
            /* IntPtr */  { typeof(IntPtr), null,           typeof(IntPtr), null,   null,   null,   null },
            /* Single */  { null,           null,           null,           null,   null,   null,   null },
            /* Double */  { null,           null,           null,           null,   null,   null,   null },
            /* &      */  { null,           null,           null,           null,   null,   null,   null },
            /* Obj    */  { null,           null,           null,           null,   null,   null,   null }
        };

        public static TypeReference IntegerOperations(object op1, object op2)
        {
            Type resultType = IntegerOperationsResultTypes[GetTypeKind(GetOperandType(op1)), GetTypeKind(GetOperandType(op2))];
            
            if (resultType == null) throw new Exception(InvalidILExceptionString);
            
            return new TypeReference(resultType.Namespace, resultType.Name, null, resultType.IsValueType);
        }

        // Shift operations
        
        public readonly static Type[,] ShiftOperationsResultTypes =
        {
            //              Int32           Int64   IntPtr          Single  Double  &       Obj
            /* Int32  */  { typeof(Int32),  null,   typeof(IntPtr), null,   null,   null,   null },
            /* Int64  */  { typeof(Int64),  null,   typeof(Int64),  null,   null,   null,   null },
            /* IntPtr */  { typeof(IntPtr), null,   typeof(IntPtr), null,   null,   null,   null },
            /* Single */  { null,           null,   null,           null,   null,   null,   null },
            /* Double */  { null,           null,   null,           null,   null,   null,   null },
            /* &      */  { null,           null,   null,           null,   null,   null,   null },
            /* Obj    */  { null,           null,   null,           null,   null,   null,   null }
        };
        
        public static TypeReference ShiftOperations(object op1, object shiftBy)
        {
            Type resultType = ShiftOperationsResultTypes[GetTypeKind(GetOperandType(op1)), GetTypeKind(GetOperandType(shiftBy))];
            
            if (resultType == null) throw new Exception(InvalidILExceptionString);
            
            return new TypeReference(resultType.Namespace, resultType.Name, null, resultType.IsValueType);
        }

        // Overflow arithmetic operations
        
        public readonly static Type[,] OverflowArithmeticOperationsResultTypes =
        {
            //              Int32           Int64           IntPtr                Single          Double        &                     Obj
            /* Int32  */  { typeof(Int32),  null,           typeof(IntPtr),       null,           null,         typeof(PointerType),  null },
            /* Int64  */  { null,           typeof(Int64),  null,                 null,           null,         null,                 null },
            /* IntPtr */  { typeof(IntPtr), null,           typeof(IntPtr),       null,           null,         typeof(PointerType),  null },
            /* Single */  { null,           null,           null,                 null,           null,         null,                 null },
            /* Double */  { null,           null,           null,                 null,           null,         null,                 null },
            /* &      */  { typeof(PointerType), null,      typeof(PointerType),  null,           null,         typeof(IntPtr),       null },
            /* Obj    */  { null,           null,           null,                 null,           null,         null,                 null }
        };
        
        public static TypeReference OverflowArithmeticOperations(object op1, object op2)
        {
            Type resultType = OverflowArithmeticOperationsResultTypes[GetTypeKind(GetOperandType(op1)), GetTypeKind(GetOperandType(op2))];
            
            //TODO: PointerType result are valid only with Add/Sub IL instructions. See CIL specification Table III.7
            if (resultType == null) throw new Exception(InvalidILExceptionString);
            
            return new TypeReference(resultType.Namespace, resultType.Name, null, resultType.IsValueType);
        }
  }

}
