/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

namespace SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode {

  public enum TripletOpCode {
    // Basic
    Assignment,     // result = op1
    
    // Array deref etc
    //...
    
    // Aritmetic
    Addition,       // result = op1 + op2
    Substraction,   // result = op1 - op2
    Multiplication, // result = op1 * op2
    Division,       // result = op1 / op2
    Reminder,       // result = op1 % op2
    And,            // result = op1 & op2
    Or,             // result = op1 | op2
    Xor,            // result = op1 ^ op2
    //...

    // Logic
    Less,           // result = op1 < op2
    Equal,          // result = op1 == op2
    //...

    // Control
    Goto,           // goto op1/label
    IfFalse,        // iffalse op1 goto op2/label
    IfTrue,         // iftrue op1 goto op2/label
    //...

    // Methods
    Call,           // result = call op1/method
    CallVirt,       // result = call op1/object op2/method
    PushParam,      // push op1
    Return          // return op1
    //...
  }

  public class Triplet
  {
    public int offset = 0;

    private Triplet previous;
    public Triplet Previous {
      get { return this.previous; }
    }

    private Triplet next;
    public Triplet Next {
      get { return this.next; }
    }

    private TripletOpCode opcode;
    private object result;
    private object operand1;
    private object operand2;

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
      return obj.ToString();
    }

    public override string ToString()
    {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      if (result != null)
        sb.AppendFormat("{0} = ", op(result));
      switch(opcode) {
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
          sb.AppendFormat("callvirt {0} {1}", op(operand1), op(operand2));
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
        case TripletOpCode.Or:
          sb.AppendFormat("{0} | {1}", op(operand1), op(operand2));
          break;
        case TripletOpCode.PushParam:
          sb.AppendFormat("pushparam {0}", op(operand1));
          break;
        case TripletOpCode.Return:
          sb.AppendFormat("return{0}", operand1==null ? "" : " "+op(operand1));
          break;
        case TripletOpCode.Substraction:
          sb.AppendFormat("{0} + {1}", op(operand1), op(operand2));
          break;
        default:
          sb.Append(" UNKNOWN ");
          break;
      }
      
      return sb.ToString();
    }
  }
}
