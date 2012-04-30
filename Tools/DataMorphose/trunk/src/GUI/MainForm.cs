/*
 * $Id: MainWindow.cs 555 2012-04-25 17:18:27Z ppetrova $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using DataMorphose.Model;
using DataMorphose.Import;
 
using System;
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
		
		public MainForm()
		{
			InitializeComponent();
		}
		
		void OpenToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (ofdDatabase.ShowDialog() == DialogResult.OK) {
        CSVImporter importer = new CSVImporter();
        db = importer.importDBFromFiles(ofdDatabase.FileName);
        Table tbl = db.Tables[2];
        foreach(Column headerCol in tbl.Header.Columns)
          dataGridView1.Columns.Add(headerCol.Name, headerCol.Name);
        
        foreach(Row row in tbl.Rows) {
          dataGridView1.Rows.Add(row.GetColumnsValues());
        }
      }	
		}

		void ExitToolStripMenuItemClick(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
