using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;
using System.Drawing.Design;

namespace SourceLibrary.Windows.Forms
{
	/// <summary>
	/// A TextBoxTypedButton that uase the UITypeEditor associated with the type.
	/// </summary>
	public class TextBoxButtonUITypeEditor : TextBoxTypedButton, IServiceProvider, System.Windows.Forms.Design.IWindowsFormsEditorService
	{
		private System.ComponentModel.IContainer components = null;

		public TextBoxButtonUITypeEditor()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
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
			components = new System.ComponentModel.Container();
		}
		#endregion

		public override void OnLoadingValidator()
		{
			object tmp = System.ComponentModel.TypeDescriptor.GetEditor(Validator.ValueType, typeof(UITypeEditor) );
			if (tmp is UITypeEditor)
				UITypeEditor = (UITypeEditor)tmp;

			base.OnLoadingValidator ();
		}


		public override void ShowDialog()
		{
			try
			{
				OnDialogOpen(EventArgs.Empty);
				if (m_UITypeEditor != null)
				{
					UITypeEditorEditStyle l_Style = m_UITypeEditor.GetEditStyle();
					if (l_Style == UITypeEditorEditStyle.DropDown ||
						l_Style == UITypeEditorEditStyle.Modal)
					{
						object l_EditObject;
						try
						{
							l_EditObject = Value;
						}
						catch
						{
							l_EditObject = Value;
						}

						object tmp = m_UITypeEditor.EditValue(this,l_EditObject);
						Value = tmp;
					}
				}

				OnDialogClosed(EventArgs.Empty);
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message,"Error");
			}
		}

		private UITypeEditor m_UITypeEditor;
		public UITypeEditor UITypeEditor
		{
			get{return m_UITypeEditor;}
			set{m_UITypeEditor = value;}
		}

		//IServiceProvider
		System.Object IServiceProvider.GetService ( System.Type serviceType )
		{
			//modal
			if (serviceType == typeof(System.Windows.Forms.Design.IWindowsFormsEditorService))
				return this;

			return null;
		}

		#region System.Windows.Forms.Design.IWindowsFormsEditorService

		private SourceLibrary.Windows.Forms.DropDownCustom m_dropDown = null;
		public virtual void CloseDropDown ()
		{
			if (m_dropDown!=null)
			{
				m_dropDown.Hide();
			}
		}

		public virtual void DropDownControl ( System.Windows.Forms.Control control )
		{
			//NB facendo il close o il dispose probabilmente qualcosa non funziona correttamente infatti non si riesce poi pi?ad accedere all'editor
			//using(m_dropDown = new ctlDropDownCustom(this))

			m_dropDown = new SourceLibrary.Windows.Forms.DropDownCustom(this, control);
			m_dropDown.DropDownFlags = SourceLibrary.Windows.Forms.DropDownFlags.CloseOnEscape;
			//m_dropDown.ShowDialog(this); //con questo sistema non riuscivo a chiudere il controllo quando l'utente disattivava la finestra
			m_dropDown.ShowDropDown();
			m_dropDown = null;

			//m_dropDown.Close(); //non si pu?chiudere ne fare il dispose altrimenti il chiamante non riesce pi?ad accedere ai controlli figlio 
		}

		public virtual System.Windows.Forms.DialogResult ShowDialog ( System.Windows.Forms.Form dialog )
		{
			return dialog.ShowDialog(this);
		}
		#endregion
	}
}

