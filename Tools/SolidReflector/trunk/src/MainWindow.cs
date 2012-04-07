/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Diagnostics;
using Mono.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

public partial class MainWindow: Gtk.Window
{
  private string[] fileNames = {};
  private AssemblyDefinition curAssembly = null;
  private ModuleDefinition curModule = null;
  private TypeDefinition curType = null;

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
        fileNames = fc.Filenames;
      }
    } finally {
      fc.Destroy();
    }

    LoadFilesInTreeView();
  }

	protected void OnExitActionActivated(object sender, System.EventArgs e)
	{
    SaveEnvironment();
		Gtk.Application.Quit();
	}

  protected void OnRealized(object sender, System.EventArgs e)
  {
    string curEnv = System.IO.Path.Combine(Environment.CurrentDirectory, "Current.env");
    if (System.IO.File.Exists(curEnv)) {
      fileNames = System.IO.File.ReadAllLines(curEnv);
      LoadFilesInTreeView();
    }
  }

  private void SaveEnvironment() {
    string curEnv = System.IO.Path.Combine(Environment.CurrentDirectory, "Current.env");
    System.IO.File.WriteAllLines(curEnv, fileNames);
  }

  private void LoadFilesInTreeView() {

    Gtk.TreeViewColumn col = new Gtk.TreeViewColumn();

    Gtk.CellRendererText colTitleCell = new Gtk.CellRendererText();
    col.PackStart(colTitleCell, true);

    col.AddAttribute (colTitleCell, "text", 0);

    assemblyView.AppendColumn(col);

    Gtk.TreeStore store = new Gtk.TreeStore(typeof(string));
    foreach (string file in fileNames) {
      store.AppendValues(System.IO.Path.GetFileName(file));
    }

    assemblyView.Model = store;
    assemblyView.ShowAll();
  }

  protected virtual void PreBuild() {
    this.Realized += new global::System.EventHandler (this.OnRealized);
  }

  protected void OnAssemblyViewRowActivated (object o, Gtk.RowActivatedArgs args)
  {
    Gtk.TreeIter iter;
    assemblyView.Model.GetIter(out iter, args.Path);
    string s = (string) assemblyView.Model.GetValue(iter, 0);

    switch(args.Path.Depth) {
    case 1:
        foreach (string f in fileNames) {
          if (System.IO.Path.GetFileName(f) == s) {
            curAssembly = AssemblyDefinition.ReadAssembly(f);
            Debug.Assert(curAssembly != null, "Assembly cannot be null.");
            AttachSubTree(assemblyView.Model, iter, curAssembly.Modules.ToArray());
          }
        }
        break;
    case 2:
        foreach (ModuleDefinition mDef in curAssembly.Modules) {
          if (mDef.Name == s) {
            curModule = mDef;
            AttachSubTree(assemblyView.Model, iter, mDef.Types.ToArray());
          }
        }
        break;

    case 3:
        Debug.Assert(curModule != null, "CurModule is null!?");
        foreach (TypeDefinition tDef in curModule.Types) {
          if (tDef.Name == s) {
            curType = tDef;
            //AttachSubTree(assemblyView.Model, iter, tDef.Fields.ToArray());
            AttachSubTree(assemblyView.Model, iter, tDef.Methods.ToArray());
            //AttachSubTree(assemblyView.Model, iter, tDef.Events.ToArray());
          }
        }
        break;
      case 4:
        Debug.Assert(curType != null, "CurType is null!?");
        foreach (MethodDefinition mDef in curType.Methods)
          if (mDef.ToString() == s)
            DumpMember(mDef);
        break;
    }
    assemblyView.ShowAll();
    ShowAll();
  }

  protected void AttachSubTree(Gtk.TreeModel model, Gtk.TreeIter parent, object[] elements)
  {
    Gtk.TreeStore store = model as Gtk.TreeStore;
    Debug.Assert(store != null, "TreeModel shouldn't be flat");
    for (uint i = 0; i < elements.Length; ++i) {
      store.AppendValues(parent, elements[i].ToString());
    }
  }

  protected void DumpMember(IMemberDefinition member)
  {
    disassemblyText.Buffer.Clear();

    Gtk.TextIter textIter = disassemblyText.Buffer.EndIter;
    MethodDefinition method = member as MethodDefinition;
    if (method != null) {
      foreach (Instruction inst in method.Body.Instructions) {
         disassemblyText.Buffer.Insert(ref textIter, inst.ToString() + "\n");
      }
      return;
    }

    EventDefinition evt = member as EventDefinition;
    if (evt != null) {
      foreach (MethodDefinition mDef in evt.OtherMethods) {
         disassemblyText.Buffer.Insert(ref textIter, mDef.ToString() + "\n");
      }
      return;
    }

    FieldDefinition field = member as FieldDefinition;
    if (field != null) {
      disassemblyText.Buffer.Insert(ref textIter, field.ToString() + "\n");
      return;
    }

  }

  protected void OnCombobox6Changed (object sender, System.EventArgs e)
  {
    Gtk.TreeIter iter;
    combobox6.GetActiveIter(out iter);
    string val = (string)combobox6.Model.GetValue(iter, 0);
    if (val == "IL") {

    }
    else if (val == "CFG") {

    }

  }
}
