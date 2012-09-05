/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

namespace SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode {

  public enum TripletOpCode {
    // Basic
    Assignment,     // result = op2
    
    // Array deref etc
    //...
    
    // Aritmetic
    Addition,       // result = op2 + op3
    Substraction,   // result = op2 - op3
    Multiplication, // result = op2 * op3
    Division,       // result = op2 / op3
    //...

    // Logic
    Less,           // result = op2 < op3
    Equal,          // result = op2 == op3
    And,            // result = op2 && op3
    Or,             // result = op2 || op3
    //...

    // Control
    Goto,           // goto result/label
    IfFalse,        // iffalse op1 goto result/label
    IfTrue,         // iftrue op1 goto result/label
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

    public override string ToString ()
    {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      if (result != null)
        sb.AppendFormat("{0} = ", result.ToString());
      if (operand1 != null)
        sb.AppendFormat("{0}", operand1.ToString());
      switch(opcode) {
        case TripletOpCode.Addition: sb.Append(" +"); break;
        case TripletOpCode.And: sb.Append(" &&"); break;
        case TripletOpCode.Assignment: sb.Append(" ="); break;
        case TripletOpCode.Call: sb.Append(" call"); break;
        case TripletOpCode.CallVirt: sb.Append(" callvirtual"); break;
        case TripletOpCode.Division: sb.Append(" /"); break;
        case TripletOpCode.Equal: sb.Append(" =="); break;
        case TripletOpCode.Goto: sb.Append(" goto"); break;
        case TripletOpCode.IfFalse: sb.Append(" ifFalse"); break;
        case TripletOpCode.IfTrue: sb.Append(" ifTrue"); break;
        case TripletOpCode.Less: sb.Append(" <"); break;
        case TripletOpCode.Multiplication: sb.Append(" *"); break;
        case TripletOpCode.Or: sb.Append(" ||"); break;
        case TripletOpCode.PushParam: sb.Append(" pushParam"); break;
        case TripletOpCode.Return: sb.Append(" return"); break;
        case TripletOpCode.Substraction: sb.Append(" -"); break;
        default: sb.Append(" UNKNOWN "); break;
      }
      if (operand2 != null)
        sb.AppendFormat("{0}", operand2.ToString());

      return sb.ToString();
    }
  }
}
