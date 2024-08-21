using RUINOR.WinFormsUI.TreeViewColumns;

namespace RUINOR.WinFormsUI.TreeViewColumns.TreeViewColumnsDemo
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.treeViewColumns1 = new RUINOR.WinFormsUI.TreeViewColumns.TreeViewColumns();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // treeViewColumns1
            // 
            this.treeViewColumns1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.treeViewColumns1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewColumns1.Location = new System.Drawing.Point(0, 0);
            this.treeViewColumns1.Name = "treeViewColumns1";
            this.treeViewColumns1.Padding = new System.Windows.Forms.Padding(1);
            this.treeViewColumns1.Size = new System.Drawing.Size(1155, 563);
            this.treeViewColumns1.TabIndex = 0;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 160;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 160;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 563);
            this.Controls.Add(this.treeViewColumns1);
            this.Name = "Form1";
            this.Text = "Example TreeViewColumns (lite)";
            this.ResumeLayout(false);

		}

		#endregion

		private RUINOR.WinFormsUI.TreeViewColumns.TreeViewColumns treeViewColumns1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}

