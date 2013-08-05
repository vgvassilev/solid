/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;

using Gtk;
using SolidOpt.Services;
using MonoDevelop.Components.Docking;

using DataMorphose.Model;
using DataMorphose.Plugins.ImportExport.Import;
using DataMorphose.Plugins.ImportExport.Export;

namespace DataMorphose.Plugins.ImportExport
{
  public class ImportExport : IPlugin
  {
    private IDataMorphose morphose = null;

    public ImportExport() {
    }

    #region IPlugin implementation
    void IPlugin.Init (object context)
    {
      morphose = context as IDataMorphose;
      
      Gtk.MenuBar mainMenuBar = morphose.GetMainMenu();

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
        
      // Setting up the Import and Export menu item in File menu       
      Gtk.MenuItem import = new Gtk.MenuItem("Import");
      import.Activated += OnImportActivated;
      (fileMenu.Submenu as Gtk.Menu).Prepend(import);

      
      Gtk.MenuItem export = new Gtk.MenuItem("Export");
      export.Activated += OnExportActivated;
      (fileMenu.Submenu as Gtk.Menu).Prepend(export);
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

    void OnImportActivated(object sender, EventArgs args)
    {
      var fc = new Gtk.FileChooserDialog("Choose a file to import", null, 
                                         Gtk.FileChooserAction.Open, "Cancel", 
                                         Gtk.ResponseType.Cancel, "Import", Gtk.ResponseType.Accept);
      try {
        fc.SetCurrentFolder("/media/LocalD/SolidProject/Tools/DataMorphose/trunk/plugins/ImportExport/test/DemoDB/Text/");
        // then create a filter for files. For example .csvdb:
        // filter is not necessary if you wish to see all files in the dialog
        Gtk.FileFilter filter = new Gtk.FileFilter();
        filter.Name = "CSV database";
        filter.AddPattern("*.csvdb");
        fc.AddFilter(filter);
        if (fc.Run() == (int)Gtk.ResponseType.Accept) {
          CSVImporter importer = new CSVImporter(/*firstRawIsHeader*/true);
          DataModel model = morphose.GetModel();
          Database db = importer.importDBFromFiles(fc.Filename);
          model.BeginUpdate();
          model.DB = db;
          model.EndUpdate();
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
      var fc = new Gtk.FileChooserDialog("Save", null, Gtk.FileChooserAction.Save, "Cancel", 
                                 Gtk.ResponseType.Cancel, "Export", Gtk.ResponseType.Accept);
      try {
        Directory.CreateDirectory("/media/LocalD/SolidProject/Tools/DataMorphose/trunk/plugins/ImportExport/test/DemoDB/Text/ExportedFiles");

        fc.SetCurrentFolder("/media/LocalD/SolidProject/Tools/DataMorphose/trunk/plugins/ImportExport/test/DemoDB/Text/ExportedFiles/");
        if (fc.Run() == (int)Gtk.ResponseType.Accept) {
          CSVExporter exporter = new CSVExporter();
          exporter.ExportDatabase(morphose.GetModel().DB, fc.Filename);
          morphose.GetModel();
        }
      } finally {
        fc.Destroy(); 
      }
    }

    #endregion
  }
}

