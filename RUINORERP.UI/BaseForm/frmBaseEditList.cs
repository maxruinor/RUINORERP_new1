using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 用于编辑时 下拉引出基本资料编辑
    /// 这个只是一个容器
    /// </summary>
    public partial class frmBaseEditList : KryptonForm
    {
        public frmBaseEditList()
        {
            InitializeComponent();
        }

        private void frmBaseEditList_Load(object sender, EventArgs e)
        {

            foreach (var item in kryptonPanel1.Controls)
            {
                if (item is BaseUControl)
                {
                    UserControl tempUc = item as BaseUControl;
                    BaseUControl buc = tempUc as BaseUControl;
                    buc.SetSelect();
                }
            }
        }
    }
}
