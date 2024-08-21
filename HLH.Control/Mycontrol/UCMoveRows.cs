using System;
using System.Windows.Forms;

namespace SHControls.Mycontrol
{
    public partial class UCMoveRows : UserControl
    {
        private DataGridView _myDG;


        /// <summary>
        /// 数据库安装类型的标示 0 为客户机,1 为主机  
        /// </summary>
        public DataGridView DataGridViewOfRow
        {
            get { return _myDG; }
            set { _myDG = value; }
        }


        public UCMoveRows()
        {
            InitializeComponent();
        }

        private void btnFirstRow_Click(object sender, EventArgs e)
        {
            if (this.DataGridViewOfRow.Rows.Count > 0)
            {
                DataGridViewOfRow.Rows[0].Selected = true;
                DataGridViewOfRow.FirstDisplayedScrollingRowIndex = 0;

            }
        }

        private void btnLastRow_Click(object sender, EventArgs e)
        {
            if (this.DataGridViewOfRow.Rows.Count > 0)
            {
                DataGridViewOfRow.Rows[DataGridViewOfRow.Rows.Count - 1].Selected = true;
                DataGridViewOfRow.FirstDisplayedScrollingRowIndex = DataGridViewOfRow.Rows[DataGridViewOfRow.Rows.Count - 1].Index; //垂直拉动，总是显示最后一行
            }
        }
    }
}
