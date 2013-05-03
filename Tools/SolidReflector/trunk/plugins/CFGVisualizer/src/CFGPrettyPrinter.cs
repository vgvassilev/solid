using Mono.Cecil;
using System;

using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;
using SolidOpt.Services.Transformations.Multimodel.ILtoCFG;

namespace SolidReflector.Plugins.CFGVisualizer
{
  public static class CFGPrettyPrinter
  {
    public static void PrintPretty(MemberReference memberRef, Gtk.TextView textView) {
      if (memberRef is TypeDefinition)
        PrintType(memberRef as TypeDefinition, textView);
      else if (memberRef is MethodDefinition)
        PrintMethod(memberRef as MethodDefinition, textView);
      else if (memberRef is EventDefinition)
        PrintEvent(memberRef as EventDefinition, textView);
      else if (memberRef is FieldDefinition)
        PrintField(memberRef as FieldDefinition, textView);
    }

    private static void PrintType(TypeDefinition typeDef, Gtk.TextView textView) { }

    private static void PrintMethod(MethodDefinition methodDef, Gtk.TextView textView) {
      textView.Buffer.Clear();

      var builder = new ControlFlowGraphBuilder(methodDef);
      ControlFlowGraph cfg = builder.Create();

      textView.Buffer.Text = cfg.ToString();
    }

    private static void PrintEvent(EventDefinition evtDef, Gtk.TextView textView) {
      textView.Buffer.Clear();
      Gtk.TextIter textIter = textView.Buffer.EndIter;

      foreach (MethodDefinition mDef in evtDef.OtherMethods) {
        textView.Buffer.Insert(ref textIter, evtDef.ToString() + "\n");
      }
    }

    private static void PrintField(FieldDefinition fldDef, Gtk.TextView textView) {
      textView.Buffer.Clear();
      Gtk.TextIter textIter = textView.Buffer.EndIter;
      textView.Buffer.Insert(ref textIter, fldDef.ToString() + "\n");
    }
  }
}