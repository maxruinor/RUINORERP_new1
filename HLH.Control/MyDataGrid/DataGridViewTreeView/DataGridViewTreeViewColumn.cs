using System;
using System.Windows.Forms;

namespace WindowsApplication23
{
    public class DataGridViewTreeViewColumn : DataGridViewColumn
    {
        public DataGridViewTreeViewColumn() :
            base(new DataGridViewTreeViewCell())
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
