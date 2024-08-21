using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SourceLibrary.Windows.Forms
{
	/// <summary>
	/// Summary description for ColorPicker.
	/// </summary>
	public class ColorPicker : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.Button btBrowse;
		private System.Windows.Forms.Panel panelColor;
		private System.Windows.Forms.Label labelColor;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ColorPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			SelectedColor = Color.Black;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public new Color BackColor
		{
			get{return panel.BackColor;}
			set{panel.BackColor = value;}
		}

		public virtual BorderStyle BorderStyle
		{
			get{return panel.BorderStyle;}
			set{panel.BorderStyle = value;}
		}

		public virtual Color SelectedColor
		{
			get{return panelColor.BackColor;}
			set{panelColor.BackColor = value;}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel = new System.Windows.Forms.Panel();
			this.labelColor = new System.Windows.Forms.Label();
			this.panelColor = new System.Windows.Forms.Panel();
			this.btBrowse = new System.Windows.Forms.Button();
			this.panel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel
			// 
			this.panel.BackColor = System.Drawing.SystemColors.Window;
			this.panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel.Controls.Add(this.labelColor);
			this.panel.Controls.Add(this.panelColor);
			this.panel.Controls.Add(this.btBrowse);
			this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel.Location = new System.Drawing.Point(0, 0);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(244, 44);
			this.panel.TabIndex = 0;
			// 
			// labelColor
			// 
			this.labelColor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelColor.Location = new System.Drawing.Point(44, 0);
			this.labelColor.Name = "labelColor";
			this.labelColor.Size = new System.Drawing.Size(162, 40);
			this.labelColor.TabIndex = 2;
			this.labelColor.Text = "Black";
			this.labelColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panelColor
			// 
			this.panelColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.panelColor.BackColor = System.Drawing.Color.Black;
			this.panelColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelColor.Location = new System.Drawing.Point(4, 2);
			this.panelColor.Name = "panelColor";
			this.panelColor.Size = new System.Drawing.Size(32, 36);
			this.panelColor.TabIndex = 1;
			this.panelColor.BackColorChanged += new System.EventHandler(this.panelColor_BackColorChanged);
			// 
			// btBrowse
			// 
			this.btBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.btBrowse.BackColor = System.Drawing.SystemColors.Control;
			this.btBrowse.Location = new System.Drawing.Point(216, 0);
			this.btBrowse.Name = "btBrowse";
			this.btBrowse.Size = new System.Drawing.Size(24, 40);
			this.btBrowse.TabIndex = 0;
			this.btBrowse.Text = "...";
			this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
			// 
			// ColorPicker
			// 
			this.Controls.Add(this.panel);
			this.Name = "ColorPicker";
			this.Size = new System.Drawing.Size(244, 44);
			this.panel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void panelColor_BackColorChanged(object sender, System.EventArgs e)
		{
			labelColor.Text = panelColor.BackColor.Name;

			if (SelectedColorChanged!=null)
				SelectedColorChanged(this, e);
		}

		private void btBrowse_Click(object sender, System.EventArgs e)
		{
			try
			{
				using (ColorDialog l_dlg = new ColorDialog())
				{
					l_dlg.Color = SelectedColor;
					if (l_dlg.ShowDialog(this) == DialogResult.OK)
					{
						SelectedColor = l_dlg.Color;
					}
				}
			}
			catch(Exception )
			{
			}
		}

		public new Color ForeColor
		{
			get{return labelColor.ForeColor;}
			set{labelColor.ForeColor = value;}
		}


		public event EventHandler SelectedColorChanged;
	}
}
