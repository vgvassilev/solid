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

using SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoTAC
{
  public class ThreeAddressCodeBuilder
  {
    private const string InvalidILExceptionString = "TAC builder: Invalid IL!";
    private readonly Triplet FixupTriplet;

    private MethodDefinition method = null;
    
    public ThreeAddressCodeBuilder(MethodDefinition method) {
      this.method = method;
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
    
    public ThreeAddressCode Create() {
      List<Triplet> triplets = new List<Triplet>();
      Stack<object> simulationStack = new Stack<object>();
      List<VariableDefinition> tempVariables = new List<VariableDefinition>();
      Instruction instr = method.Body.Instructions[0];

      VariableReference tmpVarRef;
      object obj1, obj2;
      Triplet triplet;

      int tripletIndex = triplets.Count;
      Instruction start = instr;
      int paramOffset = method.HasThis ? 1 : 0;
      while (instr != null) {
        switch (instr.OpCode.Code) {
          case Code.Nop:
            triplets.Add(new Triplet(TripletOpCode.Nop));
            break;
          case Code.Break:
            // Nothing to do
            break;
          case Code.Ldarg_0:
            if (method.HasThis)
              simulationStack.Push(method.Body.ThisParameter);
            else
              simulationStack.Push(method.Parameters[0]);
            break;
          case Code.Ldarg_1:
            simulationStack.Push(method.Parameters[1 - paramOffset]);
            break;
          case Code.Ldarg_2:
            simulationStack.Push(method.Parameters[2 - paramOffset]);
            break;
          case Code.Ldarg_3:
            simulationStack.Push(method.Parameters[3 - paramOffset]);
            break;
          case Code.Ldloc_0:
            simulationStack.Push(method.Body.Variables[0]);
            break;
          case Code.Ldloc_1:
            simulationStack.Push(method.Body.Variables[1]);
            break;
          case Code.Ldloc_2:
            simulationStack.Push(method.Body.Variables[2]);
            break;
          case Code.Ldloc_3:
            simulationStack.Push(method.Body.Variables[3]);
            break;
          case Code.Stloc_0:
            triplets.Add(new Triplet(TripletOpCode.Assignment, method.Body.Variables[0], simulationStack.Pop()));
            break;
          case Code.Stloc_1:
            triplets.Add(new Triplet(TripletOpCode.Assignment, method.Body.Variables[1], simulationStack.Pop()));
            break;
          case Code.Stloc_2:
            triplets.Add(new Triplet(TripletOpCode.Assignment, method.Body.Variables[2], simulationStack.Pop()));
            break;
          case Code.Stloc_3:
            triplets.Add(new Triplet(TripletOpCode.Assignment, method.Body.Variables[3], simulationStack.Pop()));
            break;
          case Code.Ldarga_S: // Intentional fall through
          case Code.Ldarga:
            //TODO: Check - Use reference to GetOperandType(instr.Operand)?
            tmpVarRef = GenerateTempVar(tempVariables, Helper.PointerTypeRef);
            triplets.Add(new Triplet(TripletOpCode.AddressOf, tmpVarRef, instr.Operand));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldloca_S: // Intentional fall through
          case Code.Ldloca:
            //TODO: Check - Use reference to GetOperandType(instr.Operand)?
            tmpVarRef = GenerateTempVar(tempVariables, Helper.PointerTypeRef);
            triplets.Add(new Triplet(TripletOpCode.AddressOf, tmpVarRef, instr.Operand));
            simulationStack.Push(tmpVarRef);
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
            // Make sure that argument lists are the same
            Debug.Assert(jmpMethod.CallingConvention == method.CallingConvention,
                         "Calling convention differs.");
            Debug.Assert(jmpMethod.HasThis == method.HasBody, "One of the methods has this");
            ParameterDefinition paramDef = null;
            for (int i = jmpMethod.Parameters.Count - 1; i >= 0; i--) {
              paramDef = jmpMethod.Parameters[i];
              Debug.Assert(paramDef == method.Parameters[i], "Args differ");
            }
            triplets.Add(new Triplet(TripletOpCode.Goto, null, jmpMethod));
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
              triplets.Add(new Triplet(TripletOpCode.Call, null, instr.Operand));
            } else {
              tmpVarRef = GenerateTempVar(tempVariables, callMethod.ReturnType);
              triplets.Add(new Triplet(TripletOpCode.Call, tmpVarRef, instr.Operand));
              simulationStack.Push(tmpVarRef);
            }
            break;
          case Code.Calli:
            CallSite callSite = (CallSite)instr.Operand;

            obj1 = simulationStack.Pop();

            Stack<object> calliReverseStack = new Stack<object>();
            int callSiteHasThis = callSite.HasThis ? 1 : 0;
            
            for (int i = callSite.Parameters.Count + callSiteHasThis; i > 0; i--)
              calliReverseStack.Push(simulationStack.Pop());
            for (int i = callSite.Parameters.Count + callSiteHasThis; i > 0; i--) {
              obj2 = calliReverseStack.Pop();
              triplets.Add(new Triplet(TripletOpCode.PushParam, null, obj2));
            }
            if (callSite.ReturnType.FullName == "System.Void") {
              //???    || ((instr.Next != null) && (instr.Next.OpCode.Code == Code.Pop))) {
              triplets.Add(new Triplet(TripletOpCode.Call, null, new DeReference(obj1)));
            } else {
              tmpVarRef = GenerateTempVar(tempVariables, callSite.ReturnType);
              triplets.Add(new Triplet(TripletOpCode.Call, tmpVarRef, new DeReference(obj1)));
              simulationStack.Push(tmpVarRef);
            }
            break;
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
          case Code.Br_S: // Intentional fall through
          case Code.Br:
            triplet = new Triplet(TripletOpCode.Goto, null, GetLabeledTripletByIL((Instruction)instr.Operand));
            if (triplet.Operand1 == FixupTriplet)
              ForwardBranchTriplets[triplet] = instr;
            triplets.Add(triplet);
            break;
          case Code.Brfalse_S: // Intentional fall through
          case Code.Brfalse:
            obj1 = simulationStack.Pop();
            //???if (!Helper.???(obj1)) throw new Exception(InvalidILExceptionString);
            triplet = new Triplet(TripletOpCode.IfFalse, null, obj1, GetLabeledTripletByIL((Instruction)instr.Operand));
            if (triplet.Operand2 == FixupTriplet)
              ForwardBranchTriplets[triplet] = instr;
            triplets.Add(triplet);
            break;
          case Code.Brtrue_S: // Intentional fall through
          case Code.Brtrue:
            obj1 = simulationStack.Pop();
            //???if (!Helper.???(obj1)) throw new Exception(InvalidILExceptionString);
            triplet = new Triplet(TripletOpCode.IfTrue, null, obj1, GetLabeledTripletByIL((Instruction)instr.Operand));
            if (triplet.Operand2 == FixupTriplet)
              ForwardBranchTriplets[triplet] = instr;
            triplets.Add(triplet);
            break;
          case Code.Beq_S: // Intentional fall through
          case Code.Beq:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
              throw new Exception(InvalidILExceptionString);
            tmpVarRef = GenerateTempVar(tempVariables, Helper.BoolTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Equal, tmpVarRef, obj1, obj2));
            triplet = new Triplet(TripletOpCode.IfTrue, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
            if (triplet.Operand2 == FixupTriplet)
              ForwardBranchTriplets[triplet] = instr;
            triplets.Add(triplet);
            break;
          case Code.Bge_Un: // Intentional fall through
          case Code.Bge_Un_S: // Intentional fall through
          case Code.Bge_S: // Intentional fall through
          case Code.Bge:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
              throw new Exception(InvalidILExceptionString);
            tmpVarRef = GenerateTempVar(tempVariables, Helper.BoolTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Less, tmpVarRef, obj1, obj2));
            triplet = new Triplet(TripletOpCode.IfFalse, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
            if (triplet.Operand2 == FixupTriplet)
              ForwardBranchTriplets[triplet] = instr;
            triplets.Add(triplet);
            break;
          case Code.Bgt_Un: // Intentional fall through
          case Code.Bgt_Un_S: // Intentional fall through
          case Code.Bgt_S: // Intentional fall through
          case Code.Bgt:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
              throw new Exception(InvalidILExceptionString);
            tmpVarRef = GenerateTempVar(tempVariables, Helper.BoolTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Great, tmpVarRef, obj1, obj2));
            triplet = new Triplet(TripletOpCode.IfTrue, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
            if (triplet.Operand2 == FixupTriplet)
              ForwardBranchTriplets[triplet] = instr;
            triplets.Add(triplet);
            break;
          case Code.Ble_Un_S: // Intentional fall through
          case Code.Ble_Un: // Intentional fall through
          case Code.Ble_S: // Intentional fall through
          case Code.Ble:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
              throw new Exception(InvalidILExceptionString);
            tmpVarRef = GenerateTempVar(tempVariables, Helper.BoolTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Great, tmpVarRef, obj1, obj2));
            triplet = new Triplet(TripletOpCode.IfFalse, null, tmpVarRef, GetLabeledTripletByIL((Instruction)instr.Operand));
            if (triplet.Operand2 == FixupTriplet)
              ForwardBranchTriplets[triplet] = instr;
            triplets.Add(triplet);
            break;
          case Code.Blt_Un: // Intentional fall through
          case Code.Blt_Un_S: // Intentional fall through
          case Code.Blt_S: // Intentional fall through
          case Code.Blt:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
              throw new Exception(InvalidILExceptionString);
            tmpVarRef = GenerateTempVar(tempVariables, Helper.BoolTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Less, tmpVarRef, obj1, obj2));
            triplet = new Triplet(TripletOpCode.IfTrue, null, tmpVarRef,
                                  GetLabeledTripletByIL((Instruction)instr.Operand));
            if (triplet.Operand2 == FixupTriplet)
              ForwardBranchTriplets[triplet] = instr;
            triplets.Add(triplet);
            break;
          case Code.Bne_Un_S: // Intentional fall through
          case Code.Bne_Un:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
              throw new Exception(InvalidILExceptionString);
            tmpVarRef = GenerateTempVar(tempVariables, Helper.BoolTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Equal, tmpVarRef, obj1, obj2));
            triplet = new Triplet(TripletOpCode.IfFalse, null, tmpVarRef, 
                                  GetLabeledTripletByIL((Instruction)instr.Operand));
            if (triplet.Operand2 == FixupTriplet)
              ForwardBranchTriplets[triplet] = instr;
            triplets.Add(triplet);
            break;
          case Code.Switch:
            obj1 = simulationStack.Pop();
            Triplet[] labels = new Triplet[((Instruction[])instr.Operand).Length];
            bool needFixup = false;
            int sw_i = 0;
            foreach (Instruction ins in (Instruction[])instr.Operand) {
              triplet = GetLabeledTripletByIL(ins);
              labels[sw_i++] = triplet;
              if (triplet == FixupTriplet)
                needFixup = true;
            }
            triplet = new Triplet(TripletOpCode.Switch, null, obj1, labels);
            if (needFixup)
              ForwardBranchTriplets[triplet] = instr;
            triplets.Add(triplet);
            break;
          case Code.Ldind_I1:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int8TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
                                   new DeReference(simulationStack.Pop()))
                         );
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldind_U1:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt8TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
                                   new DeReference(simulationStack.Pop()))
                         );
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldind_I2:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int16TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
                                   new DeReference(simulationStack.Pop()))
                         );
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldind_U2:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt16TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
                                   new DeReference(simulationStack.Pop()))
                         );
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldind_I4:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int32TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
                                   new DeReference(simulationStack.Pop()))
                         );
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldind_U4:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt32TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
                                   new DeReference(simulationStack.Pop()))
                         );
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldind_I8:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int64TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
                                   new DeReference(simulationStack.Pop()))
                         );
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldind_I:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.IntPtrTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
                                   new DeReference(simulationStack.Pop()))
                         );
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldind_R4:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.SingleTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
                                   new DeReference(simulationStack.Pop()))
                         );
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldind_R8:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.DoubleTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
                                   new DeReference(simulationStack.Pop()))
                         );
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldind_Ref:
            //TODO: Determine result object class by type of address referece in stack
            tmpVarRef = GenerateTempVar(tempVariables, Helper.ObjectTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
                                   new DeReference(simulationStack.Pop()))
                         );
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Stind_I1:
          case Code.Stind_I2:
          case Code.Stind_I4:
          case Code.Stind_I8:
          case Code.Stind_I:
          case Code.Stind_R4:
          case Code.Stind_R8:
          case Code.Stind_Ref:
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment,
                                   new DeReference(simulationStack.Pop()),
                                   obj1)
                         );
            break;
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
          case Code.Div_Un:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            VariableReference tmpVarRefO1 = null;
            VariableReference tmpVarRefO2 = null;

            switch (Helper.GetTypeKind(Helper.GetOperandType(obj1))) {
              case /*Int32*/ 0:
                tmpVarRefO1 = GenerateTempVar(tempVariables, Helper.UInt32TypeRef);
              break;
              case /*Int64*/ 1:
                tmpVarRefO1 = GenerateTempVar(tempVariables, Helper.UInt64TypeRef);
              break;
              case /*IntPtr*/ 2:
                tmpVarRefO1 = GenerateTempVar(tempVariables, Helper.UIntPtrTypeRef);
              break;
            }

            switch (Helper.GetTypeKind(Helper.GetOperandType(obj2))) {
              case /*Int32*/ 0:
                tmpVarRefO2 = GenerateTempVar(tempVariables, Helper.UInt32TypeRef);
              break;
              case /*Int64*/ 1:
                tmpVarRefO2 = GenerateTempVar(tempVariables, Helper.UInt64TypeRef);
              break;
              case /*IntPtr*/ 2:
                tmpVarRefO2 = GenerateTempVar(tempVariables, Helper.UIntPtrTypeRef);
              break;
            }

            if (tmpVarRefO1 != null)
              triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRefO1, tmpVarRefO1.VariableType, obj1));
            if (tmpVarRefO2 != null)
              triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRefO2, tmpVarRefO2.VariableType, obj2));

            tmpVarRef = GenerateTempVar(tempVariables,
                          Helper.IntegerOperationsUn(tmpVarRefO1 ?? obj1, tmpVarRefO2 ?? obj2));
            triplets.Add(new Triplet(TripletOpCode.Division, tmpVarRef,
                                     tmpVarRefO1 ?? obj1, tmpVarRefO2 ?? obj2));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Rem:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.BinaryNumericOperations(obj1, obj2));
            triplets.Add(new Triplet(TripletOpCode.Reminder, tmpVarRef, obj1, obj2));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Rem_Un:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            VariableReference tmpVarRefO1rem = null;
            VariableReference tmpVarRefO2rem = null;
            
            switch (Helper.GetTypeKind(Helper.GetOperandType(obj1))) {
              case /*Int32*/ 0:
                tmpVarRefO1rem = GenerateTempVar(tempVariables, Helper.UInt32TypeRef);
                break;
              case /*Int64*/ 1:
                tmpVarRefO1rem = GenerateTempVar(tempVariables, Helper.UInt64TypeRef);
                break;
              case /*IntPtr*/ 2:
                tmpVarRefO1rem = GenerateTempVar(tempVariables, Helper.UIntPtrTypeRef);
                break;
            }
            
            switch (Helper.GetTypeKind(Helper.GetOperandType(obj2))) {
              case /*Int32*/ 0:
                tmpVarRefO2rem = GenerateTempVar(tempVariables, Helper.UInt32TypeRef);
                break;
              case /*Int64*/ 1:
                tmpVarRefO2rem = GenerateTempVar(tempVariables, Helper.UInt64TypeRef);
                break;
              case /*IntPtr*/ 2:
                tmpVarRefO2rem = GenerateTempVar(tempVariables, Helper.UIntPtrTypeRef);
                break;
            }
            
            if (tmpVarRefO1rem != null)
              triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRefO1rem, tmpVarRefO1rem.VariableType, obj1));
            if (tmpVarRefO2rem != null)
              triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRefO2rem, tmpVarRefO2rem.VariableType, obj2));

            tmpVarRef = GenerateTempVar(tempVariables,
                          Helper.IntegerOperationsUn(tmpVarRefO1rem ?? obj1, tmpVarRefO2rem ?? obj2));
            triplets.Add(new Triplet(TripletOpCode.Reminder, tmpVarRef,
                                 tmpVarRefO1rem ?? obj1, tmpVarRefO2rem ?? obj2));
            simulationStack.Push(tmpVarRef);
            break;       
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
          case Code.Shr_Un: // Intentional fall through
            // [ECMA-335: 3.60] shr.un - Shift integer right, unsigned
            //
            // stack transition: ..., value, shiftAmount -> ..., result
            //
            // The shr.un instruction shifts value (int32, int 64 or native int) right by the number of bits
            // specified by shiftAmount. shiftAmount is of type int32 or native int. The return value is 
            // unspecified if shiftAmount is greater than or equal to the width of value. shr.un inserts 
            // a zero bit on each shift.
            // See Table 6: Shift Operations for details of which operand types are allowed, and their 
            // corresponding result type.
            //
          case Code.Shr:
            // [ECMA-335: 3.59] shr - Shift integer right.
            //
            // stack transition: ..., value, shiftAmount -> ..., result
            //
            // The shr instruction shifts value (int32, int64 or native int) right by the number of bits 
            // specified by shiftAmount. shiftAmount is of type int32 or native int. The return value is 
            // unspecified if shiftAmount is greater than or equal to the width of value. shr replicates
            // the high order bit on each shift, preserving the sign of the original value in result. 
            // See Table 6: Shift Operations for details of which operand types are allowed, and their 
            // corresponding result type.
            //
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.ShiftOperations(obj1, obj2));
            triplets.Add(new Triplet(TripletOpCode.ShiftRight, tmpVarRef, obj1, obj2));
            simulationStack.Push(tmpVarRef);
            break;
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
          case Code.Conv_I1:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int8TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.Int8TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Conv_I2:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int16TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.Int16TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Conv_I4:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int32TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.Int32TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Conv_I8:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int64TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.Int64TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Conv_R4:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.SingleTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.SingleTypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Conv_R8:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.DoubleTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.DoubleTypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Conv_U4:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt32TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.UInt32TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Conv_U8:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt64TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.UInt64TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Cpobj:
            TypeReference copyTypeRef = (TypeReference)instr.Operand;
            if (copyTypeRef.IsValueType) {
              obj1 = simulationStack.Pop();
              triplets.Add(new Triplet(TripletOpCode.Assignment,
                                       new DeReference(simulationStack.Pop()),
                                       new DeReference(obj1))
                           );
            } else {
              // === ldind+stind?
              obj1 = simulationStack.Pop();
              triplets.Add(new Triplet(TripletOpCode.Assignment,
                                       new DeReference(simulationStack.Pop()),
                                       new DeReference(obj1))
                           );
            }
            break;
          case Code.Ldobj:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.GetOperandType(instr.Operand));
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
                                   new DeReference(simulationStack.Pop()))
                         );
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldstr:
            simulationStack.Push(instr.Operand);
            break;
          case Code.Newobj:
            Stack<object> ctorReverseStack = new Stack<object>();
            MethodReference ctorMethod = instr.Operand as MethodReference;
            //int ctorMethodHasThis = ctorMethod.HasThis ? 1 : 0;
            int ctorMethodHasThis = 0;
            
            for (int i = ctorMethod.Parameters.Count + ctorMethodHasThis; i > 0; i--)
              ctorReverseStack.Push(simulationStack.Pop());
            for (int i = ctorMethod.Parameters.Count + ctorMethodHasThis; i > 0; i--) {
              obj1 = ctorReverseStack.Pop();
              triplets.Add(new Triplet(TripletOpCode.PushParam, null, obj1));
            }
            tmpVarRef = GenerateTempVar(tempVariables, ctorMethod.DeclaringType);
            triplets.Add(new Triplet(TripletOpCode.New, tmpVarRef, ctorMethod));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Castclass:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, (TypeReference)instr.Operand);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, (TypeReference)instr.Operand, obj1));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Isinst:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, instr.Operand as TypeReference);  
            triplets.Add(new Triplet(TripletOpCode.As, tmpVarRef, obj1, instr.Operand as TypeReference));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Conv_R_Un:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.DoubleTypeRef); //TODO: Push "F"-type to evaluation stack 
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.DoubleTypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            break;

          case Code.Box:
            TypeReference typeTok = (TypeReference)instr.Operand;
            if (typeTok.IsValueType) {
              obj1 = simulationStack.Pop();
              tmpVarRef = GenerateTempVar(tempVariables, Helper.ObjectTypeRef); //TODO: Boxed-form type?
              triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.ObjectTypeRef, obj1)); //TODO: Use (TypeReference)instr.Operand to construct "&" pointer type; Unbox === address_of?
              //triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef, obj1)); //TODO: Use (TypeReference)instr.Operand to construct "&" pointer type; Unbox === address_of?
              simulationStack.Push(tmpVarRef);
            } else if (typeTok.IsByReference) {
              // the box instruction does returns val unchanged as obj
            } // else if (?nullable?) {
            //}
            break;
          case Code.Unbox:
            //TODO: Check implementation semantic described in spec.
            //TODO: Add test cases for box/unbox Nullable<T>
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.PointerTypeRef); //TODO: Make type valueTypePtr (&); (TypeReference)instr.Operand
            triplets.Add(new Triplet(TripletOpCode.AddressOf, tmpVarRef, obj1)); //TODO: Use (TypeReference)instr.Operand to construct "&" pointer type; Unbox === address_of?
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Unbox_Any:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, (TypeReference)instr.Operand);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, (TypeReference)instr.Operand, obj1));
            simulationStack.Push(tmpVarRef);
            break;

//          case Code.Throw: //tested in Throw.il
            // [ECMA-335: 4.31] throw - Throw an exception
            //
            // stack transition: ..., object -> ...
            //
            // The throw instruction throws the exception object (type O) on the stack and empties the stack.
            //
          case Code.Ldfld:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.GetOperandType(instr.Operand));
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef, 
                                   new CompositeMemberReference(obj1, (FieldReference)instr.Operand)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldflda:
            //TODO: Check - Use reference to GetOperandType(instr.Operand)?
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.PointerTypeRef);
            triplets.Add(new Triplet(TripletOpCode.AddressOf, tmpVarRef, new CompositeMemberReference(obj1, (FieldReference)instr.Operand)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Stfld:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment, 
                                     new CompositeMemberReference(obj1, (FieldReference)instr.Operand), obj2));
            break;
          case Code.Ldsfld:
            simulationStack.Push(instr.Operand);
            break;
          case Code.Ldsflda:
            //TODO: Check - Use reference to GetOperandType(instr.Operand)?
            tmpVarRef = GenerateTempVar(tempVariables, Helper.PointerTypeRef);
            triplets.Add(new Triplet(TripletOpCode.AddressOf, tmpVarRef, instr.Operand));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Stsfld:
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment, instr.Operand, obj1));
            break;
          case Code.Stobj:
            //TODO: Check if this implementation is correct with generic types
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment,
                                   new DeReference(simulationStack.Pop()),
                                   obj1)
                         );
            break;
//          case Code.Conv_Ovf_I1_Un:
//          case Code.Conv_Ovf_I2_Un:
//          case Code.Conv_Ovf_I4_Un:
//          case Code.Conv_Ovf_I8_Un:
//          case Code.Conv_Ovf_U1_Un:
//          case Code.Conv_Ovf_U2_Un:
//          case Code.Conv_Ovf_U4_Un:
//          case Code.Conv_Ovf_U8_Un:
//          case Code.Conv_Ovf_I_Un:
//          case Code.Conv_Ovf_U_Un:
          case Code.Newarr:
            object numElems = simulationStack.Pop();
            TypeReference eType = (TypeReference)instr.Operand;
            TypeReference arrType = new ArrayType(eType, 1);

            tmpVarRef = GenerateTempVar(tempVariables, arrType);
            triplets.Add(new Triplet(TripletOpCode.New, tmpVarRef, arrType, numElems));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldlen:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UIntPtrTypeRef); // native unsigned int
            triplets.Add(new Triplet(TripletOpCode.ArrayLength, tmpVarRef, simulationStack.Pop()));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldelema:
            //TODO: Check - Use reference to GetOperandType(instr.Operand)?
            tmpVarRef = GenerateTempVar(tempVariables, Helper.PointerTypeRef);
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.AddressOf, tmpVarRef,
              new ArrayElementReference(simulationStack.Pop(), obj1)) //TODO: Check: instr.Operand is unused?
            );
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldelem_I1:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int8TypeRef);
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
              new ArrayElementReference(simulationStack.Pop(), obj1)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldelem_U1:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt8TypeRef);
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
              new ArrayElementReference(simulationStack.Pop(), obj1)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldelem_I2:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int16TypeRef);
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
              new ArrayElementReference(simulationStack.Pop(), obj1)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldelem_U2:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt16TypeRef);
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
              new ArrayElementReference(simulationStack.Pop(), obj1)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldelem_I4:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int32TypeRef);
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
              new ArrayElementReference(simulationStack.Pop(), obj1)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldelem_U4:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt32TypeRef);
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
              new ArrayElementReference(simulationStack.Pop(), obj1)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldelem_I8:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int64TypeRef);
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
              new ArrayElementReference(simulationStack.Pop(), obj1)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldelem_I:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.IntPtrTypeRef);
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
              new ArrayElementReference(simulationStack.Pop(), obj1)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldelem_R4:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.SingleTypeRef);
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
              new ArrayElementReference(simulationStack.Pop(), obj1)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldelem_R8:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.DoubleTypeRef);
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
              new ArrayElementReference(simulationStack.Pop(), obj1)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldelem_Ref:
            obj1 = simulationStack.Pop();
            obj2 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.GetOperandType(obj2).GetElementType());
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
              new ArrayElementReference(obj2, obj1)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldelem_Any:
            tmpVarRef = GenerateTempVar(tempVariables, (TypeReference)instr.Operand);
            obj1 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment, tmpVarRef,
                                   new ArrayElementReference(simulationStack.Pop(), obj1)));
            simulationStack.Push(tmpVarRef);
            break;

          case Code.Stelem_Any: // instr.Operand is unused for now for this opcode.
          case Code.Stelem_I:
          case Code.Stelem_I1:
          case Code.Stelem_I2:
          case Code.Stelem_I4:
          case Code.Stelem_I8:
          case Code.Stelem_R4:
          case Code.Stelem_R8:
          case Code.Stelem_Ref:
            obj1 = simulationStack.Pop();
            obj2 = simulationStack.Pop();
            triplets.Add(new Triplet(TripletOpCode.Assignment,
              new ArrayElementReference(simulationStack.Pop(), obj2), obj1));
            break;

          case Code.Conv_Ovf_I1:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int8TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.Int8TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            triplets.Add(new Triplet(TripletOpCode.CheckOverflow));
            break;
          case Code.Conv_Ovf_U1:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt8TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.UInt8TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            triplets.Add(new Triplet(TripletOpCode.CheckOverflow));
            break;
          case Code.Conv_Ovf_I2:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int16TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.Int16TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            triplets.Add(new Triplet(TripletOpCode.CheckOverflow));
            break;
          case Code.Conv_Ovf_U2:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt16TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.UInt16TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            triplets.Add(new Triplet(TripletOpCode.CheckOverflow));
            break;
          case Code.Conv_Ovf_I4:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int32TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.Int32TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            triplets.Add(new Triplet(TripletOpCode.CheckOverflow));
            break;
          case Code.Conv_Ovf_U4:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt32TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.UInt32TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            triplets.Add(new Triplet(TripletOpCode.CheckOverflow));
            break;
          case Code.Conv_Ovf_I8:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int64TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.Int64TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            triplets.Add(new Triplet(TripletOpCode.CheckOverflow));
            break;
          case Code.Conv_Ovf_U8:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt64TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.UInt64TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            triplets.Add(new Triplet(TripletOpCode.CheckOverflow));
            break;    
//          case Code.Refanyval://tested in RefAnyValMkRefAny.il
          case Code.Ckfinite:
            triplets.Add(new Triplet(TripletOpCode.CheckFinite));
            break;
//          case Code.Mkrefany://tested in RefAnyTypeMkRefAny.il
          case Code.Ldtoken:
            simulationStack.Push(instr.Operand);
            break;
          case Code.Conv_U2:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt16TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.UInt16TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Conv_U1:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt8TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.UInt8TypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Conv_I:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.IntPtrTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.IntPtrTypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Conv_Ovf_I:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.IntPtrTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.IntPtrTypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            triplets.Add(new Triplet(TripletOpCode.CheckOverflow));
            break;
          case Code.Conv_Ovf_U:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UIntPtrTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.UIntPtrTypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            triplets.Add(new Triplet(TripletOpCode.CheckOverflow));
            break;
          case Code.Add_Ovf:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.OverflowArithmeticOperations(obj1, obj2));
            triplets.Add(new Triplet(TripletOpCode.Addition, tmpVarRef, obj1, obj2));
            simulationStack.Push(tmpVarRef);
            triplets.Add(new Triplet(TripletOpCode.CheckOverflow));
            break;
//          case Code.Add_Ovf_Un:
          case Code.Mul_Ovf:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.OverflowArithmeticOperations(obj1, obj2));
            triplets.Add(new Triplet(TripletOpCode.Multiplication, tmpVarRef, obj1, obj2));
            simulationStack.Push(tmpVarRef);
            triplets.Add(new Triplet(TripletOpCode.CheckOverflow));
            break;
//          case Code.Mul_Ovf_Un:
          case Code.Sub_Ovf:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.OverflowArithmeticOperations(obj1, obj2));
            triplets.Add(new Triplet(TripletOpCode.Substraction, tmpVarRef, obj1, obj2));
            simulationStack.Push(tmpVarRef);
            triplets.Add(new Triplet(TripletOpCode.CheckOverflow));
            break;
//          case Code.Sub_Ovf_Un:
//          case Code.Endfinally:
//          case Code.Leave: // tested in Leave.il
//          case Code.Leave_S:
          case Code.Conv_U:
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UIntPtrTypeRef);
            triplets.Add(new Triplet(TripletOpCode.Cast, tmpVarRef, Helper.UIntPtrTypeRef, obj1));
            simulationStack.Push(tmpVarRef);
            break;
//          case Code.Arglist:
//            tmpVarRef = GenerateTempVar(tempVariables, Helper.GetTypeReference(typeof(RuntimeArgumentHandle)));
//            triplets.Add(new Triplet(TripletOpCode.ArgList, tmpVarRef));
//            break;
          case Code.Ceq:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
              throw new Exception(InvalidILExceptionString);
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int32TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Equal, tmpVarRef, obj1, obj2));
            simulationStack.Push(tmpVarRef);
            break;
//          case Code.Cgt_Un:
          case Code.Cgt:
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
              throw new Exception(InvalidILExceptionString);
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int32TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Great, tmpVarRef, obj1, obj2));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Clt_Un: // Intentional fall through
            // [ECMA-335: 3.26] clt.un - Push 1 (of type int32) if value1 < value2, unsigned or unordered, 
            //                           else push 0.
            //
            // stack transition: ..., value1, value2 -> ..., result
            //
            // The clt.un instruction compares value1 and value2. A value of 1 (of type int32) is pushed on the 
            // stack if:
            //   • for floating-point numbers, either value1 is strictly less than value2, or value1 is not 
            //     ordered with respect to value2.
            //   • for integer values, value1 is strictly less than value2 when considered as unsigned numbers.
            //   Otherwise, 0 (of type int32) is pushed on the stack.
            //
            // As per IEC 60559:1989, infinite values are ordered with respect to normal numbers 
            // (e.g., +infinity > 5.0 > - infinity).
            //
            // The acceptable operand types are encapsulated in Table 4: Binary Comparison or Branch Operations.
            //
          case Code.Clt:
            // [ECMA-335: 3.25] clt - Push 1 (of type int32) if value1 < value2, else push 0
            //
            // stack transition: ..., value1, value2 -> ..., result
            //
            // The clt instruction compares value1 and value2. If value1 is strictly less than value2, then 1
            // (of type int32) is pushed on the stack. Otherwise, 0 (of type int32) is pushed on the stack.
            //
            // For floating-point numbers, clt will return 0 if the numbers are unordered (that is, one or 
            // both of the arguments are NaN).
            //
            // As per IEC 60559:1989, infinite values are ordered with respect to normal numbers 
            // (e.g., +infinity > 5.0 > - infinity).
            //
            // The acceptable operand types are encapsulated in Table 4: Binary Comparison or Branch Operations.
            //
            obj2 = simulationStack.Pop();
            obj1 = simulationStack.Pop();
            if (!Helper.BinaryComparisonOrBranchOperations(obj1, obj2))
              throw new Exception(InvalidILExceptionString);
            tmpVarRef = GenerateTempVar(tempVariables, Helper.Int32TypeRef);
            triplets.Add(new Triplet(TripletOpCode.Less, tmpVarRef, obj1, obj2));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldftn:
            //TODO: Check - Use reference to GetOperandType(instr.Operand)?
            tmpVarRef = GenerateTempVar(tempVariables, Helper.PointerTypeRef);
            triplets.Add(new Triplet(TripletOpCode.AddressOf, tmpVarRef, (MethodReference)instr.Operand));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldvirtftn:
            //TODO: Check - Use reference to GetOperandType(instr.Operand)?
            obj1 = simulationStack.Pop();
            tmpVarRef = GenerateTempVar(tempVariables, Helper.PointerTypeRef);
            triplets.Add(new Triplet(TripletOpCode.AddressOf, tmpVarRef, new CompositeMemberReference(obj1, (MethodReference)instr.Operand)));
            simulationStack.Push(tmpVarRef);
            break;
          case Code.Ldarg_S: // Intentional fall through
          case Code.Ldarg:
            if (method.HasThis && ((ParameterReference)instr.Operand).Index == 0)
              simulationStack.Push(method.Body.ThisParameter);
            else
              simulationStack.Push(method.Parameters[((ParameterReference)instr.Operand).Index - paramOffset]);
            break;
          case Code.Starg_S: // Intentional fall through
          case Code.Starg:
            if (method.HasThis && ((ParameterReference)instr.Operand).Index == 0)
              triplets.Add(new Triplet(TripletOpCode.Assignment, method.Body.ThisParameter, simulationStack.Pop()));
            else
              triplets.Add(new Triplet(TripletOpCode.Assignment, method.Parameters[((ParameterReference)instr.Operand).Index - paramOffset], simulationStack.Pop()));
            break;
          case Code.Ldloc_S: // Intentional fall through
          case Code.Ldloc:
            simulationStack.Push(instr.Operand);
            break;
          case Code.Stloc_S: // Intentional fall through
          case Code.Stloc:
            triplets.Add(new Triplet(TripletOpCode.Assignment, instr.Operand, simulationStack.Pop()));
            break;
//          case Code.Localloc:
//          case Code.Endfilter:
//          case Code.Unaligned:
//          case Code.Volatile:
//          case Code.Tail:
          case Code.Initobj:
            TypeReference initTypeRef = (TypeReference)instr.Operand;
            if (initTypeRef.IsValueType) {
              triplets.Add(new Triplet(TripletOpCode.DefaultInit,
                                       null,
                                       new DeReference(simulationStack.Pop()),
                                       initTypeRef)
                           );
            } else {
              triplets.Add(new Triplet(TripletOpCode.Assignment,
                                      new DeReference(simulationStack.Pop()),
                                      null)
                          );
            }
            break;
//          case Code.Constrained: //tested in Generics_ReadOnlyPrefixAndConstrainedPrefix.il
//          case Code.Cpblk: // tested in CpblkAndInitblk.il
            // [ECMA-335: 3.30] cpblk – copy data from memory to memory
            //
            // stack transition: ..., destaddr, srcaddr, size -> ...
            //
            // The cpblk instruction copies size (of type unsigned int32) bytes from address srcaddr
            // (of type native int, or &) to address destaddr (of type native int, or &). The behavior 
            // of cpblk is unspecified if the source and destination areas overlap.
            //
            // cpblk assumes that both destaddr and srcaddr are aligned to the natural size of the
            // machine (but see the unaligned. prefix instruction). The operation of the cpblk instruction
            // can be altered by an immediately preceding volatile. or unaligned. prefix instruction.
            //
            // [Rationale: cpblk is intended for copying structures (rather than arbitrary byte-runs). 
            // All such structures, allocated by the CLI, are naturally aligned for the current platform. 
            // Therefore, there is no need for the compiler that generates cpblk instructions to be aware
            // of whether the code will eventually execute on a 32-bit or 64-bit platform. end rationale]
            //
//          case Code.Initblk: // tested in CpblkAndInitblk.il
            // [ECMA-335: 3.36] initblk – Set all bytes in a block of memory to a given byte value.
            //
            // stack transition: ..., addr, value, size -> ...
            //
            // The initblk instruction sets size (of type unsigned int32) bytes starting at addr (of type
            // native int, or &) to value (of type unsigned int8). initblk assumes that addr is aligned 
            // to the natural size of the machine (but see the unaligned. prefix instruction).
            // 
            // [Rationale: initblk is intended for initializing structures (rather than arbitrary byte-runs).
            // All such structures, allocated by the CLI, are naturally aligned for the current platform.
            // Therefore, there is no need for the compiler that generates initblk instructions to be aware
            // of whether the code will eventually execute on a 32-bit or 64-bit platform. end rationale]
            //
            // The operation of the initblk instructions can be altered by an immediately preceding volatile.
            // or unaligned. prefix instruction.
            //
//          case Code.No:
//          case Code.Rethrow: //tested in Rethrow.il
            // [ECMA-335: 4.24] rethrow – rethrow the current exception.
            //
            // stack transition: ...,  -> ...,
            //
            // The rethrow instruction is only permitted within the body of a catch handler 
            // (see Partition I). It throws the same exception that was caught by this handler. A rethrow 
            // does not change the stack trace in the object.
            //
          case Code.Sizeof:
            tmpVarRef = GenerateTempVar(tempVariables, Helper.UInt32TypeRef);
            triplets.Add(new Triplet(TripletOpCode.SizeOf, tmpVarRef, instr.Operand));
            simulationStack.Push(tmpVarRef);
            break;
//          case Code.Refanytype: //tested in RefAnyTypeMkRefAny.il
//          case Code.Readonly: //tested in Generics_ReadOnlyPrefixAndConstrainedPrefix.il

          default:
            string msg = String.Format("Unknown instruction: {0}\n", instr.OpCode.ToString());
            if (triplets.Count > 0) {
              ThreeAddressCode partiallyBuiltTac = new ThreeAddressCode(method, triplets[0], triplets, tempVariables);
              msg += String.Format("\n Model built partially:\n{0}", partiallyBuiltTac);
            }
            throw new NotImplementedException(msg);
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
      
      return new ThreeAddressCode(method, triplets[0], triplets, tempVariables);
    }
  }

  //TODO: Add support to Native Float (R)
  public static class Helper
  {
    private const string InvalidILExceptionString = "TAC builder: Invalid IL!";
    private const string UnsupportedTypeExceptionString = "TAC builder: Unsupported type!";

    public static readonly TypeReference BoolTypeRef;
    public static readonly TypeReference Int8TypeRef;
    public static readonly TypeReference Int16TypeRef;
    public static readonly TypeReference Int32TypeRef;
    public static readonly TypeReference Int64TypeRef;
    public static readonly TypeReference SingleTypeRef;
    public static readonly TypeReference DoubleTypeRef;
    public static readonly TypeReference UInt8TypeRef;
    public static readonly TypeReference UInt16TypeRef;
    public static readonly TypeReference UInt32TypeRef;
    public static readonly TypeReference UInt64TypeRef;
    public static readonly TypeReference IntPtrTypeRef;
    public static readonly TypeReference UIntPtrTypeRef;
    public static readonly TypeReference PointerTypeRef;
    public static readonly TypeReference ObjectTypeRef;

    static Helper()
    {
      BoolTypeRef = new TypeReference("System", "Bool", null, true);
      Int8TypeRef = new TypeReference("System", "Int8", null, true);
      Int16TypeRef = new TypeReference("System", "Int16", null, true);
      Int32TypeRef = new TypeReference("System", "Int32", null, true);
      Int64TypeRef = new TypeReference("System", "Int64", null, true);
      SingleTypeRef = new TypeReference("System", "Single", null, true);
      DoubleTypeRef = new TypeReference("System", "Double", null, true);
      UInt8TypeRef = new TypeReference("System", "UInt8", null, true);
      UInt16TypeRef = new TypeReference("System", "UInt16", null, true);
      UInt32TypeRef = new TypeReference("System", "UInt32", null, true);
      UInt64TypeRef = new TypeReference("System", "UInt64", null, true);
      IntPtrTypeRef = new TypeReference("System", "IntPtr", null, true);
      UIntPtrTypeRef = new TypeReference("System", "UIntPtr", null, true);
      //TODO: Check CLI managed pointer "&" type spec; Check last param: false or true?
      PointerTypeRef = new TypeReference("Mono.Cecil", "PointerType", null, false); 
      ObjectTypeRef = new TypeReference("System", "Object", null, false); 
    }
    
    public static TypeReference GetTypeReference(Type t)
    {
      return new TypeReference(t.Namespace, t.Name, null, t.IsValueType);
    }
    
    public static TypeReference GetOperandType(object op)
    {
      Type t = op.GetType();
      if (op is VariableReference) return (op as VariableReference).VariableType;
      if (op is ParameterReference) return (op as ParameterReference).ParameterType;
      if (op is FieldReference) return (op as FieldReference).FieldType;
      return new TypeReference(t.Namespace, t.Name, null, t.IsValueType);
    }
    
    public static int GetTypeKind(TypeReference tr)
    {
      switch (tr.FullName) {
        case "System.Int8": return 0;
        case "System.Int16": return 0;
        case "System.Int32": return 0;
        case "System.Int64": return 1;
        case "System.IntPtr": return 2;
        case "System.Single": return 3;
        case "System.Double": return 4;
      }
      
      if (tr.IsPointer) return 5;
      if (!tr.IsValueType) return 6;
      return -1;
    }
    
    public static int GetTypeKindUn(TypeReference tr)
    {
        switch (tr.FullName) {
            case "System.Int8": return 0;
            case "System.Int16": return 0;
            case "System.Int32": return 0;
            case "System.UInt8": return 0;
            case "System.UInt16": return 0;
            case "System.UInt32": return 0;
            case "System.Int64": return 1;
            case "System.UInt64": return 1;
            case "System.IntPtr": return 2;
            case "System.UIntPtr": return 2;
            case "System.Single": return 3;
            case "System.Double": return 4;
        }
        
        if (tr.IsPointer) return 5;
        if (!tr.IsValueType) return 6;
        return -1;
    }
    
    // Binary numeric operations
    
    public readonly static Type[,] BinaryNumericOperationsResultTypes =
      {
        //              Int32           Int64           IntPtr                Single          Double          &                    Obj
        /* Int32  */  { typeof(Int32),  null,           typeof(IntPtr),       null,           null,           typeof(PointerType), null },
        /* Int64  */  { null,           typeof(Int64),  null,                 null,           null,           null,                null },
        /* IntPtr */  { typeof(IntPtr), null,           typeof(IntPtr),       null,           null,           typeof(PointerType), null },
        /* Single */  { null,           null,           null,                 typeof(Single), null,           null,                null },
        /* Double */  { null,           null,           null,                 null,           typeof(Double), null,                null },
        /* &      */  { typeof(PointerType), null,      typeof(PointerType),  null,           null,           typeof(IntPtr),      null },
        /* Obj    */  { null,           null,           null,                 null,           null,           null,                null }
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
      int op1Kind = GetTypeKindUn(GetOperandType(op1));
      int op2Kind = GetTypeKindUn(GetOperandType(op2));
      bool result = BinaryComparisonOrBranchOperationsResultTypes[op1Kind, op2Kind];

      //TODO: PointerType and Object/Object result are valid only with beq[.s], bne.un[.s] and 
      // ceq IL instructions. See CIL specification Table III.4

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

    public readonly static Type[,] IntegerOperationsResultTypesUn =
    {
        //              Int32             Int64           IntPtr            Single  Double  &       Obj
        /* Int32  */  { typeof(UInt32),   null,           typeof(UIntPtr),  null,   null,   null,   null },
        /* Int64  */  { null,             typeof(UInt64), null,             null,   null,   null,   null },
        /* IntPtr */  { typeof(UIntPtr),  null,           typeof(UIntPtr),  null,   null,   null,   null },
        /* Single */  { null,             null,           null,             null,   null,   null,   null },
        /* Double */  { null,             null,           null,             null,   null,   null,   null },
        /* &      */  { null,             null,           null,             null,   null,   null,   null },
        /* Obj    */  { null,             null,           null,             null,   null,   null,   null }
    };
    
    public static TypeReference IntegerOperationsUn(object op1, object op2)
    {
        Type resultType = IntegerOperationsResultTypesUn[GetTypeKindUn(GetOperandType(op1)), GetTypeKindUn(GetOperandType(op2))];
        
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
      Type resultType = ShiftOperationsResultTypes[GetTypeKindUn(GetOperandType(op1)), GetTypeKindUn(GetOperandType(shiftBy))];

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
