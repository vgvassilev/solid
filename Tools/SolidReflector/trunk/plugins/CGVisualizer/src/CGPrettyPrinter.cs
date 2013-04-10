using Mono.Cecil;
using System;

using SolidOpt.Services.Transformations.CodeModel.CallGraph;
using SolidOpt.Services.Transformations.Multimodel.ILtoCG;

namespace SolidReflector.Plugins.CGVisualizer
{
  public static class CGPrettyPrinter
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

      var builder = new CallGraphBuilder(methodDefinition);
      CallGraph callGraph = builder.Create(5);

      textView.Buffer.Text = callGraph.ToString();
    }

    private static void PrintEvent(EventDefinition eventDefinition, Gtk.TextView textView) { }

    private static void PrintField(FieldDefinition fieldDefinition, Gtk.TextView textView) { }
  }
}