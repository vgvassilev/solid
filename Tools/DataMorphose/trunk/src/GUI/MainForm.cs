/*
 * $Id: MainWindow.cs 555 2012-04-25 17:18:27Z ppetrova $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using DataMorphose.Actions;
using DataMorphose.Model;
using DataMorphose.Import;
 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DataMorphose.GUI
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
    private Database db;
    private List<Actions.Action> actions =  new List<Actions.Action>();
		
		public MainForm()
		{
			InitializeComponent();
		}
		
		void OpenToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (ofdDatabase.ShowDialog() == DialogResult.OK) {
        CSVImporter importer = new CSVImporter(/*firstRowIsHeader*/ true);
        db = importer.importDBFromFiles(ofdDatabase.FileName);
        PopulateGridView();
      }	
		}
		
		private void PopulateGridView()
		{
		  dataGridView1.Columns.Clear();
		  dataGridView1.Rows.Clear();
      Table tbl = db.Tables[2];
      foreach(Column col in tbl.Columns)
        dataGridView1.Columns.Add(col.Meta.Name, col.Meta.Name);
        
      for(int i = 0; i < tbl.Columns.Count; i++) {
        dataGridView1.Rows.Add(tbl.GetRow(i).Data);
      }
		}

		void ExitToolStripMenuItemClick(object sender, System.EventArgs e)
		{
			Close();
		}
		
		void RedoToolStripMenuItemClick(object sender, EventArgs e)
		{
		  SortByColumnAction a = new SortByColumnAction(db.Tables[2], /*columnIndex*/0);
		  actions.Add(a);
		  a.Redo();
		  PopulateGridView();		  
		}
		
		
		void UndoToolStripMenuItemClick(object sender, EventArgs e)
		{
		  //Undo the last action
		  actions[0].Undo();
		  PopulateGridView();
		}
	}
}
