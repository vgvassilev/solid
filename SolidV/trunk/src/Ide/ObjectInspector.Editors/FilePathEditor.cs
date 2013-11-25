/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Linq;
using Gtk;

namespace SolidV.Gtk.InspectorGrid.InspectorEditors 
{
  using Gtk = global::Gtk;

  [InspectorEditorType(typeof(System.IO.Path))]
  public class FilePathEditor : InspectorEditorCell
  {
    public override bool DialogueEdit {
      get { return true; }
    }
    
    public override void LaunchDialogue()
    {
      var kindAtt = this.Inspector.Attributes.OfType<FilePathIsFolderAttribute>().FirstOrDefault();
      FileChooserAction action;
      string title;
      if(kindAtt == null) {
        action = FileChooserAction.Open;
//        title = GettextCatalog.GetString("Select File...");
        title = "Select File...";
      } else {
        action = FileChooserAction.SelectFolder;
//        title = GettextCatalog.GetString("Select Folder...");
        title = "Select Folder...";
      }
//      var fs = new MonoDevelop.Components.SelectFileDialog(title, action);
//      if(fs.Run())
//        Inspector.SetValue(Instance, fs.SelectedFile);
    }
  }
  
  public class FilePathIsFolderAttribute : Attribute
  {
  }
}