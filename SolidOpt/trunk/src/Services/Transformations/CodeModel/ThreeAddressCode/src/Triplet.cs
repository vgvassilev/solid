/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

namespace SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode {

  public enum TripletOpCode {
    Assignment,
    Add,
    Sub,
    Multiplication,
    Call,
    Push
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
    private object operand1;
    private object operand2;
    private object operand3;

    public Triplet(int offset, TripletOpCode opCode)
    {
      this.offset = offset;
      this.opcode = opCode;
    }

    public Triplet(TripletOpCode opcode, object operand1)
    {
      this.opcode = opcode;
      this.operand1 = operand1;
    }

    public Triplet(TripletOpCode opcode, object operand1, object operand2)
    {
      this.opcode = opcode;
      this.operand1 = operand1;
      this.operand2 = operand2;
    }    

    public Triplet(TripletOpCode opcode, object operand1, object operand2, object operand3)
    {
        this.opcode = opcode;
        this.operand1 = operand1;
        this.operand2 = operand2;
        this.operand3 = operand3;
    }

/*
    public static Triplet Create(TripletOpCode opcode)
    {
      return new Triplet(opcode);
    }

        public static Triplet Create(TripletOpCode opcode, object operand1)
    {
        return new Triplet(opcode, operand1);
    }

    public static Triplet Create(TripletOpCode opcode,object operand1, object operand2)
    {
      return new Triplet(opcode, operand1, operand2);
    }

    public static Triplet Create(TripletOpCode opcode, object operand1, object operand2, object operand3)
    {
        return new Triplet(opcode, operand1, operand2, operand3);
    }
*/
  }
}
