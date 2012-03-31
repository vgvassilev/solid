/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using Gtk;
using Mono.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

public partial class MainWindow: Gtk.Window
{
  private string[] fileNames = {};
	public MainWindow(): base(Gtk.WindowType.Toplevel)
	{
    // That's a hack because of the designer. If one needs to attach an event the designer attaches
    // it in the end of the file after the call to Initialize. Works for the most of the events
    // but not for events like Realize which happen in the initialization. This function is used
    // to attach the event handlers before the initialization part.
    PreBuild();
		Build();
	}
	
	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
    SaveEnvironment();
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnOpenActionActivated(object sender, System.EventArgs e)
  {
    var fc = new FileChooserDialog("Choose the file to open",
      this,
      FileChooserAction.Open,
      "Cancel", ResponseType.Cancel,
      "Open", ResponseType.Accept);
    try {
      fc.SelectMultiple = true;
      fc.SetCurrentFolder(Environment.CurrentDirectory);
      if (fc.Run() == (int)ResponseType.Accept) {
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
		Application.Quit();
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

    TreeViewColumn col = new TreeViewColumn();

    CellRendererText colTitleCell = new CellRendererText ();
    col.PackStart(colTitleCell, true);

    col.AddAttribute (colTitleCell, "text", 0);

    assemblyView.AppendColumn(col);

    ListStore ls = new ListStore(typeof(string));
    foreach (string file in fileNames) {
      ls.AppendValues(System.IO.Path.GetFileName(file));
    }

    assemblyView.Model = ls;
    assemblyView.ShowAll();
  }

  protected virtual void PreBuild() {
    this.Realized += new global::System.EventHandler (this.OnRealized);
  }

  protected void OnAssemblyViewRowActivated (object o, Gtk.RowActivatedArgs args)
  {
    TreeIter iter;
    assemblyView.Model.GetIter(out iter, args.Path);
    string s = (string)assemblyView.Model.GetValue(iter, 0);
    foreach (string f in fileNames) {
      if (System.IO.Path.GetFileName(f) == s) {
        AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(f);
        TypeDefinition type = assembly.MainModule.GetType("SolidReflector.MainClass");
        MethodDefinition found = GetMethod(type.Methods, "Main");
        TextIter textIter = disassemblyText.Buffer.EndIter;
        foreach (Instruction i in found.Body.Instructions) {
          textIter = disassemblyText.Buffer.EndIter;
          disassemblyText.Buffer.Insert(ref textIter, i.ToString() + "\n");
        }
        break;
      }
    }
    //assemblyView.
  }

  private MethodDefinition GetMethod(Collection<MethodDefinition> methods, string name)
   {
     foreach (MethodDefinition mDef in methods) {
       if (mDef.Name == name)
         return mDef;
     }
     return null;
   }

}
