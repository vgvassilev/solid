/*
 * $Id: SampleClass.cs 993 2013-05-25 17:14:45Z vvassilev $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using SolidOpt.Services.Transformations;
using SolidOpt.Services.Transformations.Multimodel;
using SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree;
using SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode;


namespace SolidOpt.Services.Transformations.Multimodel.TACtoAST
{


  public class TACtoASTTransformer : DecompilationStep, ITransform<ThreeAddressCode, AstMethodDefinition>
  {

    public TACtoASTTransformer()
    {
    }

    #region ITransform implementation

    AstMethodDefinition ITransform<ThreeAddressCode, AstMethodDefinition>.Transform(ThreeAddressCode source) {
      throw new NotImplementedException();
    }

    #endregion

    #region implemented abstract members of Step

    public override object Process(object codeModel) {
      throw new NotImplementedException();
    }

    public override Type GetSourceType() {
      return typeof(ThreeAddressCode);
    }

    public override Type GetTargetType() {
      return typeof(AstMethodDefinition);
    }

    #endregion

    private AstMethodDefinition ProcessInternal(ThreeAddressCode codeModel) {
      AstMethodDefinition result = new AstMethodDefinition();
      /*Triplet t = codeModel.Root;
      //FunctionDeclaration decl = new FunctionDecl();
      do {
        switch (t.Opcode) {
          // Basic
          case TripletOpCode.Assignment: // result = op1
            // Create a VariableDeclaration
            //
            //t.Operand1 + t.Operand2;
            break;

/*
          // Array deref etc
          //...

          // Aritmetic
          Addition,       // result = op1 + op2
          Substraction,   // result = op1 - op2
          Multiplication, // result = op1 * op2
          Division,       // result = op1 / op2
          Reminder,       // result = op1 % op2
          Negate,         // result = - op1
          And,            // result = op1 & op2
          Or,             // result = op1 | op2
          Xor,            // result = op1 ^ op2
          Not,            // result = ! op1
          ShiftLeft,      // result = op1 << op2
          ShiftRight,     // result = op1 >> op2
          CheckOverflow,  // checkoverflow
          //...

          // Cast
          Cast,           // result = (op1) op2

          // Logic
          Equal,          // result = op1 == op2
          Less,           // result = op1 < op2
          Great,          // result = op1 > op2
          //...

          // Control
          Goto,           // goto op1/label
          IfFalse,        // iffalse op1 goto op2/label
          IfTrue,         // iftrue op1 goto op2/label
          Switch,         // switch op1 goto op2/array/labels
          //...

          // Methods
          Call,           // result = call op1/method/signature
          CallVirt,       // result = callvirt op1/method/signature
          PushParam,      // pushparam op1
          Return,         // return op1
          //...

          // Object model
          //...

          // Exceptions handling
          //...

          // Other
          Nop             // nop
*//*
          dafault:
            throw new NotImplementedException("Triplet opcode not supported yet.");
        }
      }
      while ((t = t.Next) != null);
  */
      return result;

    }
  }
}
