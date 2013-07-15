// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
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
    private DockItem importPanel = null;

    private Gtk.Notebook nb = new Gtk.Notebook();
    private Gtk.VBox box = new Gtk.VBox();

    private Gtk.TreeView treeView = new TreeView();
    private Gtk.ComboBox comboBox = new Gtk.ComboBox();

    private string selectedTable = "";
    private TreeIter iter;

    public ImportExport() {
    }

    #region IPlugin implementation
    void IPlugin.Init (object context)
    {

      morphose = context as IDataMorphose;
      var MainWindow = morphose.GetMainWindow();
      
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
        
      // Setting up the Import and Export menu item in File          
      Gtk.MenuItem import = new Gtk.MenuItem("Import");
      import.Activated += OnImportActivated;
      (fileMenu.Submenu as Gtk.Menu).Prepend(import);

      
      Gtk.MenuItem export = new Gtk.MenuItem("Export");
      export.Activated += OnExportActivated;
      (fileMenu.Submenu as Gtk.Menu).Prepend(export);

      // Attach the comboBox first
      box.PackStart(comboBox, false, false, 0);

      ScrolledWindow scroll =  new ScrolledWindow();
      // Attach the treeView to the scroll bar.
      scroll.AddWithViewport(treeView);
      // Attach the scrollBar 
      box.PackStart(scroll, true, true, 0);

      // Add the box to the main window
      nb.AppendPage(box, new Gtk.Label("DataBrowser"));

      nb.ShowAll();
      // Attaching the current dockItem in the DockFrame
      importPanel = MainWindow.DockFrame.AddItem("DataBrowser");
      importPanel.DrawFrame = true;
      importPanel.Label = "Grid";
      importPanel.Expand = true;
      importPanel.DrawFrame = true;
      importPanel.DefaultVisible = true;
      importPanel.Visible = true;
      importPanel.Content = nb;
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
        fc.SetCurrentFolder("/media/LocalD/SolidProject/Tools/DataMorphose/trunk/plugins/ImportExport/test/DemoDB/Text/");
        // then create a filter for files. For example .csvdb:
        // filter is not necessary if you wish to see all files in the dialog
        Gtk.FileFilter filter = new Gtk.FileFilter();
        filter.Name = "CSV database";
        filter.AddPattern("*.csvdb");
        fc.AddFilter(filter);
        if (fc.Run() == (int)Gtk.ResponseType.Accept) {
          CSVImporter importer = new CSVImporter(/*firstRawIsHeader*/true);
          Model.Database db = importer.importDBFromFiles(fc.Filename);
          morphose.SetModel(db);
          populateComboBox(db);
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
          exporter.ExportDatabase(morphose.GetModel(), fc.Filename);
          morphose.GetModel();
        }
      } finally {
        fc.Destroy(); 
      }
    }

    #endregion

    private void populateGrid(Model.Table table) {
      Gtk.ListStore store = treeView.Model as Gtk.ListStore;

      // Empty the store on every new table-selection
      if (store != null) {
        foreach(Gtk.TreeViewColumn col in treeView.Columns)
          treeView.RemoveColumn(col);
        store.Clear();
      }

      // Construct the array of types which is the size of the columns.
      Type[] types = new Type[table.Columns.Count];
      for(int i = 0, e = table.Columns.Count; i < e; ++i)
        types[i] = typeof(string);

      // The list store must take array of types which it will hold.
      store = new Gtk.ListStore(types);
      treeView.Model = store;

      // Put columns and cell renderers in the ListStore. There we will put the actual values.
      string t = (string) comboBox.Model.GetValue(iter, 0);

      for (int i = 0, e = table.Columns.Count; i < e; ++i) {
        Model.Column dcol = table.Columns[i];
        Gtk.TreeViewColumn col = new Gtk.TreeViewColumn();
        Gtk.CellRendererText cell = new Gtk.CellRendererText();
        col.PackStart(cell, true);
        col.AddAttribute(cell, "text", i);
        col.Title = dcol.Meta.Name;
        treeView.AppendColumn(col);
      }

      // Now fill in the rows.
      foreach(Model.Row r in table.GetAsRows()) {
        store.AppendValues(r.GetAsStringArray());
      }
      treeView.ShowAll();
    }


    private void populateComboBox(Database db) {
      // Set up the comboBox
      CellRendererText combocell = new CellRendererText();
      comboBox.PackStart(combocell, false);
      comboBox.AddAttribute(combocell, "text", 0);
      ListStore combostore = new ListStore(typeof(string));
      comboBox.Model = combostore;

      // Take the names of the tables
      for (int i = 0; i < db.Tables.Count; ++i) {
        Model.Table table = db.Tables[i];
        combostore.AppendValues(table.Name);
      }
      comboBox.Changed += new EventHandler(OnComboBoxChanged);

      comboBox.ShowAll();
    }

    // Get the name of the selectedTable and fill the grid with the representing data
    private void OnComboBoxChanged(object o, EventArgs args) {
      ComboBox combo = o as ComboBox;
      if (o == null) 
        return;

      if (combo.GetActiveIter(out iter)) 
        selectedTable = (string) combo.Model.GetValue(iter, 0);

      populateGrid(morphose.GetModel().GetTable(selectedTable));
    }

  }
}

