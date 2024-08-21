using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using RUINORERP.UI.UCSourceGrid;

namespace RUINORERP.UI.UCSourceGrid
{
	/// <summary>
	/// Control to simulate a ComboBox, because the one provided with the Framework doesn't support vertical sizing different from the size of the font.
	/// </summary>
	public class TextBoxTypedButtonQuery : System.Windows.Forms.UserControl
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
		public TextBoxTypedButtonQuery()
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
			this.btDown = new System.Windows.Forms.Button();
			this.txtBox = new SourceLibrary.Windows.Forms.TextBoxTyped();
			this.SuspendLayout();
			// 
			// btDown
			// 
			this.btDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.btDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btDown.Location = new System.Drawing.Point(136, 0);
			this.btDown.Name = "btDown";
			this.btDown.Size = new System.Drawing.Size(24, 20);
			this.btDown.TabIndex = 1;
			this.btDown.Text = "...";
			this.btDown.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
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
			this.txtBox.Size = new System.Drawing.Size(136, 20);
			this.txtBox.TabIndex = 0;
			this.txtBox.Text = "";
			this.txtBox.WordWrap = false;
			// 
			// TextBoxTypedButton
			// 
			this.Controls.Add(this.txtBox);
			this.Controls.Add(this.btDown);
			this.Name = "TextBoxTypedButton";
			this.Size = new System.Drawing.Size(160, 20);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Required
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SourceLibrary.ComponentModel.Validator.IValidator Validator
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

		/// <summary>
		/// Show the dialog
		/// </summary>
		public virtual void ShowDialog()
		{
			OnDialogOpen(EventArgs.Empty);

			OnDialogClosed(EventArgs.Empty);
		}

//		private bool m_bEditTxtBoxByCode = false;

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
			}
		}

		private void btDown_Click(object sender, System.EventArgs e)
		{
			ShowDialog();
		}

		/// <summary>
		/// Select all the text of the textbox
		/// </summary>
		public void SelectAllTextBox()
		{
			txtBox.SelectAll();
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

		/// <summary>
		/// Fired when showing the drop down
		/// </summary>
		public event EventHandler DialogOpen;

		/// <summary>
		/// Fired when showing the drop down
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDialogOpen(EventArgs e)
		{
			if (DialogOpen!=null)
				DialogOpen(this,e);
		}

		/// <summary>
		/// Fired when closing the dropdown
		/// </summary>
		public event EventHandler DialogClosed;

		/// <summary>
		/// Fired when closing the dropdown
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDialogClosed(EventArgs e)
		{
			if (DialogClosed!=null)
				DialogClosed(this,e);
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
