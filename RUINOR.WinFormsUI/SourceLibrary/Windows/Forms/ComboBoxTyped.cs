using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SourceLibrary.Windows.Forms
{
	/// <summary>
	/// Control to simulate a ComboBox, because the one provided with the Framework doesn't support vertical sizing different from the size of the font.
	/// </summary>
	public class ComboBoxTyped : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button btDown;
		private SourceLibrary.Windows.Forms.TextBoxTyped txtBox;
		
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Constructor
		/// </summary>
		public ComboBoxTyped()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			btDown.BackColor = Color.FromKnownColor(KnownColor.Control);
			txtBox.LoadingValidator += new EventHandler(txtBox_LoadingValidator);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ComboBoxTyped));
			this.btDown = new System.Windows.Forms.Button();
			this.txtBox = new SourceLibrary.Windows.Forms.TextBoxTyped();
			this.SuspendLayout();
			// 
			// btDown
			// 
			this.btDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.btDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btDown.Image = ((System.Drawing.Image)(resources.GetObject("btDown.Image")));
			this.btDown.Location = new System.Drawing.Point(142, 0);
			this.btDown.Name = "btDown";
			this.btDown.Size = new System.Drawing.Size(18, 20);
			this.btDown.TabIndex = 1;
			this.btDown.Click += new System.EventHandler(this.btDown_Click);
			// 
			// txtBox
			// 
			this.txtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtBox.AutoSize = false;
			this.txtBox.BackColor = System.Drawing.Color.White;
			this.txtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtBox.EnableAutoValidation = false;
			this.txtBox.EnableEnterKeyValidate = false;
			this.txtBox.EnableEscapeKeyUndo = true;
			this.txtBox.EnableLastValidValue = true;
			this.txtBox.ErrorProvider = null;
			this.txtBox.ErrorProviderMessage = "Invalid value";
			this.txtBox.ForceFormatText = true;
			this.txtBox.HideSelection = false;
			this.txtBox.Location = new System.Drawing.Point(0, 0);
			this.txtBox.Name = "txtBox";
			this.txtBox.Size = new System.Drawing.Size(142, 20);
			this.txtBox.TabIndex = 0;
			this.txtBox.Text = "";
			this.txtBox.WordWrap = false;
			this.txtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
			this.txtBox.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
			// 
			// ComboBoxTyped
			// 
			this.Controls.Add(this.txtBox);
			this.Controls.Add(this.btDown);
			this.Name = "ComboBoxTyped";
			this.Size = new System.Drawing.Size(160, 20);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Required
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComponentModel.Validator.IValidator Validator
		{
			get{return txtBox.Validator;}
			set{txtBox.Validator = value;}		
		}

		/// <summary>
		/// Reload the properties from the validator
		/// </summary>
		public virtual void OnLoadingValidator()
		{
			if (LoadingValidator != null)
				LoadingValidator(this, EventArgs.Empty);
		}

		/// <summary>
		/// 
		/// </summary>
		public event EventHandler LoadingValidator;

		/// <summary>
		/// True to set the textbox readonly otherwise false.
		/// </summary>
		public bool ReadOnlyTextBox
		{
			get{return txtBox.ReadOnly;}
			set{txtBox.ReadOnly = value;}
		}

		private int m_iSelectedItem = -1;
		/// <summary>
		/// Selected Index of the Items array. -1 if no value is selected or if the value is not in the Items list.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedIndex
		{
			get{return m_iSelectedItem;}
			set{m_iSelectedItem = value;OnSelectedIndexChanged();}
		}

		private string[] Vals=null;
		public string[] vals
		{
			get
			{
				return Vals;
			}
			set
			{
				Vals=value;
			}
		}
		/// <summary>
		/// Populate and show the listbox
		/// </summary>
		public virtual void ShowListBox()
		{
			OnDropDownOpen(EventArgs.Empty);

			using (ListBox l_ListBox = new ListBox())
			{
				l_ListBox.BorderStyle = BorderStyle.None;
				if (Validator.StandardValues != null)
				{
					foreach (object o in Validator.StandardValues)
					{
						l_ListBox.Items.Add(Validator.ValueToDisplayString(o));
					}
				}
				l_ListBox.Items.AddRange(this.vals);

				using (SourceLibrary.Windows.Forms.DropDownCustom l_DropDown = new SourceLibrary.Windows.Forms.DropDownCustom(this,l_ListBox))
				{
					if (m_iSelectedItem >= 0 && m_iSelectedItem < l_ListBox.Items.Count)
					{
						l_ListBox.SelectedIndex = m_iSelectedItem;
					}
					l_ListBox.SelectedIndexChanged += new EventHandler(ListBox_SelectedChange);
					l_ListBox.Click += new EventHandler(ListBox_Click);
					l_DropDown.ShowDropDown();
					l_ListBox.Click -= new EventHandler(ListBox_Click);
					l_ListBox.SelectedIndexChanged -= new EventHandler(ListBox_SelectedChange);

					txtBox.Focus();

					OnDropDownClosed(EventArgs.Empty);
				}
			}
		}


		/// <summary>
		/// Fired when the SelectedIndex property change
		/// </summary>
		protected virtual void OnSelectedIndexChanged()
		{
			try 
			{
				txtBox.Value = vals[m_iSelectedItem];

			}
			catch(Exception)
			{}
			if (Validator.StandardValues != null &&
				m_iSelectedItem != -1 && 
				m_iSelectedItem < Validator.StandardValues.Count)
			{
				try
				{
					m_bEditTxtBoxByCode = true;//per disabilitare l'evento txtBoxChange

					txtBox.Value = Validator.StandardValueAtIndex(m_iSelectedItem);

					SelectAllTextBox();
				}
				finally
				{
					m_bEditTxtBoxByCode = false;
				}
			}
		}

		/// <summary>
		/// Returns the string valud at the specified index using the editor. If index is not valid return Validator.NullDisplayString.
		/// </summary>
		/// <param name="p_Index"></param>
		/// <returns></returns>
		protected virtual string GetStringValueAtIndex(int p_Index)
		{
			if (Validator.StandardValues != null &&
				p_Index >= 0 && 
				p_Index < Validator.StandardValues.Count)
			{
				if (Validator.IsStringConversionSupported())
					return Validator.ValueToString(Validator.StandardValueAtIndex(p_Index));
				else
					return Validator.ValueToDisplayString(Validator.StandardValueAtIndex(p_Index));
			}
			else
				return Validator.NullDisplayString;
		}

		private bool m_bEditTxtBoxByCode = false;

		private void ListBox_SelectedChange(object sender, EventArgs e)
		{
			SelectedIndex = ((ListBox)sender).SelectedIndex;
		}

		/// <summary>
		/// Gets or sets the current value of the editor.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Value
		{
			get
			{
				return txtBox.Value;
			}
			set
			{
				txtBox.Value = value;

				//provo a cercare il valore nell'elenco di valori attualmente nella lista in modo da poterlo selezionare
				if (Validator.StandardValues != null)
					m_iSelectedItem = Validator.StandardValuesIndexOf(value);
				else
					m_iSelectedItem = -1;
			}
		}

//		protected virtual void FillControl()
//		{
//			btDown.Size = new Size(18,Height);
//			btDown.Location = new Point(Width-btDown.Width,0);
//			txtBox.Location = new Point(0,0);
//			txtBox.Size = new Size(Width-btDown.Width,Height);
//		}

		private void btDown_Click(object sender, System.EventArgs e)
		{
			ShowListBox();
		}

		/// <summary>
		/// Select all the text of the textbox
		/// </summary>
		public void SelectAllTextBox()
		{
			txtBox.SelectAll();
		}

//		protected override void OnSizeChanged(EventArgs e)
//		{
//			base.OnSizeChanged(e);
//			FillControl();
//		}

		private void txtBox_TextChanged(object sender, System.EventArgs e)
		{
			if (m_bEditTxtBoxByCode==false)
			{
				m_iSelectedItem = -1;
			}
		}

		private void txtBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
			{
				int l_SelectedIndex = m_iSelectedItem;
				if (e.KeyCode == Keys.Down)
					l_SelectedIndex++;
				else if (e.KeyCode == Keys.Up)
					l_SelectedIndex--;

				//controllo che sia valido
				if (l_SelectedIndex >= 0 && Validator.StandardValues != null && l_SelectedIndex < Validator.StandardValues.Count)
				{
					SelectedIndex = l_SelectedIndex;
				}
			}
		}

		/// <summary>
		/// The button in the right of the editor
		/// </summary>
		public Button Button
		{
			get{return btDown;}
		}

		public SourceLibrary.Windows.Forms.TextBoxTyped TextBox
		{
			get{return txtBox;}
		}

		private void ListBox_Click(object sender, EventArgs e)
		{
			txtBox.Focus();
		}

		/// <summary>
		/// Fired when showing the drop down
		/// </summary>
		public event EventHandler DropDownOpen;

		/// <summary>
		/// Fired when showing the drop down
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDropDownOpen(EventArgs e)
		{
			if (DropDownOpen!=null)
				DropDownOpen(this,e);
		}

		/// <summary>
		/// Fired when closing the dropdown
		/// </summary>
		public event EventHandler DropDownClosed;

		/// <summary>
		/// Fired when closing the dropdown
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDropDownClosed(EventArgs e)
		{
			if (DropDownClosed!=null)
				DropDownClosed(this,e);
		}

		#region Properties
		/// <summary>
		/// Indicates if after the Validating event the Text is refreshed with the new value, forcing the correct formatting.
		/// </summary>
		public bool ForceFormatText
		{
			get{return txtBox.ForceFormatText;}
			set{txtBox.ForceFormatText = value;}
		}


		/// <summary>
		/// True to enable the Escape key to undo any changes. Default is true.
		/// </summary>
		public bool EnableEscapeKeyUndo
		{
			get{return txtBox.EnableEscapeKeyUndo;}
			set{txtBox.EnableEscapeKeyUndo = value;}
		}

		/// <summary>
		/// True to enable the Enter key to validate any changes. Default is true.
		/// </summary>
		public bool EnableEnterKeyValidate
		{
			get{return txtBox.EnableEnterKeyValidate;}
			set{txtBox.EnableEnterKeyValidate = value;}
		}

		/// <summary>
		/// True to enable the validation of the textbox text when the Validating event is fired, to force always the control to be valid. Default is true.
		/// </summary>
		public bool EnableAutoValidation
		{
			get{return txtBox.EnableAutoValidation;}
			set{txtBox.EnableAutoValidation = value;}
		}

		/// <summary>
		/// True to allow the Value property to always return a valid value when the textbox.text is not valid, false to throw an error when textbox.text is not valid.
		/// </summary>
		public bool EnableLastValidValue
		{
			get{return txtBox.EnableLastValidValue;}
			set{txtBox.EnableLastValidValue = value;}
		}
		#endregion

		private void txtBox_LoadingValidator(object sender, EventArgs e)
		{
			OnLoadingValidator();
		}
	}
}
