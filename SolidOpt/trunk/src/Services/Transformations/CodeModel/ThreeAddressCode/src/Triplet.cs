/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Text;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode {

  public enum TripletOpCode {
    // Basic

    /// result = op1
    Assignment,
    /// result = addressof op
    AddressOf,
    
    // Array deref etc

    /// result = length op1
    ArrayLength,

    // Aritmetic

    /// result = op1 + op2
    Addition,
    /// result = op1 - op2
    Substraction,
    /// result = op1 * op2
    Multiplication,
    /// result = op1 / op2
    Division,
    /// result = op1 % op2
    Reminder,
    /// result = - op1
    Negate,
    /// result = op1 & op2
    And,
    /// result = op1 | op2
    Or,
    /// result = op1 ^ op2
    Xor,
    /// result = ! op1
    Not,
    /// result = op1 << op2
    ShiftLeft,
    /// result = op1 >> op2
    ShiftRight,
    /// result = sizeof op1/type
    SizeOf,
    /// checkoverflow
    CheckOverflow,
    /// checkfinite
    CheckFinite,
    
    // Cast

    /// result = (op1/type) op2
    Cast,
    
    // Logic

    /// result = op1 == op2
    Equal,
    /// result = op1 < op2
    Less,
    /// result = op1 > op2
    Great,
    
    // Control

    /// goto op1/label
    Goto,
    /// iffalse op1 goto op2/label
    IfFalse,
    /// iftrue op1 goto op2/label
    IfTrue,
    /// switch op1 goto op2/array/labels
    Switch,
    
    // Methods

    /// result = call op1/method
    Call,
    /// result = callvirt op1/method
    CallVirt,
    /// pushparam op1
    PushParam,
    /// return op1
    Return,
    
    // Object model

    /// result = new op1/method
    /// result = new op1/type[op2/array_elements_count]
    New,
    /// result = op1 as op2/type
    As,
    /// result = token op1/token
    Token,
    /// defaultinit op1/memory_to_init op2/type
    DefaultInit,

    // Exceptions handling
    //...

    // Other

    /// nop
    Nop
  }

  public class Triplet
  {
    public int offset = 0;

    private Triplet previous;
    public Triplet Previous {
      get { return this.previous; }
      set { this.previous = value; }
    }

    private Triplet next;
    public Triplet Next {
      get { return this.next; }
      set { this.next = value; }
    }

    private TripletOpCode opcode;
    public TripletOpCode Opcode {
      get { return opcode; }
      set { opcode = value; }
    }

    private object result;
    public object Result {
      get { return result; }
      set { result = value; }
    }

    private object operand1;
    public object Operand1 {
      get { return operand1; }
      set { operand1 = value; }
    }

    private object operand2;
    public object Operand2 {
        get { return operand2; }
        set { operand2 = value; }
    }

    public Triplet(TripletOpCode opcode)
    {
      this.opcode = opcode;
    }
    
    public Triplet(int offset, TripletOpCode opcode)
    {
      this.offset = offset;
      this.opcode = opcode;
    }

    public Triplet(TripletOpCode opcode, object result)
    {
      this.opcode = opcode;
      this.result = result;
    }

    public Triplet(int offset, TripletOpCode opcode, object result)
    {
        this.offset = offset;
        this.opcode = opcode;
        this.result = result;
    }
    
    public Triplet(TripletOpCode opcode, object result, object operand1)
    {
      this.opcode = opcode;
      this.result = result;
      this.operand1 = operand1;
    }    
    
    public Triplet(int offset, TripletOpCode opcode, object result, object operand1)
    {
        this.offset = offset;
        this.opcode = opcode;
        this.result = result;
        this.operand1 = operand1;
    }    
    
    public Triplet(TripletOpCode opcode, object result, object operand1, object operand2)
    {
        this.opcode = opcode;
        this.result = result;
        this.operand1 = operand1;
        this.operand2 = operand2;
    }
    
    public Triplet(int offset, TripletOpCode opcode, object result, object operand1, object operand2)
    {
        this.offset = offset;
        this.opcode = opcode;
        this.result = result;
        this.operand1 = operand1;
        this.operand2 = operand2;
    }

    private static string op(object obj)
    {
      if (obj is string) return "\"" + obj.ToString() + "\"";  //TODO: Escape string
      if (obj is Triplet) return "L" + ((Triplet)obj).offset;
      if (obj is Triplet[]) {
        StringBuilder sb = new StringBuilder();
        foreach (Triplet t in (obj as Triplet[])) {
          sb.Append(", ");
          sb.Append(op(t));
        }
        sb.Remove(0, 2);
        return sb.ToString();
      }
      if (obj is ParameterReference && (obj as ParameterReference).Index == -1)
        return "this";
      if (obj is CompositeFieldReference) {
        CompositeFieldReference cfr = obj as CompositeFieldReference;
        return string.Format("{0}.{1}", cfr.Instance, cfr.Field.Name);
      }
      if (obj is ArrayElementReference) {
        ArrayElementReference aer = obj as ArrayElementReference;
        return string.Format("{0}[{1}]", aer.Array, aer.Index);
      }
      if (obj is DeReference) {
        DeReference der = obj as DeReference;
        return string.Format("deref {0}", der.AddressTo);
      }
      if (obj is FieldReference) {
        FieldReference fldRef = (FieldReference)obj;
        return string.Format("{0}.{1}", fldRef.DeclaringType.FullName, fldRef.Name);
      }
      return obj.ToString();
    }

    public override string ToString()
    {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      if (result != null)
        sb.AppendFormat("{0} = ", op(result));
      switch(opcode) {
        case TripletOpCode.AddressOf:
          sb.AppendFormat("addressof {0}", op(operand1));
          break;
        case TripletOpCode.Addition:
          sb.AppendFormat("{0} + {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.And:
          sb.AppendFormat("{0} & {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.Assignment:
          sb.AppendFormat("{0}", op(operand1));
          break;
        case TripletOpCode.Call:
          sb.AppendFormat("call {0}", op(operand1));
          break;
        case TripletOpCode.CallVirt:
          sb.AppendFormat("callvirt {0}", op(operand1));
          break;
        case TripletOpCode.CheckOverflow:
          sb.Append("checkoverflow");
          break;
        case TripletOpCode.Cast:
          sb.AppendFormat("({0}) {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.Division:
          sb.AppendFormat("{0} / {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.Equal:
          sb.AppendFormat("{0} == {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.Goto:
          sb.AppendFormat("goto {0}", op(operand1));
          break;
        case TripletOpCode.Great:
          sb.AppendFormat("{0} > {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.IfFalse:
          sb.AppendFormat("iffalse {0} goto {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.IfTrue:
          sb.AppendFormat("iftrue {0} goto {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.Less:
          sb.AppendFormat("{0} < {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.Multiplication:
          sb.AppendFormat("{0} * {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.Negate:
            sb.AppendFormat("- {0}", op(operand1));
            break;
        case TripletOpCode.Nop:
          sb.Append("nop");
          break;
        case TripletOpCode.Not:
            sb.AppendFormat("! {0}", op(operand1));
            break;
        case TripletOpCode.Or:
          sb.AppendFormat("{0} | {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.PushParam:
          sb.AppendFormat("pushparam {0}", op(operand1));
          break;
        case TripletOpCode.Return:
          sb.Append("return");
          if (operand1 != null) sb.AppendFormat(" {0}", op(operand1));
          break;
        case TripletOpCode.Reminder:
          sb.AppendFormat("{0} % {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.ShiftLeft:
            sb.AppendFormat("{0} << {1}", op(operand1), op(operand2));
            break;
        case TripletOpCode.ShiftRight:
            sb.AppendFormat("{0} >> {1}", op(operand1), op(operand2));
            break;
        case TripletOpCode.Substraction:
          sb.AppendFormat("{0} - {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.Switch:
            sb.AppendFormat("switch {0} goto {1}", op(operand1), op(operand2));
            break;

        case TripletOpCode.New:
          sb.AppendFormat("new {0}", op(operand1));
          if (operand1 is TypeReference) {
            sb.Insert(sb.ToString().IndexOf("[") + 1, op(operand2));
          }
          break;
        case TripletOpCode.As:
          sb.AppendFormat("{0} as {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.Token:
          sb.AppendFormat("token {0}", op(operand1));
          break;
        case TripletOpCode.ArrayLength:
          sb.AppendFormat("length {0}", op(operand1));
          break;
        case TripletOpCode.SizeOf:
          sb.AppendFormat("sizeof {0}", op(operand1));
          break;
        case TripletOpCode.CheckFinite:
          sb.Append("checkfinite");
          break;
        case TripletOpCode.DefaultInit:
          sb.AppendFormat("dafaultinit {0} {1}", op(operand1), op(operand2));
          break;

        default:
          sb.Append(" UNKNOWN ");
          break;
      }
      
      return sb.ToString();
    }
  }
}
