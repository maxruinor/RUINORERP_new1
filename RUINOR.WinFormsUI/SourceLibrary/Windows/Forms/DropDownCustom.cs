using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

//Thanks King Rufus and his "Table Size Selector Control"
// http://www.codeproject.com/useritems/TableSizeSelector.asp

namespace SourceLibrary.Windows.Forms
{
	/// <summary>
	/// Summary description for DropDownCustom.
	/// </summary>
	public class DropDownCustom : System.Windows.Forms.Form
	{
		private Point StartLocation = new Point(0,0);
		private Rectangle ParentRectangle;
		private System.Windows.Forms.Panel panelContainer;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DropDownCustom()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}
		public DropDownCustom(Control p_ParentControl, Control p_InnerControl):this()
		{
			m_InnerControl = p_InnerControl;
			m_ParentControl = p_ParentControl;
		}

		private void InnerControl_Resize(object sender, EventArgs e)
		{
			if (m_InnerControl != null)
			{
				Size = m_InnerControl.Size;
			}
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panelContainer = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// panelContainer
			// 
			this.panelContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelContainer.Location = new System.Drawing.Point(0, 0);
			this.panelContainer.Name = "panelContainer";
			this.panelContainer.Size = new System.Drawing.Size(84, 48);
			this.panelContainer.TabIndex = 0;
			// 
			// ctlDropDownCustom
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(84, 48);
			this.ControlBox = false;
			this.Controls.Add(this.panelContainer);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ctlDropDownCustom";
			this.ShowInTaskbar = false;
			this.Text = "ctlDropDownCustom";
			this.Activated += new System.EventHandler(this.ctlDropDownCustom_Activated);
			this.Deactivate += new System.EventHandler(this.ctlDropDownCustom_Deactivate);
			this.ResumeLayout(false);

		}
		#endregion

		private Control m_ParentControl = null;
		private Control m_InnerControl = null;

		public Control ParentControl
		{
			get{return m_ParentControl;}
			set{m_ParentControl = value;}
		}

		public Control InnerControl
		{
			get{return m_InnerControl;}
			set{m_InnerControl = value;}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if (m_InnerControl != null && m_ParentControl != null)
			{
				m_InnerControl.Width = Math.Max(m_ParentControl.Width,m_InnerControl.Width);
				ParentRectangle = m_ParentControl.RectangleToScreen(m_ParentControl.ClientRectangle);
				panelContainer.Controls.Add(m_InnerControl);
				m_InnerControl.Location = new Point(0,0);
				//m_InnerControl.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
				Size = m_InnerControl.Size;
			}
		}

		private void CalcLocation()
		{
			// Determine which screen we're on and how big it is.
			Screen DisplayedOnScreen = Screen.FromPoint(new Point(ParentRectangle.X, ParentRectangle.Bottom));
			int MinScreenXPos = DisplayedOnScreen.Bounds.X;
			int MinScreenYPos = DisplayedOnScreen.Bounds.Y;
			int MaxScreenXPos = DisplayedOnScreen.Bounds.X + DisplayedOnScreen.Bounds.Width;
			int MaxScreenYPos = DisplayedOnScreen.Bounds.Y + DisplayedOnScreen.Bounds.Height;

			int DropdownWidth  = Width; //CalcWidth();
			int DropdownHeight = Height; //CalcHeight();

			// Will we bump into the right edge of the window when we first display the control?
			if((ParentRectangle.X + DropdownWidth) <= MaxScreenXPos )
			{
				if( ParentRectangle.X < MinScreenXPos )
					StartLocation.X = MinScreenXPos;
				else
					StartLocation.X = ParentRectangle.X;
			}
			else
			{
				//DrawLeftToRight = false;

				// Make sure we aren't overhanging the left side of the screen.
				if( Screen.FromPoint(new Point((ParentRectangle.X + ParentRectangle.Width), 0)) == DisplayedOnScreen )
					StartLocation.X = ParentRectangle.Right-DropdownWidth;
				else
					StartLocation.X = MaxScreenXPos - DropdownWidth;
			}

			// And now check the bottom of the screen.
			if( (ParentRectangle.Bottom + DropdownHeight) <= MaxScreenYPos )
				StartLocation.Y = ParentRectangle.Bottom;
			else
			{
				//DrawTopToBottom = false;
				StartLocation.Y = ParentRectangle.Y-DropdownHeight;
			}

			this.Location = StartLocation;
		}

		private void ctlDropDownCustom_Activated(object sender, System.EventArgs e)
		{
			if (m_InnerControl != null && m_ParentControl != null)
			{
				CalcLocation();
			}
		}

		protected override bool ProcessCmdKey(
			ref Message msg,
			Keys keyData
			)
		{
			if ( (m_Flags & DropDownFlags.CloseOnEscape) == DropDownFlags.CloseOnEscape)
			{
				if (keyData == Keys.Escape)
				{
					DialogResult = DialogResult.Cancel;
					Hide();
					//return true; altrimenti alcuni controlli che gestiscono i tasti non funzionano bene (ad esempio i controlli UITypeEditor)
				}
			}

			if ( (m_Flags & DropDownFlags.CloseOnEnter) == DropDownFlags.CloseOnEnter)
			{
				if (keyData == Keys.Enter)
				{
					DialogResult = DialogResult.OK;
					Hide();
					//return true; altrimenti alcuni controlli che gestiscono i tasti non funzionano bene (ad esempio i controlli UITypeEditor)
				}
			}

			return base.ProcessCmdKey(ref msg,keyData);
		}

		private DropDownFlags m_Flags = DropDownFlags.CloseOnEnter | DropDownFlags.CloseOnEscape;
		public DropDownFlags DropDownFlags
		{
			get{return m_Flags;}
			set{m_Flags = value;}
		}

		private bool m_bIsDeactivated = false;
		private void ctlDropDownCustom_Deactivate(object sender, System.EventArgs e)
		{
			m_bIsDeactivated = true;
			Hide();
		}

		public void ShowDropDown()
		{
			m_InnerControl.Resize += new EventHandler(InnerControl_Resize);

			Show();

			Size = m_InnerControl.Size;

			//wait until form deactivated
			while(m_bIsDeactivated==false)
				Application.DoEvents();

			m_InnerControl.Resize -= new EventHandler(InnerControl_Resize);
		}
	}

	public enum DropDownFlags
	{
		None = 0,
		/// <summary>
		/// Close the DropDown whe the user press the escape key, return DialogResult.Cancel
		/// </summary>
		CloseOnEscape = 1,
		/// <summary>
		/// Close the DropDown whe the user press the enter key, return DialogResult.OK
		/// </summary>
		CloseOnEnter = 2
	}
}
