/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using Gtk;
using Mono.Cecil.Cil;
using System;

namespace SolidReflector.Plugins.ILVisualizer
{
  public class ILFormatter
  {
    private int indent = 0;
    private Gtk.TextBuffer Out = null;
    private Gtk.TextIter endIter;

    public ILFormatter(Gtk.TextBuffer Out) {
      this.Out = Out;
      this.endIter = Out.EndIter;

      CreateTags(Out);
    }

    private void CreateTags(TextBuffer buffer) {
      TextTag tag = new TextTag("Keywords");
      tag.Foreground = "blue";
      buffer.TagTable.Add(tag);

      tag = new TextTag("MethodAttributes");
      tag.Foreground = "blue";
      buffer.TagTable.Add(tag);

      tag = new TextTag("Exceptions");
      tag.Foreground = "brown";
      buffer.TagTable.Add(tag);

      tag = new TextTag("Types");
      tag.Foreground = "darkgreen";
      buffer.TagTable.Add(tag);

      tag = new TextTag("Names");
      tag.Foreground = "black";
      tag.Weight = Pango.Weight.Bold;
      buffer.TagTable.Add(tag);

      tag = new TextTag("ImplementationAttributes");
      tag.Foreground = "darkred";
      buffer.TagTable.Add(tag);

      tag = new TextTag("Label");
      tag.Foreground = "darkgreen";
      tag.Style = Pango.Style.Italic;
      tag.Weight = Pango.Weight.Bold;
      buffer.TagTable.Add(tag);
    }

    public void Indent() {
      indent++;
    }

    public void Outdent() {
      indent--;
    }

    public void NewLine() {
      WriteNewLine();
    }

    public void WriteInstruction(Instruction inst) {
      WriteIndent();
      WriteInstLabel(inst);

      // Copied and modified for Mono.Cecil.Cil.Instruction.ToString()
      Out.Insert(ref endIter, ": ");
      Out.Insert(ref endIter, inst.OpCode.Name);

      if (inst.Operand != null) {
        Out.Insert(ref endIter, " ");

        switch (inst.OpCode.OperandType) {
        case OperandType.ShortInlineBrTarget:
        case OperandType.InlineBrTarget:
          WriteInstLabel((Instruction) inst.Operand);
          break;
        case OperandType.InlineSwitch:
          var labels = (Instruction[]) inst.Operand;
          for (int i = 0; i < labels.Length; i++) {
            if (i > 0)
              Out.Insert(ref endIter,",");

            WriteInstLabel(labels[i]);
          }
          break;
        case OperandType.InlineString:
          Out.Insert(ref endIter, "\"");
          Out.Insert(ref endIter, inst.Operand.ToString());
          Out.Insert(ref endIter, "\"");
          break;
        default:
          Out.Insert(ref endIter, inst.Operand.ToString());
          break;
        }
      }

      WriteNewLine();
    }

    public void WriteInstLabel(Instruction inst) {
      Out.InsertWithTagsByName(ref endIter, "IL_", "Label");
      Out.InsertWithTagsByName(ref endIter, inst.Offset.ToString ("x4") + " ", "Label");
    }

    public void WriteKeyword(string keyword) {
      WriteIndent();
      Out.InsertWithTagsByName(ref endIter, keyword + " ", "Keywords");
    }

    public void WriteExceptionDirective(string except) {
      WriteIndent();
      Out.InsertWithTagsByName(ref endIter, except + " ", "Exceptions");
      WriteNewLine();
    }

    public void WriteMethodAttribute(string attr) {
      Out.InsertWithTagsByName(ref endIter, attr + " ", "MethodAttributes");
    }

    public void WriteImplementationAttribute(string attr) {
      Out.InsertWithTagsByName(ref endIter, attr + " ", "ImplementationAttributes");
    }

    public void WriteType(string type) {
      Out.InsertWithTagsByName(ref endIter, type + " ", "Types");
    }

    public void WriteName(string ident) {
      Out.InsertWithTagsByName(ref endIter, ident, "Names");
    }

    public void Write(string text) {
      Out.Insert(ref endIter, text);
    }

    private void WriteIndent() {
      for(uint i = 0; i < indent; i++) {
        Out.Insert(ref endIter, "\t");
      }
    }

    private void WriteNewLine() {
      Out.Insert(ref endIter, "\n");
    }
  }
}