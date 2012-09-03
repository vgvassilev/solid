/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

namespace SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode {

  public enum OpCode {
    Assignment,
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

    private OpCode opcode;
    private object operand;

    internal Triplet(int offset, OpCode opCode)
    {
      this.offset = offset;
      this.opcode = opCode;
    }

    internal Triplet(OpCode opcode, object operand)
    {
      this.opcode = opcode;
      this.operand = operand;
    }

    public static Triplet Create(OpCode opcode)
    {
      return new Triplet(opcode, null);
    }

//    public static Triplet Create(OpCode opcode, ParameterDefinition parameter)
//    {
//      return new Triplet(opcode, parameter);
//    }
  }
}
