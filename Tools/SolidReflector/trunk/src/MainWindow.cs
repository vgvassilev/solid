///*
// * $Id$
// * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
// * For further details see the nearest License.txt
// */

using SolidOpt.Services;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;
using SolidOpt.Services.Transformations.Multimodel;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Mono.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

public partial class MainWindow: Gtk.Window
{
  private PluginServiceContainer plugins = new PluginServiceContainer();

  //FIXME: Put them in combobox model.
  private List<DecompilationStep> decompilationSteps = new List<DecompilationStep>();

	public MainWindow(): base(Gtk.WindowType.Toplevel)
	{
    // That's a hack because of the designer. If one needs to attach an event the designer attaches
    // it in the end of the file after the call to Initialize. Works for the most of the events
    // but not for events like Realize which happen in the initialization. This function is used
    // to attach the event handlers before the initialization part.
    PreBuild();
		Build();
	}

	protected void OnDeleteEvent(object sender, Gtk.DeleteEventArgs a)
	{
    SaveEnvironment();
		Gtk.Application.Quit();
		a.RetVal = true;
	}

	protected void OnOpenActionActivated(object sender, System.EventArgs e)
  {
    var fc = new Gtk.FileChooserDialog("Choose the file to open",
                                        this, Gtk.FileChooserAction.Open,
                                        "Cancel", Gtk.ResponseType.Cancel,
                                        "Open", Gtk.ResponseType.Accept);
    try {
      fc.SelectMultiple = true;
      fc.SetCurrentFolder(Environment.CurrentDirectory);
      if (fc.Run() == (int)Gtk.ResponseType.Accept) {
        // Get the currently loaded files in the tree view
        List<string> filesLoaded;
        GetLoadedFiles(out filesLoaded);

        List<string> filesToLoad = new List<string>();
        filesToLoad.AddRange(fc.Filenames);
        for (uint i = 0; i < fc.Filenames.Length; i++)
          if (filesLoaded.Contains(fc.Filenames[i])) {
            filesToLoad.Remove(fc.Filenames[i]);
            ShowMessageGtk(String.Format("File {0} already loaded.", fc.Filenames[i]));
          }
        LoadFilesInTreeView(filesToLoad.ToArray());
      }
    } finally {
      fc.Destroy();
    }
  }

  /// <summary>
  /// Walks up the tree view's model and collects the loaded files.
  /// </summary>
  /// <param name='filesLoaded'>[out]
  /// Storage where the loaded file names will be added.
  /// </param>
  private void GetLoadedFiles(out List<string> filesLoaded) {
    filesLoaded = new List<string>();
    if (assemblyView.Columns.Length == 0)
      return;

    Gtk.TreeIter iter;
    assemblyView.Model.GetIterFirst(out iter);
    AssemblyDefinition aDef = null;
    do {
      aDef = assemblyView.Model.GetValue(iter, 0) as AssemblyDefinition;
      Debug.Assert(aDef != null && aDef.MainModule != null,
                   "We must have only assembly definitions here");
      filesLoaded.Add(aDef.MainModule.FullyQualifiedName);
    }
    while (assemblyView.Model.IterNext(ref iter));
  }

  /// <summary>
  /// Shows message box using GTK.
  /// </summary>
  /// <param name='msg'>
  /// The message.
  /// </param>
  private void ShowMessageGtk(string msg) {
    var msgBox = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Info,
                                       Gtk.ButtonsType.Ok, msg);
    msgBox.Run();
    msgBox.Destroy();
  }

	protected void OnExitActionActivated(object sender, System.EventArgs e)
	{
    SaveEnvironment();
		Gtk.Application.Quit();
	}

  protected void OnRealized(object sender, System.EventArgs e)
  {
    LoadEnvironment();
    LoadRegisteredPlugins();
  }

  private void LoadEnvironment() {
    string curEnv = System.IO.Path.Combine(Environment.CurrentDirectory, "Current.env");
    if (File.Exists(curEnv)) {
      LoadFilesInTreeView(File.ReadAllLines(curEnv));
    }
  }

  private void SaveEnvironment() {
    List<string> filesLoaded;
    GetLoadedFiles(out filesLoaded);

    saveEnvironmentData(System.IO.Path.Combine(Environment.CurrentDirectory, "Current.env"),
                        filesLoaded);
    saveEnvironmentData(System.IO.Path.Combine(Environment.CurrentDirectory, "Plugin.env"),
                        plugins.plugins.ConvertAll<string>(x => x.ToString()));
  }

  private void saveEnvironmentData(string curEnv, List<string> items) {
    FileStream file = new FileStream(curEnv, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    using (StreamWriter writer = new StreamWriter(file)) {
      writer.Write("");
      writer.Flush();
      foreach (string line in items)
        writer.WriteLine(line);
      writer.Flush();
    }
  }

  private void LoadFilesInTreeView(string [] files) {
    Gtk.TreeViewColumn col = new Gtk.TreeViewColumn();

    Gtk.CellRendererText colAssemblyCell = new Gtk.CellRendererText();
    col.PackStart(colAssemblyCell, true);

    col.AddAttribute(colAssemblyCell, "text", 0);

    if (assemblyView.GetColumn(0) != null)
      assemblyView.Columns[0] = col;
    else
      assemblyView.AppendColumn(col);

    Gtk.TreeStore store = assemblyView.Model as Gtk.TreeStore;
    if (store == null)
     store = new Gtk.TreeStore(typeof(object));

    foreach (string file in files) {
      if (File.Exists(file))
        store.AppendValues(AssemblyDefinition.ReadAssembly(file));
      else
        ShowMessageGtk(String.Format("File {0} doesn't exits.", file));
    }

    // Add functions managinig the visualization of those assembly definitions
    col.SetCellDataFunc(colAssemblyCell, new Gtk.TreeCellDataFunc(RenderAssemblyDefinition));

    assemblyView.Model = store;
    assemblyView.ShowAll();
  }

  private void RenderAssemblyDefinition(Gtk.TreeViewColumn column, Gtk.CellRenderer cell,
                                          Gtk.TreeModel model, Gtk.TreeIter iter) {
    object curObject = model.GetValue(iter, 0);
    switch (model.GetPath(iter).Depth) {
      // Assemblies
      case 1:
        AssemblyDefinition aDef = curObject as AssemblyDefinition;
        Debug.Assert(aDef != null, "Must have assembly.");
        (cell as Gtk.CellRendererText).Text =  aDef.Name.Name;
        break;
      // Modules
      case 2:
        ModuleDefinition modDef = curObject as ModuleDefinition;
        Debug.Assert(modDef != null, "Must have module.");
        (cell as Gtk.CellRendererText).Text =  modDef.Name;
        break;
      // Types
      case 3:
        TypeDefinition tDef = curObject as TypeDefinition;
        Debug.Assert(tDef != null, "Must have type (definition).");
        (cell as Gtk.CellRendererText).Text =  tDef.Name;
        break;
      // Methods
      case 4:
        IMemberDefinition memberDef = curObject as IMemberDefinition;
        Debug.Assert(memberDef != null, "Must have member.");
        (cell as Gtk.CellRendererText).Text =  memberDef.Name;
        break;
    }

  }

  private void LoadRegisteredPlugins() {
    string registeredPlugins =
        System.IO.Path.Combine(Environment.CurrentDirectory, "Plugins.env");
    if (File.Exists(registeredPlugins)) {
      foreach (string s in File.ReadAllLines(registeredPlugins))
        if (File.Exists(s))
          plugins.AddPlugin(s);
    }

    plugins.LoadPlugins();
    foreach (DecompilationStep ds in plugins.GetServices<DecompilationStep>()) {
      // FIXME: Should be get plugin short description
      combobox6.AppendText(ds.GetTargetType().FullName);
      decompilationSteps.Add(ds);
    }
  }

  protected virtual void PreBuild() {
    this.Realized += new global::System.EventHandler (this.OnRealized);
  }

  protected void OnAssemblyViewRowActivated (object o, Gtk.RowActivatedArgs args)
  {
    Gtk.TreeIter iter;
    assemblyView.Model.GetIter(out iter, args.Path);
    object currentObj = (object) assemblyView.Model.GetValue(iter, 0);

    switch(args.Path.Depth) {
    case 1:
        AssemblyDefinition curAssembly = currentObj as AssemblyDefinition;
        Debug.Assert(curAssembly != null, "Assembly cannot be null.");
        AttachSubTree(assemblyView.Model, iter, curAssembly.Modules.ToArray());
        break;
    case 2:
        ModuleDefinition curModule = currentObj as ModuleDefinition;
        Debug.Assert(curModule != null, "CurModule is null!?");
        AttachSubTree(assemblyView.Model, iter, curModule.Types.ToArray());
        break;

    case 3:
        TypeDefinition curType = currentObj as TypeDefinition;
        Debug.Assert(curType != null, "CurType is null!?");
        List<IMemberDefinition> members = new List<IMemberDefinition>();
        members.AddRange(curType.Fields.ToArray());
        members.AddRange(curType.Methods.ToArray());
        members.AddRange(curType.Events.ToArray());
        AttachSubTree(assemblyView.Model, iter, members.ToArray());
        break;
      case 4:
        IMemberDefinition memberDef = currentObj as IMemberDefinition;
        Debug.Assert(memberDef != null, "MemberDef is null!?");
        DumpMember(memberDef);
        break;
    }
    assemblyView.ShowAll();
    ShowAll();
  }

  /// <summary>
  /// Attaches a submodel to the tree view's model.
  /// </summary>
  /// <param name='model'>
  /// The root model to be attached to.
  /// </param>
  /// <param name='parent'>[ref]
  /// The pointer where the elements to be attached to.
  /// </param>
  /// <param name='elements'>
  /// Elements.
  /// </param>
  protected void AttachSubTree(Gtk.TreeModel model, Gtk.TreeIter parent, object[] elements)
  {
    Gtk.TreeStore store = model as Gtk.TreeStore;
    Debug.Assert(store != null, "TreeModel shouldn't be flat");

    // remove the values if they were added before.
    Gtk.TreePath path = store.GetPath(parent);
    path.Down();
    Gtk.TreeIter iter;
    while (store.GetIter(out iter, path))
      store.Remove(ref iter);

    // Add the elements to the tree view.
    for (uint i = 0; i < elements.Length; ++i) {
      store.AppendValues(parent, elements[i]);
    }
  }

  protected void DumpMember(IMemberDefinition memberDef) {
    MethodDefinition methodDef = memberDef as MethodDefinition;
    if (methodDef != null) {
      DumpMethodDef(methodDef);
      return;
    }

    EventDefinition eventDef = memberDef as EventDefinition;
    if (eventDef != null) {
      DumpEventDef(eventDef);
      return;
    }

    FieldDefinition fieldDef = memberDef as FieldDefinition;
    if (fieldDef != null) {
      DumpFieldDef(fieldDef);
      return;
    }
  }

  protected void DumpMethodDef(MethodDefinition methodDef)
  {
    disassemblyText.Buffer.Clear();

    SolidReflector.ILWriter writer = new SolidReflector.ILWriter(disassemblyText.Buffer);
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
      writer.WriteType(methodDef.Parameters[0].ParameterType.ToString());
      writer.WriteName(methodDef.Parameters[0].Name.ToString());
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

        if (handler.FilterEnd == inst) {
          writer.Outdent();
          writer.WriteExceptionDirective("}");
        }

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

  protected void DumpControlFlowGraph(ControlFlowGraph cfg) {
    disassemblyText.Buffer.Clear();

    Gtk.TextIter textIter = disassemblyText.Buffer.EndIter;

    if (cfg == null)
      disassemblyText.Buffer.Insert(ref textIter, "Cannot dump CFG\n");
    else
      disassemblyText.Buffer.Insert(ref textIter, cfg.ToString() + "\n");
  }


  protected void DumpEventDef(EventDefinition evtDef) {
    disassemblyText.Buffer.Clear();

    Gtk.TextIter textIter = disassemblyText.Buffer.EndIter;

    foreach (MethodDefinition mDef in evtDef.OtherMethods) {
      disassemblyText.Buffer.Insert(ref textIter, evtDef.ToString() + "\n");
    }
  }

  protected void DumpFieldDef(FieldDefinition fldDef) {
    disassemblyText.Buffer.Clear();
    Gtk.TextIter textIter = disassemblyText.Buffer.EndIter;
    disassemblyText.Buffer.Insert(ref textIter, fldDef.ToString() + "\n");
  }

  private void AppendLabel (System.Text.StringBuilder builder, Instruction instruction) {
    builder.Append ("IL_");
    builder.Append (instruction.Offset.ToString ("x4"));
  }

  protected void OnCombobox6Changed (object sender, System.EventArgs e)
  {
    Gtk.TreeIter iter;
    combobox6.GetActiveIter(out iter);
    string val = (string)combobox6.Model.GetValue(iter, 0);
    if (val == "IL") {

    }
    // FIXME: Plugin short desc
    else if (val == typeof(ControlFlowGraph).FullName) {
      Gtk.TreeIter selIter;
      if (assemblyView.Selection.GetSelected(out selIter)) {
        MethodDefinition mDef = assemblyView.Model.GetValue(selIter, 0) as MethodDefinition;
        foreach (DecompilationStep ds in decompilationSteps)
          if (ds.GetSourceType() == mDef.GetType() && ds.GetTargetType() == typeof(ControlFlowGraph)) {
            object cfg = ds.Process(mDef.Body);
            DumpControlFlowGraph(cfg as ControlFlowGraph);
        }
      }
    }
  }
}