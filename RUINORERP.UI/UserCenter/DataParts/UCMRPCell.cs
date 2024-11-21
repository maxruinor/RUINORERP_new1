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
    /// 生产计划
    /// </summary>
    public partial class UCMRPCell : UserControl
    {
        public UCMRPCell()
        {
            InitializeComponent();
        }
        UCMRP uCMRP = new UCMRP();
        private async void UCPURCell_Load(object sender, EventArgs e)
        {
            await Task.Delay(5000); // 等待5秒
            int PURListCount = await uCMRP.QueryMRPDataStatus();
            kryptonHeaderGroup1.ValuesPrimary.Heading = "【" + PURListCount.ToString() + "】计划生产中";
            krypton生产.Controls.Add(uCMRP);
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
            int PURListCount = await uCMRP.QueryMRPDataStatus();
            kryptonHeaderGroup1.ValuesPrimary.Heading = "【" + PURListCount.ToString() + "】计划生产中";
        }

        private void buttonSpecHeaderGroup2_Click(object sender, EventArgs e)
        {
            tb_MenuInfo menuinfo = MainForm.Instance.MenuList.FirstOrDefault(t => t.ClassPath == "RUINORERP.UI.MRP.UCProdWorkbench".ToString());
            if (menuinfo == null)
            {
                MainForm.Instance.PrintInfoLog("菜单关联类型为空,或您没有执行此菜单的权限，请联系管理员。");
                return;
            }
            menuPowerHelper.ExecuteEvents(menuinfo, null, null, uCMRP.PURList);
        }
    }


}
