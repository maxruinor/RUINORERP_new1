using System;
using System.Windows.Forms;

namespace WindowsApplication23
{
    public class DataGridViewFormatColumn : DataGridViewColumn
    {
        public DataGridViewFormatColumn()
            :
            base(new DataGridViewFormatCell())
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
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewTreeViewCell)))
                {
                    throw new InvalidCastException("²»ÊÇDataGridViewTreeViewCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}
