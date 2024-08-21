using System.Windows.Forms;
namespace SHControls.MyDataGrid
{




    /// <summary>
    /// 一行中,从第一个单元格回车开始横向移动到X列, 第二次回车到第二行的第一列
    /// </summary>
    public class HorizontalMoveDG : System.Windows.Forms.DataGridView
    {
        /// <summary>
        /// 默认为下一格
        /// </summary>
        private int _PositionX = 1;

        /// <summary>
        /// 横向移动到X列
        /// </summary>
        public int PositionX
        {
            get { return _PositionX; }
            set { _PositionX = value; }
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {

            if (msg.WParam.ToInt32() == (int)Keys.Enter)
            {
                if (this.CurrentCellAddress.X == this.PositionX)
                {
                    SendKeys.Send("{Tab}");
                }
                if (this.CurrentCellAddress.X == 0)
                {
                    for (int i = 0; i < this.PositionX; i++)
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                if (this.CurrentCellAddress.X != 0 && this.CurrentCellAddress.X != PositionX)
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);

        }

    }

}
