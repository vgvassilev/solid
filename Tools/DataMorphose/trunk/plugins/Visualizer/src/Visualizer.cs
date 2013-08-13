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

namespace DataMorphose.Plugins.Visualizer
{
  public class Visualizer : IPlugin, IObserver
  {
    private IDataMorphose morphose = null;
    private DockItem dockItem = null;

    private Gtk.Notebook nb = new Gtk.Notebook();
    private Gtk.VBox vbox = new Gtk.VBox();
    private Gtk.TreeView treeView = new Gtk.TreeView();
    private Gtk.ComboBox comboBox = new Gtk.ComboBox();
    private SchemaVisualizer schemaV = null;

    private TreeIter iter;
    private string selectedTable = "";

    public Visualizer() {
    }

    #region IPlugin implementation
    void IPlugin.Init (object context)
    {
      morphose = context as IDataMorphose;
      morphose.GetModel().Attach(this);

      // Set up basic visualizer tab 
      // Attach the comboBox first
      vbox.PackStart(comboBox, false, false, 0);
      ScrolledWindow scroll =  new ScrolledWindow();
      // Attach the treeView to the scroll bar.
      scroll.AddWithViewport(treeView);
      // Attach the scrollBar to the box
      vbox.PackStart(scroll, true, true, 0);
      // Add the box to the main window
      nb.AppendPage(vbox, new Gtk.Label("Data Browser"));

      // Set up DrawingArea tab
      Gtk.DrawingArea drawingArea = new DrawingArea();
      ScrolledWindow scrollDA =  new ScrolledWindow();
      Viewport view = new Viewport();
      Gtk.VBox vBoxDA = new Gtk.VBox();
      view.Add(drawingArea);
      scrollDA.Add(view);
      vBoxDA.PackStart(scrollDA, true, true, 0);
      nb.AppendPage(vBoxDA, new Gtk.Label("Schema Visualizer"));
      schemaV = new SchemaVisualizer(drawingArea);

      nb.ShowAll();
      
      // Attaching the current dockItem in the DockFrame
      var MainWindow = morphose.GetMainWindow();
      
      dockItem = MainWindow.DockFrame.AddItem("Visualizer");
      dockItem.DrawFrame = true;
      dockItem.Label = "Grid";
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
    
    #region IObserver implementation
    void IObserver.Update (DataModel model)
    {
      populateComboBox(model.DB);
      schemaV.DrawSchema(model);
    }
    #endregion

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

      populateGrid(morphose.GetModel().DB.GetTable(selectedTable));
    }   
  }
}

