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
using DataMorphose.View;

using SolidV.MVC;

namespace DataMorphose.Plugins.Visualizer
{
  public class Visualizer : IPlugin
  {
    private IDataMorphose morphose = null;
    private DockItem dockItem = null;

    private Gtk.Notebook nb = new Gtk.Notebook();
//    private Gtk.Notebook nbBottom = new Gtk.Notebook();
    private Gtk.VBox vBoxTV = new Gtk.VBox();
    private Gtk.TreeView treeView = new Gtk.TreeView();
    private Gtk.ComboBox comboBox = new Gtk.ComboBox();
    private SchemaVisualizer schemaV = null;
    private Gtk.DrawingArea drawingArea = null;

    private TreeIter iter;
    private string selectedTable = "";

    public Visualizer() {
    }

    #region IPlugin implementation
    void IPlugin.Init (object context)
    {
      morphose = context as IDataMorphose;

      morphose.GetModel().ModelChanged += HandleModelChanged;

      // Set up data grid.
      // Attach the comboBox first.
      vBoxTV.PackStart(comboBox, false, false, 0);
      ScrolledWindow scrollTV =  new ScrolledWindow();
      // Attach the treeView to the scroll bar.
      scrollTV.AddWithViewport(treeView);
      // Attach the scrollBar to the box.
      vBoxTV.PackStart(scrollTV, true, true, 0);

      // Set up DrawingArea tab.
      drawingArea = new DrawingArea();
      ScrolledWindow scrollDA =  new ScrolledWindow();
      Gtk.VBox vBoxDA = new Gtk.VBox();
      Viewport viewPortDA = new Viewport();
      viewPortDA.Add(drawingArea);
      scrollDA.Add(viewPortDA);
      vBoxDA.PackStart(scrollDA, true, true, 0);

      // Add Paned container in order to make the two boxes resizable
      Gtk.VPaned splitter = new Gtk.VPaned();
      splitter.Pack1(vBoxDA, true, true);
      splitter.Pack2(vBoxTV, true, true);
      splitter.Position = 200;
      splitter.ShowAll();

      nb.AppendPage(splitter, new Gtk.Label("Schema Visualizer"));
      schemaV = new SchemaVisualizer(drawingArea);
      // Get the selected shape.
      SelectionModel selectionModel = schemaV.Selection;
      selectionModel.ModelChanged += HandleSelectionModelChanged;

      nb.ShowAll();
      
      // Attaching the current dockItem in the DockFrame.
      var MainWindow = morphose.GetMainWindow();

      dockItem = MainWindow.DockFrame.AddItem("Visualizer");
      dockItem.DrawFrame = true;
      dockItem.Label = "Visualizer";
      dockItem.Expand = true;
      dockItem.DrawFrame = true;
      dockItem.DefaultVisible = true;
      dockItem.Visible = true;
      dockItem.Content = nb;
    }

    void IPlugin.UnInit (object context)
    {
      throw new NotImplementedException ();
    }    

    #endregion

    private void HandleModelChanged(object model) {
      DataModel morphoseModel = (DataModel)model;
      populateComboBox(morphoseModel.DB);
      schemaV.DrawSchema(morphoseModel);
    }

    /// <summary>
    /// When a textblock shape is selected make a preview of the selected table.
    /// </summary>
    /// <param name="model">Model.</param>
    private void HandleSelectionModelChanged(object model) {
      SelectionModel selectionModel = model as SelectionModel;
      if (selectionModel.Selected.Count != 0) {
        string title = (selectionModel.Selected[0] as TextBlockShape).Title;
        populateGrid(morphose.GetModel().DB.GetTable(title));
      }
    }

    private void populateGrid(Model.Table table) {
      Gtk.ListStore store = treeView.Model as Gtk.ListStore;

      // Empty the store on every new table-selection.
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
      // Set up the comboBox.
      CellRendererText combocell = new CellRendererText();
      comboBox.PackStart(combocell, false);
      comboBox.AddAttribute(combocell, "text", 0);
      ListStore combostore = new ListStore(typeof(string));
      comboBox.Model = combostore;

      // Take the names of the tables.
      for (int i = 0; i < db.Tables.Count; ++i) {
        Model.Table table = db.Tables[i];
        combostore.AppendValues(table.Name);
      }
      comboBox.Changed += new EventHandler(OnComboBoxChanged);

      comboBox.ShowAll();
    }

    // Get the name of the selectedTable in the ComboBox and fill the grid with the data. 
    private void OnComboBoxChanged(object o, EventArgs args) {
      ComboBox combo = o as ComboBox;
      if (o == null) 
        return;

      if (combo.GetActiveIter(out iter)) 
        selectedTable = (string) combo.Model.GetValue(iter, 0);

      populateGrid(morphose.GetModel().DB.GetTable(selectedTable));
    }   
  }
}

