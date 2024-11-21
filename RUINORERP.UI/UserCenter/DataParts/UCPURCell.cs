using RUINORERP.Business.Security;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using RUINORERP.Common.CollectionExtension;
using HLH.Lib.List;
using System.Linq.Expressions;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using RUINORERP.UI.Common;
using RUINORERP.Business;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Extensions;
using System.Collections.Concurrent;
using RUINORERP.Global;
using RUINORERP.Business.CommService;
using System.Windows.Navigation;
using Netron.GraphLib;
using RUINORERP.UI.BaseForm;
using RUINORERP.Common.Helper;
using Krypton.Toolkit;
using SuperSocket.ClientEngine;

namespace RUINORERP.UI.UserCenter.DataParts
{
    /// <summary>
    /// 采购
    /// </summary>
    public partial class UCPURCell : UserControl
    {
        public UCPURCell()
        {
            InitializeComponent();
        }
        UCPUR uCPUR = new UCPUR();
        private async void UCPURCell_Load(object sender, EventArgs e)
        {
            await Task.Delay(4000); // 等待5秒
           int ListCount = await uCPUR.QueryData();
            kryptonHeaderGroup1.ValuesPrimary.Heading = "【" + ListCount.ToString() + "】采购入库中";
            kryptonPanelCell.Controls.Add(uCPUR);
            timer1.Start();
        }
        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
        private void timer1_Tick(object sender, EventArgs e)
        {
            long lastInputTime = MainForm.GetLastInputTime();

            //if (MainForm.GetLastInputTime() > 100000 && kryptonTreeGridView1.Rows.Count > 0)
            //{
            //    //刷新工作台数据？
            //    QueryMRPDataStatus();
            //}

        }

        //进度有两种，一种是有缴库过的。另一个是没有缴库过的。这种就按步骤来算一下进度。  前者用绿色进度条。后用橙色？  没有任何进度的 用灰色？

        private async void kryptonCommandRefresh_Execute(object sender, EventArgs e)
        {
            int ListCount = await uCPUR.QueryData();
            kryptonHeaderGroup1.ValuesPrimary.Heading = "【" + ListCount.ToString() + "】采购入库中";
        }

        private void buttonSpecHeaderGroup2_Click(object sender, EventArgs e)
        {
            tb_MenuInfo menuinfo = MainForm.Instance.MenuList.FirstOrDefault(t => t.ClassPath == "RUINORERP.UI.PUR.UCPURWorkbench".ToString());
            if (menuinfo == null)
            {
                MainForm.Instance.PrintInfoLog("菜单关联类型为空,或您没有执行此菜单的权限，请联系管理员。");
                return;
            }
            menuPowerHelper.ExecuteEvents(menuinfo, null,  null, uCPUR.OrderList);
        }
    }


}
