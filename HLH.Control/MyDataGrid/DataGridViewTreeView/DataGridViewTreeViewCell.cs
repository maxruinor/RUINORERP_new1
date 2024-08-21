using System;
using System.Windows.Forms;

namespace WindowsApplication23
{
    public class DataGridViewTreeViewCell : DataGridViewTextBoxCell
    {
        public DataGridViewTreeViewCell()
        {

        }
        public override void InitializeEditingControl(int rowIndex, object
       initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            DataGridViewTreeViewEditingControl ctl =
                DataGridView.EditingControl as DataGridViewTreeViewEditingControl;
            ctl.Text = (string)this.Value;
        }

        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewTreeViewEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(string);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return "";
            }
        }
    }
}
