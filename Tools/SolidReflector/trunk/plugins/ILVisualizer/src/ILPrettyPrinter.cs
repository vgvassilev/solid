using System;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SolidReflector.Plugins.ILVisualizer
{
  public static class ILPrettyPrinter
  {
    public static void PrintAssembly(AssemblyDefinition assemblyDefinition, Gtk.TextView textView) {
      textView.Buffer.Clear();
      ILFormatter writer = new ILFormatter(textView.Buffer);
      foreach (CustomAttribute attr in assemblyDefinition.CustomAttributes) {
        writer.Write(attr.AttributeType.Name);
        writer.Write("(\"");
        if (attr.ConstructorArguments.Count > 0)
          writer.Write(attr.ConstructorArguments[0].Value.ToString());
        else
          writer.Write("");
        writer.Write("\")");
        writer.NewLine();
      }
    }

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

      ILFormatter writer = new ILFormatter(textView.Buffer);
      writer.Indent();
      writer.WriteMethodAttribute(".method");

      if (methodDef.IsPublic)
        writer.WriteMethodAttribute("public");
      if (methodDef.IsPrivate)
        writer.WriteMethodAttribute("private");
      if (methodDef.IsHideBySig)
        writer.WriteMethodAttribute("hidebysig");
      if (methodDef.IsStatic)
        writer.WriteMethodAttribute("static");
      else
        writer.WriteMethodAttribute("instance");

      writer.WriteType(methodDef.ReturnType.Name);
      writer.WriteName(methodDef.Name);
      writer.Write(" (");
      if (methodDef.Parameters.Count > 0) {
        for (int i = 0; i < methodDef.Parameters.Count; i++) {
          writer.WriteType(methodDef.Parameters[i].ParameterType.ToString());
          writer.WriteName(methodDef.Parameters[i].Name.ToString());
          writer.Write(" ");
        }
      }
      writer.Write(") ");
      if (methodDef.IsIL)
        writer.WriteImplementationAttribute("cil");
      else if (methodDef.IsNative)
        writer.WriteImplementationAttribute("native");

      if (methodDef.IsManaged)
        writer.WriteImplementationAttribute("managed");
      else if (methodDef.IsUnmanaged)
        writer.WriteImplementationAttribute("unmanaged");

      writer.NewLine();
      writer.Write("{");
      writer.NewLine();

      if (methodDef.Body.Variables.Count > 0) {
        writer.WriteKeyword(".locals init");
        writer.Write("(");
        for (int i = 0; i < methodDef.Body.Variables.Count; i++) {
          writer.WriteType(methodDef.Body.Variables[i].VariableType.Name.ToString());
          writer.WriteName(methodDef.Body.Variables[i].ToString());
          if (i + 1 != methodDef.Body.Variables.Count)
            writer.Write(", ");
        }
        writer.Write(")");
        writer.NewLine();
      }

      foreach (Instruction inst in methodDef.Body.Instructions) {
        //if (method.Body.HasExceptionHandlers) {
        foreach (ExceptionHandler handler in methodDef.Body.ExceptionHandlers) {
          if (handler.FilterStart == inst) {
            writer.Indent();
            writer.WriteExceptionDirective(".filter {");
          }

          //if (handler.FilterEnd == inst) {
          //  writer.Outdent();
          //  writer.WriteExceptionDirective("}");
          //}

          if (handler.TryStart == inst) {
            writer.WriteExceptionDirective(".try {");
            writer.Indent();
          }

          if (handler.TryEnd == inst) {
            writer.Outdent();
            writer.WriteExceptionDirective("}");
          }

          if (handler.HandlerStart == inst) {
            writer.WriteExceptionDirective(handler.HandlerType.ToString() + " {");
            writer.Indent();
          }

          if (handler.HandlerEnd == inst) {
            writer.Outdent();
            writer.WriteExceptionDirective("}");
          }
        }
        writer.WriteInstruction(inst);
      }
      writer.Outdent();
      writer.Write("}");
    }

    private static void PrintEvent(EventDefinition evtDef, Gtk.TextView textView) {
      textView.Buffer.Clear();

      Gtk.TextIter textIter = textView.Buffer.EndIter;

      foreach (MethodDefinition mDef in evtDef.OtherMethods)
        textView.Buffer.Insert(ref textIter, evtDef.ToString() + "\n");
    }

    private static void PrintField(FieldDefinition fldDef, Gtk.TextView textView) {
      textView.Buffer.Clear();
      Gtk.TextIter textIter = textView.Buffer.EndIter;
      textView.Buffer.Insert(ref textIter, fldDef.ToString() + "\n");
    }
  }
}