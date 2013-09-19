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
    private Toolbar tbLeft = new Gtk.Toolbar();
    private Gtk.Notebook nbMiddleDock = new Gtk.Notebook();
    private Gtk.Notebook nbLeftDock = new Gtk.Notebook();
    private Gtk.VBox vbRight = new Gtk.VBox();
    private Gtk.VBox vbMiddle = new Gtk.VBox();
    private Gtk.TreeView treeView = new Gtk.TreeView();
    private Gtk.ComboBox comboBox = new Gtk.ComboBox();
    private SchemaVisualizer schemaV = null;
    private Gtk.DrawingArea drawingArea = null;
    private TreeIter iter;
    private string selectedTable = "";
    private SolidV.Gtk.ObjectInspector propertyGrid;

    public Visualizer() {
    }

    #region IPlugin implementation
    void IPlugin.Init(object context) {
      morphose = context as IDataMorphose;

      morphose.GetModel().ModelChanged += HandleModelChanged;
         
      // Left dock item.
      TuneLeftDockItem();
      AddDockItem("Tools", nbLeftDock);
      
      // Attaching the Visualizer in the DockFrame.
      AddDockItem("Visualizer", nbMiddleDock);
      TuneMiddleDockItem();

      // Right dock item. 
      TuneRightDockItem();
      AddDockItem("Actions", vbRight);
    }    

    /// <summary>
    /// Adds the dock item.
    /// </summary>
    /// <param name="label">Label.</param>
    /// <param name="container">Container.</param>
    private void AddDockItem(string label, Gtk.Container container) {
      DockItem dockToolbar = morphose.GetMainWindow().DockFrame.AddItem(label);
      dockToolbar.DrawFrame = true;
      dockToolbar.Label = label;
      dockToolbar.Expand = true;
      dockToolbar.DefaultVisible = true;
      dockToolbar.Visible = true;
      dockToolbar.Content = container;
      container.ShowAll();
    }

    /// <summary>
    /// Tunes the left dock item.
    /// </summary>
    private void TuneLeftDockItem() {      
      Gtk.VBox vbox = new Gtk.VBox();
      tbLeft.Name = "LeftToolbar";
      vbox.PackStart(tbLeft, true, true, 0);
      tbLeft.Orientation = Gtk.Orientation.Vertical;
      // Show only text on the buttons
      tbLeft.ToolbarStyle = Gtk.ToolbarStyle.Text;
      
      nbLeftDock.Add(vbox);;
      
      this.propertyGrid = new SolidV.Gtk.ObjectInspector();
      nbLeftDock.AppendPage(propertyGrid, new Label("Properties"));
      nbLeftDock.ShowAll();
    }

    /// <summary>
    /// Tunes the middle dock item or the data visualizer.
    /// </summary>
    private void TuneMiddleDockItem() {
      // Attach the comboBox first.
      vbMiddle.PackStart(comboBox, false, false, 0);
      ScrolledWindow scroll = new ScrolledWindow();
      // Attach the treeView to the scroll bar.
      scroll.AddWithViewport(treeView);
      // Attach the scrollBar to the box.
      vbMiddle.PackStart(scroll, true, true, 0);
      
      // Set up DrawingArea.
      drawingArea = new DrawingArea();
      drawingArea.SetSizeRequest(800, 500);
      ScrolledWindow scrollDA = new ScrolledWindow();
      Gtk.VBox vBoxDA = new Gtk.VBox();
      Viewport viewPortDA = new Viewport();
      viewPortDA.Add(drawingArea);
      scrollDA.Add(viewPortDA);
      vBoxDA.PackStart(scrollDA, true, true, 0);
      
      // Add Paned container in order to make the two boxes resizable
      Gtk.VPaned splitter = new Gtk.VPaned();
      splitter.Pack1(vBoxDA, true, true);
      splitter.Pack2(vbMiddle, true, true);
      splitter.Position = 400;
      splitter.ShowAll();

      nbMiddleDock.AppendPage(splitter, new Gtk.Label("Schema Visualizer"));
      nbMiddleDock.ShowAll();

      schemaV = new SchemaVisualizer(drawingArea);
      
      // Get the selected shape.
      SelectionModel selectionModel = schemaV.Selection;
      selectionModel.ModelChanged += HandleSelectionModelChanged;
    }

    /// <summary>
    /// Tunes the right dock item.
    /// </summary>
    private void TuneRightDockItem() {
      Gtk.Toolbar tb = new Gtk.Toolbar();
      tb.Name = "RightToolbar";
      
      // Add TreeView for the history 
      Gtk.TreeView historyView = new Gtk.TreeView();
      Gtk.TreeViewColumn languages = new Gtk.TreeViewColumn();
      languages.Title = "History";
      
      // Write some sample data to see how it looks like
      Gtk.CellRendererText cell = new Gtk.CellRendererText();
      languages.PackStart(cell, true);
      languages.AddAttribute(cell, "text", 0);
      
      Gtk.TreeStore treestore = new Gtk.TreeStore(typeof(string));
      treestore.AppendValues("Import database");
      treestore.AppendValues("Set primary keys");
      treestore.AppendValues("Delete row");
      
      historyView.AppendColumn(languages);
      historyView.Model = treestore;
      
      // We need to set the expand and fill properties to false, because that way we can resize 
      // the buttons.
      vbRight.PackStart(tb, false, false, 0);
      vbRight.PackStart(historyView, true, true, 0);
      
      tb.ToolbarStyle = Gtk.ToolbarStyle.Icons;
      tb.Orientation = Gtk.Orientation.Horizontal;
      
      Gtk.ToolButton undo = new Gtk.ToolButton(Gtk.Stock.Undo);
      Gtk.ToolButton redo = new Gtk.ToolButton(Gtk.Stock.Redo);
      tb.Insert(undo, 0);
      tb.Insert(redo, 1);    
    }

    void IPlugin.UnInit(object context) {
      throw new NotImplementedException();
    }    

    #endregion

    private void HandleModelChanged(object model) {
      DataModel morphoseModel = (DataModel)model;
      AddButtonsToTheLeftToolBar(morphose.GetModel().DB);
      populateComboBox(morphoseModel.DB);
    }

    /// <summary>
    /// When a textblock shape is selected make a preview of the selected table.
    /// </summary>
    /// <param name="model">SelectionModel.</param>
    private void HandleSelectionModelChanged(object model) {
      SelectionModel selectionModel = model as SelectionModel;
      if (selectionModel.Selected.Count != 0) {
        var selected = selectionModel.Selected[0];
        // FIXME: Handle when more than one.
        propertyGrid.CurrentObject = selected;
        //propertyGrid.Populate();
        if (selected is TextBlockShape) {
          string title = (selected as TextBlockShape).Title;
          populateGrid(morphose.GetModel().DB.GetTable(title));
        } else if (selected is EllipseShape)
            for (int i = 0; i < schemaV.Scene.Shapes.Count; i++) {
              Shape block = schemaV.Scene.Shapes[0];
              if (block is TextBlockShape) {
                string title = (block as TextBlockShape).Title;
                ViewFilterResults(new Filter(1, Filter.ConditionRelations.LessThan), 
                              morphose.GetModel().DB.GetTable(title));
              }
            }
      }
    }

    /// <summary>
    /// Adds buttons that will put a table on the screen.
    /// </summary>
    /// <param name="db">Database</param>
    private void AddButtonsToTheLeftToolBar(Database db) {
      for (int i = 0; i < db.Tables.Count; i++) {
        Gtk.ToolButton btn = new Gtk.ToolButton(Gtk.Stock.About);        
        btn.Label = db.Tables[i].Name;
        tbLeft.Insert(btn, i);
        btn.Clicked += OnTableBtnClick;
      }
      Gtk.ToolButton filterBtn = new Gtk.ToolButton(Gtk.Stock.About);
      filterBtn.Label = "Add Filter";
      tbLeft.Insert(filterBtn, tbLeft.NItems);
      tbLeft.ShowAll();
      filterBtn.Clicked += OnFilterBtnPressed;
    }

    /// <summary>
    /// Draws a table on the screen when a table-button is clicked. 
    /// </summary>
    /// <param name="sender">ToolButton</param>
    /// <param name="args">Arguments.</param>
    private void OnTableBtnClick(object sender, EventArgs args) {
      Gtk.ToolButton btn = (Gtk.ToolButton)sender;
      schemaV.Model.BeginUpdate();
      schemaV.DrawSchema(morphose.GetModel().DB.GetTable(btn.Label));
      schemaV.Scene.AutoArrange();
      schemaV.Model.EndUpdate();
    }

    /// <summary>
    /// Draws a shape used as a filter.
    /// </summary>
    /// <param name="o">O.</param>
    /// <param name="args">Arguments.</param>
    private void OnFilterBtnPressed(object o, EventArgs args) {
      morphose.GetModel().DB.Tables[0].Columns[0].Filter 
        = new Filter(1, Filter.ConditionRelations.LessThan);
      schemaV.DrawFilter();
    }

    private void ViewFilterResults(Filter filter, Model.Table table) {
      if (filter.Condition == 1) {

        Gtk.ListStore store = treeView.Model as Gtk.ListStore;
        
        // Empty the store on every new table-selection.
        if (store != null) {
          foreach (Gtk.TreeViewColumn col in treeView.Columns)
            treeView.RemoveColumn(col);
          store.Clear();
        }
        
        // Construct the array of types which is the size of the columns.
        Type[] types = new Type[table.Columns.Count];
        for (int i = 0, e = table.Columns.Count; i < e; ++i)
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
        for (int i = 0; i < table.GetAsRows().Count; i++) {
          Model.Row r = table.GetAsRows()[i];
          if (table.Columns[0].Values[i].ToString().Contains("1")) {
            store.AppendValues(r.GetAsStringArray());
          }
        }
        treeView.ShowAll();
      }    

    }

    /// <summary>
    /// Populates the data from a selected table.
    /// </summary>
    /// <param name="table">Table.</param>
    private void populateGrid(Model.Table table) {
      Gtk.ListStore store = treeView.Model as Gtk.ListStore;

      // Empty the store on every new table-selection.
      if (store != null) {
        foreach (Gtk.TreeViewColumn col in treeView.Columns)
          treeView.RemoveColumn(col);
        store.Clear();
      }

      // Construct the array of types which is the size of the columns.
      Type[] types = new Type[table.Columns.Count];
      for (int i = 0, e = table.Columns.Count; i < e; ++i)
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
      foreach (Model.Row r in table.GetAsRows()) {
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
        selectedTable = (string)combo.Model.GetValue(iter, 0);

      populateGrid(morphose.GetModel().DB.GetTable(selectedTable));
    }
  }
}

