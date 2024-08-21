using System;
using System.Windows.Forms;

namespace WindowsApplication23
{
    public class DataGridViewDateTimeColumn : DataGridViewColumn
    {
        public DataGridViewDateTimeColumn()
            : base(new DataGridViewDateTimeCell())

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
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewDateTimeCell)))
                {
                    throw new InvalidCastException("²»ÊÇDataGridViewDateTimeCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}
