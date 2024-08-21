using System;
using System.Windows.Forms;



namespace HLH.WinControl
{

    public class PictureBoxColumn : DataGridViewColumn
    {
        public PictureBoxColumn()
            : base(new PictureBoxCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                  !value.GetType().IsAssignableFrom(typeof(PictureBoxColumn)))
                {
                    throw new InvalidCastException("Must be a PictureBoxCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class PictureBoxCell : DataGridViewImageCell
    {

        public PictureBoxCell()
            : base()
        {

            //this.ImageUrl = "";
        }

        public override void InitializeEditingControl(int rowIndex, object
          initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            PictureBoxEditingControl pbox =
                DataGridView.EditingControl as PictureBoxEditingControl;
            if (this.Value != null && this.Value.ToString() != "")
            {
                pbox.ImageLocation = this.Value.ToString();
            }
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing contol that CalendarCell uses.
                return typeof(PictureBoxEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains.
                return typeof(string);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value.
                return string.Empty;
            }
        }
    }

    class PictureBoxEditingControl : PictureBox, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public PictureBoxEditingControl()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = System.Drawing.Color.LightGreen;
        }

        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
        // property.
        public object EditingControlFormattedValue
        {
            get
            {
                return this.ImageLocation;
            }
            set
            {
                String newValue = value as String;
                if (newValue != null)
                {
                    this.ImageLocation = newValue;
                }
            }
        }



        public string ImageUrl { get; set; }

        // Implements the 
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
        public object GetEditingControlFormattedValue(
          DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        // Implements the 
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
        public void ApplyCellStyleToEditingControl(
          DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            //this= dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex 
        // property.
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey 
        // method.
        public bool EditingControlWantsInputKey(
          Keys key, bool dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed.
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return false;
            }
        }

        // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit 
        // method.
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
        }

        // Implements the IDataGridViewEditingControl
        // .RepositionEditingControlOnValueChange property.
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlDataGridView property.
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlValueChanged property.
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingPanelCursor property.
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        //protected override void OnValueChanged(EventArgs eventargs)
        //{
        //    // Notify the DataGridView that the contents of the cell
        //    // have changed.
        //    // valueChanged = true;
        //    this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
        //    //base.OnValueChanged(eventargs);
        //}


    }

}
