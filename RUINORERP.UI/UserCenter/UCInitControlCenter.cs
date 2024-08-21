using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using System.Drawing.Drawing2D;

namespace RUINORERP.UI.UserCenter
{
    [FormMark(typeof(UCInitControlCenter), "控制中心", true)]
    public partial class UCInitControlCenter : UserControl
    {
        public UCInitControlCenter()
        {
            InitializeComponent();
         }

        private void UCInitControlCenter_Load(object sender, EventArgs e)
        {
            return;
            RightPanel rp = new RightPanel();
            rp.Dock = DockStyle.Fill;
            this.kryptonPanel1.Controls.Add(rp);
            rp.LoadPage("销售流程");
            rp.ItemClick += Rp_ItemClick;
        }

        private void Rp_ItemClick(object sender, EventArgs e)
        {
            //打开对应窗体
            //参考菜单项目实现的功能
        }
    }
}
