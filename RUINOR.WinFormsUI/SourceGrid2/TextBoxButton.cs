using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using SourceGrid2;
using SourceGrid2.Cells.Real;
namespace SourceGrid2
{
	/// <summary>
	/// Summary description for ComboBoxEx.
	/// </summary>
	public class TextBoxButton : System.Windows.Forms.UserControl
	{
		private CurTextBox txtBox;
		private System.Windows.Forms.Button btDown;
			
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TextBoxButton()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			btDown.BackColor = Color.FromKnownColor(KnownColor.Control);
			txtBox.AutoSize = false;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextBoxButton));
            this.txtBox = new SourceGrid2.CurTextBox();
            this.btDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtBox
            // 
            this.txtBox.AcceptsReturn = true;
            this.txtBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtBox.Location = new System.Drawing.Point(1, 1);
            this.txtBox.Name = "txtBox";
            this.txtBox.Size = new System.Drawing.Size(280, 14);
            this.txtBox.TabIndex = 0;
            // 
            // btDown
            // 
            this.btDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDown.Image = ((System.Drawing.Image)(resources.GetObject("btDown.Image")));
            this.btDown.Location = new System.Drawing.Point(264, -8);
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(22, 64);
            this.btDown.TabIndex = 1;
            this.btDown.Text = "...";
            this.btDown.Click += new System.EventHandler(this.btDown_Click);
            this.btDown.Paint += new System.Windows.Forms.PaintEventHandler(this.btDown_Paint);
            // 
            // TextBoxButton
            // 
            this.Controls.Add(this.txtBox);
            this.Controls.Add(this.btDown);
            this.Name = "TextBoxButton";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(282, 64);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TextBoxButton_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public void FillControl()
		{
			btDown.Size = new Size(22,Height);
			btDown.Location = new Point(Width-btDown.Width,0);
			txtBox.Location = new Point(1,1);
			txtBox.Size = new Size(Width-btDown.Width-2,Height-2);
		}

		private void btDown_Click(object sender, System.EventArgs e)
		{
			OnButtonClick(EventArgs.Empty);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			FillControl();
		}

//		private void txtBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
//		{
//			base.OnKeyDown(e);
//		}

		protected virtual void OnButtonClick(EventArgs e)
		{
			if (m_Cell==null)
				throw new ApplicationException("LoadCell not called");

			if (ButtonClick != null)
				ButtonClick(this,e);
		}

		public event EventHandler ButtonClick;

		private object m_EditObject = null;
		public object EditObject
		{
			get
			{
				if (m_Cell!=null && m_Cell.DataModel != null)
				{
					
						return m_Cell.DataModel.StringToValue(txtBox.Text);
					}
				else
					return m_EditObject;
			}
			set
			{
				if (m_Cell!=null && m_Cell.DataModel!=null)
				{
					
						txtBox.Text = m_Cell.DataModel.ValueToString(value);//.ObjectToString(value);
				
				}

				m_EditObject = value;
			}
		}

		private bool ReadOnlyTextBox
		{
			get{return txtBox.ReadOnly;}
			set{txtBox.ReadOnly = value;}
		}

		private Cell m_Cell = null;
		public Cell Cell
		{
			get{return m_Cell;}
		}

		public virtual void LoadCell(Cell p_Cell, object p_StartEditValue)
		{
			m_Cell = p_Cell;

			if (p_StartEditValue!=null)
				EditObject = p_StartEditValue;
			else
				EditObject = m_Cell.Value;;

//			if (m_Cell.DataModel.SupportStringConversion)
//				ReadOnlyTextBox = false;
//			else
//				ReadOnlyTextBox = true;

			SelectAllDisplayText();
		}

		public void SelectAllDisplayText()
		{
			txtBox.SelectAll();
		}

		private void btDown_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void TextBoxButton_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			e.Graphics.DrawRectangle(Pens.Black,0,0,this.Width-1,this.Height-1);
		}

		public TextBox TextBox
		{
			get{return txtBox;}
		}

		public System.Windows.Forms.Button Button
		{
			get{return btDown;}
		}
	}
}
