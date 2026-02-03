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
using System.Diagnostics;
using RUINORERP.UI.UserCenter.DataParts;

namespace RUINORERP.UI.UserCenter.DataParts
{
    /// <summary>
    /// 销售出库
    /// </summary>
    public partial class UCSaleCell : UCBaseCell
    {
        public UCSaleCell()
        {
            InitializeComponent();
        }
        UCSale uCSale = new UCSale();
        private void UCPURCell_Load(object sender, EventArgs e)
        {
            // 仅执行UI初始化操作，不包含数据库查询
            kryptonPanelCell.Controls.Add(uCSale);
        }

        /// <summary>
        /// 重写基类的LoadData方法，实现数据加载逻辑
        /// </summary>
        public override async Task LoadData()
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                
                // 移除长时间延迟，改用更合理的加载策略
                int ListCount = await uCSale.QueryData();
                
                // 在UI线程更新界面
                this.Invoke((MethodInvoker)(() =>
                {
                    kryptonHeaderGroup1.ValuesPrimary.Heading = "【" + ListCount.ToString() + "】销售出库中";
                }));
                
                stopwatch.Stop();
                MainForm.Instance.uclog.AddLog($"初始化UCSale 执行时间：{stopwatch.ElapsedMilliseconds} 毫秒");
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, "UCSaleCell.LoadData 加载数据失败");
            }
        }
        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
 

        //进度有两种，一种是有缴库过的。另一个是没有缴库过的。这种就按步骤来算一下进度。  前者用绿色进度条。后用橙色？  没有任何进度的 用灰色？

        private async void kryptonCommandRefresh_Execute(object sender, EventArgs e)
        {
            await base._guardService.ExecuteWithGuardAsync(
           nameof(kryptonCommandRefresh_Execute),
           this.GetType().Name,
           async () =>
           {
               #region 加载工作台数据

               int ListCount = await uCSale.QueryData();
               kryptonHeaderGroup1.ValuesPrimary.Heading = "【" + ListCount.ToString() + "】销售出库中";
               #endregion
           },
           showStatusMessage: true
        );

       
        }

        private void buttonSpecHeaderGroup2_Click(object sender, EventArgs e)
        {
            tb_MenuInfo menuinfo = MainForm.Instance.MenuList.FirstOrDefault(t => t.ClassPath == "RUINORERP.UI.SAL.UCSaleWorkbench".ToString());
            if (menuinfo == null)
            {
                MainForm.Instance.PrintInfoLog("菜单关联类型为空,或您没有执行此菜单的权限，或配置菜时参数不正确。请联系管理员。");
                return;
            }
            menuPowerHelper.ExecuteEvents(menuinfo, null,  null, uCSale.SaleOrderList);
        }
    }


}
