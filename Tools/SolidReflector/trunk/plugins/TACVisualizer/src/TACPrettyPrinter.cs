using System;
using Mono.Cecil;

using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;
using SolidOpt.Services.Transformations.Multimodel.ILtoCFG;

using SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode;
using SolidOpt.Services.Transformations.Multimodel.CFGtoTAC;

namespace SolidReflector.Plugins.TACVisualizer
{
  public static class TACPrettyPrinter
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

    private static void PrintType(TypeDefinition typeDefinition, Gtk.TextView textView) { }

    private static void PrintMethod(MethodDefinition methodDefinition, Gtk.TextView textView) {
      textView.Buffer.Clear();

      var cfgBuilder = new ControlFlowGraphBuilder(methodDefinition);
      ControlFlowGraph cfg = cfgBuilder.Create();

      var tacBuilder = new ThreeAddressCodeBuilder(cfg);
      ThreeAdressCode tac = tacBuilder.Create();

      textView.Buffer.Text = tac.ToString();
    }

    private static void PrintEvent(EventDefinition eventDefinition, Gtk.TextView textView) { }

    private static void PrintField(FieldDefinition fieldDefinition, Gtk.TextView textView) { }
  }
}