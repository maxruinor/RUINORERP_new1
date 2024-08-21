using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SourceLibrary.Windows.Forms
{
	/// <summary>
	/// A TextBox that allows to set the type of value to edit, then you can use the Value property to read and write the specific type.
	/// </summary>
	public class TextBoxTyped : System.Windows.Forms.TextBox
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Constructor
		/// </summary>
		public TextBoxTyped()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			Validator = new ComponentModel.Validator.ValidatorTypeConverter(typeof(string));
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

		/// <summary>
		/// The value of the TextBox, returns an instrnace of the specified type
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Value
		{
			get
			{
				//se il testo non ?cambiato restituisco il valore precedente.
				if (m_bIsTextChanged == false)
					return m_LastValidValue;
				else if (m_Validator.IsStringConversionSupported())
				{

					try
					{
						return m_Validator.StringToValue(Text);
					}
					catch
					{
						if (EnableLastValidValue)
							return m_LastValidValue;
						else
							throw ;
					}
				}
				else
					throw new ApplicationException("TextBoxTyped not configured properly, string conversion is not supported but the text is changed");
			}

			set
			{
//				if (m_Validator.IsValidValue(value) == false)
//					throw new ApplicationException("Invalid value for the current validator");

				//ricarico il valore in modo da avere nella textbox il valore nel formato corretto
				string tmp;
				try 
				{
					if (m_Validator.IsStringConversionSupported())
						tmp = m_Validator.ValueToString(value);
					else
						tmp = m_Validator.ValueToDisplayString(value);
					if (tmp != Text)
						Text = tmp;

					if (m_errorProvider!=null)
						m_errorProvider.SetError(this,null);

					m_LastValidValue = value;
					m_bIsTextChanged = false;
				}
				catch(Exception)
				{}
			}
		}


		


        private object _tagValue;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object TagValue { get => _tagValue; set => _tagValue = value; }

        /// <summary>
        /// If true the text has changed. Returns false when set the Value property.
        /// </summary>
        private bool m_bIsTextChanged = false;

		/// <summary>
		/// Validate the content of the TextBox
		/// </summary>
		/// <returns>Returns True if the value is valid otherwise false</returns>
		public virtual bool ValidateTextBoxValue()
		{
			try
			{
				if (m_bIsTextChanged == false)
					return m_Validator.IsValidValue(Value);

				object l_val; 
				if (m_Validator.IsValidString(Text, out l_val) == false)
				{
					if (m_errorProvider!=null)
						m_errorProvider.SetError(this,m_strErrorProviderMessage);
					//non valido
					return false;
				}
				else
				{
					if (ForceFormatText)
					{
						//ricarico il valore in modo da avere nella textbox il valore nel formato corretto
						Value = l_val;
					}

					if (m_errorProvider!=null)
						m_errorProvider.SetError(this,null);
					//valido
					return true;
				}
			}
			catch(Exception)
			{
				if (m_errorProvider!=null)
					m_errorProvider.SetError(this,m_strErrorProviderMessage);

				return false;
			}
		}

		#region Properties
		private System.Windows.Forms.ErrorProvider m_errorProvider;
		/// <summary>
		/// Error provider used when a text is not valid.
		/// </summary>
		public ErrorProvider ErrorProvider
		{
			get{return m_errorProvider;}
			set{m_errorProvider = value;}
		}

		private ComponentModel.Validator.IValidator m_Validator;
		/// <summary>
		/// Type converter used for conversion
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComponentModel.Validator.IValidator Validator
		{
			get{return m_Validator;}
			set
			{
				if (value == null)
					throw new ApplicationException("Invalid Validator, cannot be null");
				m_Validator = value;
				OnLoadingValidator();
			}
		}

		/// <summary>
		/// Reload the properties from the validator
		/// </summary>
		public virtual void OnLoadingValidator()
		{
			if (LoadingValidator != null)
				LoadingValidator(this, EventArgs.Empty);

			m_LastValidValue = m_Validator.DefaultValue;
			ReadOnly = !(m_Validator.IsStringConversionSupported());
		}

		/// <summary>
		/// 
		/// </summary>
		public event EventHandler LoadingValidator;

		/// <summary>
		/// Indica l'ultimo valore impostato valido. null se non ?stato impostato nessun valore. Questo serve nel caso in cui ci sia un Validating che fallisce e viene richiesta la property Value. In questo caso si restituisce questo valore.
		/// </summary>
		private object m_LastValidValue = null;

		private string m_strErrorProviderMessage = "Invalid value";
		/// <summary>
		/// Message used with the ErrorProvider object
		/// </summary>
		public string ErrorProviderMessage
		{
			get{return m_strErrorProviderMessage;}
			set{m_strErrorProviderMessage = value;}
		}

		private bool m_bForceFormatText = true;
		/// <summary>
		/// Indicates if after the Validating event the Text is refreshed with the new value, forcing the correct formatting.
		/// </summary>
		public bool ForceFormatText
		{
			get{return m_bForceFormatText;}
			set{m_bForceFormatText = value;}
		}

		private bool m_bEnableEscapeKeyUndo = true;
		/// <summary>
		/// True to enable the Escape key to undo any changes. Default is true.
		/// </summary>
		public bool EnableEscapeKeyUndo
		{
			get{return m_bEnableEscapeKeyUndo;}
			set{m_bEnableEscapeKeyUndo = value;}
		}
		private bool m_bEnableEnterKeyValidate = true;
		/// <summary>
		/// True to enable the Enter key to validate any changes. Default is true.
		/// </summary>
		public bool EnableEnterKeyValidate
		{
			get{return m_bEnableEnterKeyValidate;}
			set{m_bEnableEnterKeyValidate = value;}
		}

		private bool m_bEnableAutoValidation = true;
		/// <summary>
		/// True to enable the validation of the textbox text when the Validating event is fired, to force always the control to be valid. Default is true.
		/// </summary>
		public bool EnableAutoValidation
		{
			get{return m_bEnableAutoValidation;}
			set{m_bEnableAutoValidation = value;}
		}

		private bool m_bEnableLastValidValue = true;
		/// <summary>
		/// True to allow the Value property to always return a valid value when the textbox.text is not valid, false to throw an error when textbox.text is not valid.
		/// </summary>
		public bool EnableLastValidValue
		{
			get{return m_bEnableLastValidValue;}
			set{m_bEnableLastValidValue = value;}
		}

		private char[] m_ValidCharacters;

		/// <summary>
		/// A list of characters allowed for the textbox. Used in the OnKeyPress event. If null no check is made.
		/// If not null any others charecters is not allowed. First the function check if ValidCharacters is not null then check for InvalidCharacters.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public char[] ValidCharacters
		{
			get{return m_ValidCharacters;}
			set{m_ValidCharacters = value;}
		}

		private char[] m_InvalidCharacters;

		/// <summary>
		/// A list of characters not allowed for the textbox. Used in the OnKeyPress event. If null no check is made.
		/// If not null any characters in the list is not allowed. First the function check if ValidCharacters is not null then check for InvalidCharacters.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public char[] InvalidCharacters
		{
			get{return m_InvalidCharacters;}
			set{m_InvalidCharacters = value;}
		}


        #endregion

        #region Override Events

        /// <summary>
        /// Raises the System.Windows.Forms.Control.TextChanged event.  
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged (e);

			m_bIsTextChanged = true;
		}


		/// <summary>
		/// Raises the System.Windows.Forms.Control.Validating event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnValidating(CancelEventArgs e)
		{
			base.OnValidating (e);
			
			if (EnableAutoValidation && ValidateTextBoxValue() == false)
				e.Cancel = true;
		}

		/// <summary>
		/// Raises the System.Windows.Forms.Control.KeyDown event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown (e);

			if (e.KeyCode == Keys.Escape && EnableEscapeKeyUndo)
			{
				Value = m_LastValidValue;
			}
		}

		/// <summary>
		/// Raises the System.Windows.Forms.Control.KeyPress event. 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress (e);

			if (e.KeyChar == 13 && EnableEnterKeyValidate && Multiline == false)
			{
				ValidateTextBoxValue();
				e.Handled = true;
			}
			else if (char.IsControl(e.KeyChar) == false) //is not a non printable character like backspace, ctrl+c, ...
			{
				if (m_ValidCharacters != null && m_ValidCharacters.Length > 0)
				{
					for (int i = 0; i < m_ValidCharacters.Length; i++)
					{
						if (e.KeyChar == m_ValidCharacters[i])
							return;
					}

					e.Handled = true;
				}
				else if (m_InvalidCharacters != null && m_InvalidCharacters.Length > 0)
				{
					for (int i = 0; i < m_InvalidCharacters.Length; i++)
					{
						if (e.KeyChar == m_InvalidCharacters[i])
						{
							e.Handled = true;
							return;
						}
					}
				}
			}
		}
		#endregion

		/// <summary>
		/// Check in the specific string if all the characters are valid
		/// </summary>
		/// <param name="p_Input"></param>
		/// <param name="p_ValidCharacters"></param>
		/// <param name="p_InvalidCharacters"></param>
		/// <returns></returns>
		public static string ValidateCharactersString(string p_Input, char[] p_ValidCharacters, char[] p_InvalidCharacters)
		{
			string tmp;

			if (p_Input != null && p_ValidCharacters != null && p_ValidCharacters.Length > 0)
			{
				tmp = "";
				for (int i = 0; i < p_Input.Length; i++)
				{
					bool l_bFound = false;
					for (int j = 0; j < p_ValidCharacters.Length; j++)
					{
						if (p_ValidCharacters[j] == p_Input[i])
						{
							l_bFound = true;
							break;
						}
					}
					if (l_bFound)
						tmp += p_Input[i];
				}
			}
			else if (p_Input != null && p_InvalidCharacters != null && p_InvalidCharacters.Length > 0)
			{
				tmp = "";
				for (int i = 0; i < p_Input.Length; i++)
				{
					bool l_bFound = false;
					for (int j = 0; j < p_InvalidCharacters.Length; j++)
					{
						if (p_InvalidCharacters[j] == p_Input[i])
						{
							l_bFound = true;
							break;
						}
					}
					if (!l_bFound)
						tmp += p_Input[i];
				}
			}
			else
				tmp = p_Input;

			return tmp;
		}
	}
}

