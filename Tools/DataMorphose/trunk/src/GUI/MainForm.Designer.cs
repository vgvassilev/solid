/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 29.4.2012 г.
 * Time: 22:44 ч.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace DataMorphose.GUI
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
		  this.ofdDatabase = new System.Windows.Forms.OpenFileDialog();
		  this.mainMenu = new System.Windows.Forms.MenuStrip();
		  this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		  this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		  this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		  this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		  this.dataGridView1 = new System.Windows.Forms.DataGridView();
		  this.mainMenu.SuspendLayout();
		  ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
		  this.SuspendLayout();
		  // 
		  // ofdDatabase
		  // 
		  this.ofdDatabase.Filter = "Database description files (*.csvdb)|*.csvdb";
		  this.ofdDatabase.Title = "Choose Database";
		  // 
		  // mainMenu
		  // 
		  this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
		  		  		  this.fileToolStripMenuItem,
		  		  		  this.helpToolStripMenuItem});
		  this.mainMenu.Location = new System.Drawing.Point(0, 0);
		  this.mainMenu.Name = "mainMenu";
		  this.mainMenu.Size = new System.Drawing.Size(920, 24);
		  this.mainMenu.TabIndex = 0;
		  // 
		  // fileToolStripMenuItem
		  // 
		  this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
		  		  		  this.openToolStripMenuItem,
		  		  		  this.exitToolStripMenuItem});
		  this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
		  this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
		  this.fileToolStripMenuItem.Text = "File";
		  // 
		  // openToolStripMenuItem
		  // 
		  this.openToolStripMenuItem.Name = "openToolStripMenuItem";
		  this.openToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
		  this.openToolStripMenuItem.Text = "Open";
		  this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItemClick);
		  // 
		  // exitToolStripMenuItem
		  // 
		  this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
		  this.exitToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
		  this.exitToolStripMenuItem.Text = "Exit";
		  this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
		  // 
		  // helpToolStripMenuItem
		  // 
		  this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
		  this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
		  this.helpToolStripMenuItem.Text = "Help";
		  // 
		  // dataGridView1
		  // 
		  this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		  this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
		  this.dataGridView1.Location = new System.Drawing.Point(0, 231);
		  this.dataGridView1.Name = "dataGridView1";
		  this.dataGridView1.Size = new System.Drawing.Size(920, 150);
		  this.dataGridView1.TabIndex = 1;
		  // 
		  // MainForm
		  // 
		  this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		  this.ClientSize = new System.Drawing.Size(920, 381);
		  this.Controls.Add(this.dataGridView1);
		  this.Controls.Add(this.mainMenu);
		  this.MainMenuStrip = this.mainMenu;
		  this.Name = "MainForm";
		  this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		  this.Text = "MainForm";
		  this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		  this.mainMenu.ResumeLayout(false);
		  this.mainMenu.PerformLayout();
		  ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
		  this.ResumeLayout(false);
		  this.PerformLayout();
		}
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.OpenFileDialog ofdDatabase;
	}
}
