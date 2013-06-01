// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

using SolidOpt.Services;
using MonoDevelop.Components.Docking;

namespace DataMorphose.Plugins.ImportExport
{
  public class ImportExport : IPlugin
  {
    public ImportExport() {
    }

    #region IPlugin implementation
    void IPlugin.Init (object context)
    {
      DockItem dockItem = null;
      IDataMorphose reflector = context as IDataMorphose;
      var MainWindow = reflector.GetMainWindow();
      
      Gtk.MenuBar mainMenuBar = reflector.GetMainMenu();

      Gtk.MenuItem fileMenu = null;
      // Find the File menu if present
      foreach(Gtk.Widget w in mainMenuBar.Children)
        if (w.Name == "FileAction") {
          fileMenu = w as Gtk.MenuItem;
          break;
        }
      // If not present - create it
      if (fileMenu == null) {
        Gtk.Menu menu = new Gtk.Menu();
        fileMenu = new Gtk.MenuItem("File");
        fileMenu.Submenu = menu;
        mainMenuBar.Append(fileMenu);
      }
        
      // Setting up the Importand Export menu item in File          
      Gtk.MenuItem import = new Gtk.MenuItem("Import");
      import.Activated += OnImportActivated;
      (fileMenu.Submenu as Gtk.Menu).Prepend(import);
      
      Gtk.MenuItem export = new Gtk.MenuItem("Export");
      export.Activated += OnExportActivated;
      (fileMenu.Submenu as Gtk.Menu).Prepend(export);
      
      // Attaching the current dockItem in the DockFrame
      dockItem = MainWindow.DockFrame.AddItem("AssemblyBrowser");
      dockItem.DrawFrame = true;
      dockItem.Label = "Assembly";

     
    }
    

    void IPlugin.UnInit (object context)
    {
      throw new NotImplementedException ();
    }
    
    
    /// <summary>
    /// Raises the activated event when the Import menu item is invoked.
    /// </summary>
    /// <param name='sender'>
    /// The Gtk.MenuItem.
    /// </param>
    /// <param name='args'>
    /// Arguments.
    /// </param>
    /// 
    void OnImportActivated(object sender, EventArgs args)
    {
      var fc = new Gtk.FileChooserDialog("Choose a file to import", null, 
                                         Gtk.FileChooserAction.Open, "Cancel", 
                                         Gtk.ResponseType.Cancel, "Import", Gtk.ResponseType.Accept);
      try {
        fc.SelectMultiple = true;
        fc.SetCurrentFolder(Environment.CurrentDirectory);
        if (fc.Run() == (int)Gtk.ResponseType.Accept) {
          // Get the currently loaded files in the tree view
          List<string> filesLoaded;
          //GetLoadedFiles(out filesLoaded);

          List<string> filesToLoad = new List<string>();
          filesToLoad.AddRange(fc.Filenames);
          for (uint i = 0; i < fc.Filenames.Length; i++)
            if (filesLoaded.Contains(fc.Filenames[i])) {
              filesToLoad.Remove(fc.Filenames[i]);
              ShowMessageGtk(String.Format("File {0} already loaded.", fc.Filenames[i]));
            }
          //LoadFilesInTreeView(filesToLoad.ToArray());
        }
      } finally {
        fc.Destroy();
      }
    }
    
    /// <summary>
    /// Raises the activated event when the Export menu item is invoked.
    /// </summary>
    /// <param name='sender'>
    /// The Gtk.MenuItem.
    /// </param>
    /// <param name='args'>
    /// Arguments.
    /// </param>
    /// 
    void OnExportActivated(object sender, EventArgs args)
    {
      var fc = new Gtk.FileChooserDialog("Save", null, 
                                         Gtk.FileChooserAction.Open, "Cancel", 
                                         Gtk.ResponseType.Cancel, "Export", Gtk.ResponseType.Accept);
      try {
        fc.SelectMultiple = true;
        fc.SetCurrentFolder(Environment.CurrentDirectory);
        if (fc.Run() == (int)Gtk.ResponseType.Accept) {
          // Get the currently loaded files in the tree view
          List<string> filesLoaded;
          //GetLoadedFiles(out filesLoaded);

          List<string> filesToLoad = new List<string>();
          filesToLoad.AddRange(fc.Filenames);
          for (uint i = 0; i < fc.Filenames.Length; i++)
            if (filesLoaded.Contains(fc.Filenames[i])) {
              filesToLoad.Remove(fc.Filenames[i]);
              ShowMessageGtk(String.Format("File {0} already loaded.", fc.Filenames[i]));
            }
          //LoadFilesInTreeView(filesToLoad.ToArray());
        }
      } finally {
        fc.Destroy();
      }
    }
    
    
    /// <summary>
    /// Shows message box using GTK.
    /// </summary>
    /// <param name='msg'>
    /// The message.
    /// </param>
    /// 
    private void ShowMessageGtk(string msg) {
      var msgBox = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Info,
                                         Gtk.ButtonsType.Ok, msg);
      msgBox.Run();
      msgBox.Destroy();
    }
    
    #endregion


  }
}

