using System;
using System.Windows.Forms;

namespace WindowsApplication23
{
    public class DataGridViewFormatCell : DataGridViewTextBoxCell
    {
        public DataGridViewFormatCell()
        {

        }
        public override void InitializeEditingControl(int rowIndex, object
      initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            DataGridViewFormatEditingControl ctl =
                DataGridView.EditingControl as DataGridViewFormatEditingControl;
            ctl.Text = (string)this.Value;
        }

        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewFormatEditingControl);
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
