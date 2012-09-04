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
  }
}
