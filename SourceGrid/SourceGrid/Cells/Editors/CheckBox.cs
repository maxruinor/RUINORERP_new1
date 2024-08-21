using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;


namespace SourceGrid.Cells.Editors
{
	/// <summary>
	/// An editor that use a TextBoxTyped for editing support.
	/// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class CheckBox : EditorControlBase
	{
		#region Constructor
		/// <summary>
		/// Construct a Model. Based on the Type specified the constructor populate AllowNull, DefaultValue, TypeConverter, StandardValues, StandardValueExclusive
		/// </summary>
		/// <param name="p_Type">The type of this model</param>
		public CheckBox(Type p_Type):base(p_Type)
		{
		}
		#endregion

		#region Edit Control
		/// <summary>
		/// Create the editor control
		/// </summary>
		/// <returns></returns>
		protected override Control CreateControl()
		{
			DevAge.Windows.Forms.DevAgeCheckBox editor = new DevAge.Windows.Forms.DevAgeCheckBox();
			//editor.BorderStyle = BorderStyle.None;
			editor.AutoSize = false;
			editor.Validator = this;
			return editor;
		}

		/// <summary>
		/// Gets the control used for editing the cell.
		/// </summary>
		public new DevAge.Windows.Forms.DevAgeCheckBox Control
		{
			get
			{
				return (DevAge.Windows.Forms.DevAgeCheckBox)base.Control;
			}
		}
		#endregion

		/// <summary>
		/// This method is called just before the edit start. You can use this method to customize the editor with the cell informations.
		/// </summary>
		/// <param name="cellContext"></param>
		/// <param name="editorControl"></param>
		protected override void OnStartingEdit(CellContext cellContext, Control editorControl)
		{
			base.OnStartingEdit(cellContext, editorControl);

			//DevAge.Windows.Forms.DevAgeCheckBox l_TxtBox = (DevAge.Windows.Forms.DevAgeCheckBox)editorControl;
			
			 
		}

		/// <summary>
		/// Set the specified value in the current editor control.
		/// </summary>
		/// <param name="editValue"></param>
		public override void SetEditValue(object editValue)
		{
			Control.Value = editValue;
		 
		}

		/// <summary>
		/// Returns the value inserted with the current editor control
		/// </summary>
		/// <returns></returns>
		public override object GetEditedValue()
		{
			return Control.Value;
		}

		public override object GetEditedTagValue()
		{
			return Control.Tag;
		}

		protected override void OnSendCharToEditor(char key)
        {
            Control.Text = key.ToString();
            //if (Control.Text != null)
            //    Control.SelectionStart = Control.Text.Length;
        }
	}
}

