using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;


namespace SourceGrid.Cells.Editors
{
	/// <summary>
	/// Create an Editor that use a DateTimePicker as control for date editing.
	/// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class DateTimePicker : EditorControlBase
	{


		/// <summary>
		/// Constructor
		/// </summary>
		public DateTimePicker():base(typeof(System.DateTime))
		{
		}

        private DateTimePickerFormat m_Format = DateTimePickerFormat.Short;

        /// <summary>
        /// Gets or sets the format of the date and time displayed in the control.
        /// </summary>
        [DefaultValue(DateTimePickerFormat.Short)]
        [Description("Gets or sets the format of the date and time displayed in the control.")]
        public DateTimePickerFormat Format
        {
            get { return m_Format; }
            set
            {
                if (m_Format != value)
                {
                    m_Format = value;
                    OnChanged(EventArgs.Empty);
                }
            }
        }

        
		
		#region Edit Control
        /// <summary>
        /// Create the editor control
        /// </summary>
        /// <returns></returns>
        protected override Control CreateControl()
		{
			System.Windows.Forms.DateTimePicker dtPicker = new System.Windows.Forms.DateTimePicker();
            //dtPicker.Format = DateTimePickerFormat.Short;
            dtPicker.Format = Format; // 使用新添加的Format属性
            dtPicker.ShowCheckBox = AllowNull;
			return dtPicker;
		}

        protected override void OnChanged(EventArgs e)
        {
            base.OnChanged(e);

            //if the control is null the editor is not yet created
            if (Control != null)
            {
                Control.ShowCheckBox = AllowNull;
            }
        }

		/// <summary>
		/// Gets the control used for editing the cell.
		/// </summary>
		public new System.Windows.Forms.DateTimePicker Control
		{
			get
			{
				return (System.Windows.Forms.DateTimePicker)base.Control;
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

			System.Windows.Forms.DateTimePicker dtPicker = (System.Windows.Forms.DateTimePicker)editorControl;
			dtPicker.Font = cellContext.Cell.View.Font;
		}
		/// <summary>
		/// Set the specified value in the current editor control.
		/// </summary>
		/// <param name="editValue"></param>
		public override void SetEditValue(object editValue)
		{
			if (editValue is DateTime)
				Control.Value = (DateTime)editValue;
            else if (editValue == null)
                Control.Checked = false;
            else
                throw new SourceGridException("Invalid edit value, expected DateTime");
		}
		/// <summary>
		/// Returns the value inserted with the current editor control
		/// </summary>
		/// <returns></returns>
		public override object GetEditedValue()
		{
            //if (Control.Checked)
            //    return Control.Value;
            //else
            //    return null;
            if (Control.Checked)
            {
                // 根据格式决定返回日期还是日期时间
                if (Format == DateTimePickerFormat.Short || Format == DateTimePickerFormat.Long)
                    return Control.Value.Date;
                else
                    return Control.Value;
            }
            else
                return null;
        }


		public override object GetEditedTagValue()
		{
			if (Control.Checked)
				return Control.Tag;
			else
				return null;
		}

		protected override void OnSendCharToEditor(char key)
        {
            //No implementation
        }
	}
}

