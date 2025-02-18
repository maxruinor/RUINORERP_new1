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
using RUINORERP.UI.Common;
using System.Web.WebSockets;
using Krypton.Toolkit.Suite.Extended.TreeGridView;
using System.Linq.Expressions;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.UserCenter.DataParts
{
    public partial class UCSalePerformanceCell : UserControl
    {
        public UCSalePerformanceCell()
        {
            InitializeComponent();
        }

        private void UCSaleCell_Load(object sender, EventArgs e)
        {
            QuerySaleOrderStatus();
            timer1.Start();
        }


        int errorCount = 0;
        private void QuerySaleOrderStatus()
        {
            try
            {
                string strEmployees = string.Empty;
                List<object> EmployeeGroupList = new List<object>();
                //如果限制
                if (AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext))
                {
                    if (!EmployeeGroupList.Contains(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString()))
                    {
                        EmployeeGroupList.Add(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_ID.ToString());
                    }
                }

                string sqlquery = string.Empty;
                string WhereClause = " and 1=1 ";
                if (!strEmployees.IsNullOrEmpty())
                {
                    WhereClause += "and Employee_ID in ( " + strEmployees + " )";
                }
                #region 显示销售业绩

                tb_RoleInfo CurrentRole = MainForm.Instance.AppContext.CurrentRole;
                tb_UserInfo CurrentUser = MainForm.Instance.AppContext.CurUserInfo.UserInfo;
                //先取人，无人再取角色。
                tb_WorkCenterConfig centerConfig = MainForm.Instance.AppContext.WorkCenterConfigList.FirstOrDefault(c => c.RoleID == CurrentRole.RoleID && c.User_ID == CurrentUser.User_ID);
                if (centerConfig == null)
                {
                    centerConfig = MainForm.Instance.AppContext.WorkCenterConfigList.FirstOrDefault(c => c.RoleID == CurrentRole.RoleID);
                }
                if (centerConfig != null)
                {
                    List<string> DataOverviewItems = centerConfig.DataOverview.Split(',').ToList();
                    if (DataOverviewItems.Contains(数据概览.销售情况概览.ToString()))
                    {
                        lblMonthlyPerformance.Text = string.Empty;
                        sqlquery = string.Format("SELECT sum(c.TransactionPrice*(c.Quantity-c.TotalReturnedQty)) as 订单金额  from  tb_SaleOrder m RIGHT JOIN tb_SaleOrderDetail c on m.SOrder_ID=c.SOrder_ID WHERE m.DataStatus != 1 and  MONTH(m.SaleDate) = MONTH('{0}') " + WhereClause, System.DateTime.Now.ToString("yyyy-MM-dd"));
                        decimal orderAmount = MainForm.Instance.AppContext.Db.Ado.GetDecimal(sqlquery);
                        lblMonthlyPerformance.Text = "本月订单业绩:" + orderAmount.ToString("##,###0元");
                    }

                

                    //客户数量
                    lblMonthlyCustomer.Text = string.Empty;
                    sqlquery = string.Format("SELECT COUNT(CustomerVendor_ID) as 新增客户数 from tb_CustomerVendor  WHERE IsCustomer=1 and  MONTH(Created_at) = MONTH('{0}') " + WhereClause, System.DateTime.Now.ToString("yyyy-MM-dd"));
                    decimal NewCustomerQty = MainForm.Instance.AppContext.Db.Ado.GetDecimal(sqlquery);
                    lblMonthlyCustomer.Text = "本月新增客户:" + NewCustomerQty.ToString("##,###0个");

                }

                #endregion


                #region 显示CRM数据
                //如果启用了CRM
                //检测CRM如果没有购买则不会显示
                if (MainForm.Instance.AppContext.CanUsefunctionModules.Contains(Global.GlobalFunctionModule.客户管理系统CRM))
                {
                    
                }
                #endregion

            }
            catch (Exception ex)
            {
                errorCount++;
                // MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                MainForm.Instance.logger.Error(ex);
                if (errorCount > 10)
                {
                    timer1.Stop();
                }
            }
        }

        MenuPowerHelper menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();


        private void timer1_Tick(object sender, EventArgs e)
        {
            long lastInputTime = MainForm.GetLastInputTime();
            //if (lastInputTime > 10000 && kryptonTreeGridView1.Rows.Count > 0)
            //{
            //    MainForm.Instance.ShowMsg($"您有需要处理的销售订单。");
            //    MainForm.Instance.PrintInfoLog("您有需要处理的销售订单。");
            //}
        }

        private void kryptonCommand1_Execute(object sender, EventArgs e)
        {
            QuerySaleOrderStatus();
        }
    }
}
